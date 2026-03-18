using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Simple.CaseConverter.Extensions;
using SqlSugar;

namespace Blog.Core.Utils
{
    public static class EntityGeneratorUtil
    {
        /// <summary>
        /// 根据数据库生成实体类文件到指定输出目录。
        /// - 表名、字段名、生成的类名与文件名均使用 Simple.CaseConverter 转为 PascalCase（例如 create_at -> CreateAt）。
        /// - 时间类型字段添加 DisplayFormat 将时间格式化为 "yyyy-MM-dd HH:mm:ss"，并添加 JsonConverter。
        /// - 字段名为 create_at（不区分大小写）时添加 SqlSugar 的 InsertSql 自动插入当前时间。
        /// - 字段名为 update_at（不区分大小写）时添加 SqlSugar 的 InsertSql 与 UpdateSql 自动插入/更新当前时间。
        /// - 对主键/自增字段添加相应的特性。
        /// </summary>
        public static void GenerateEntity(string connectionStr, string outputPath, SqlSugar.DbType dbType = DbType.MySql)
        {
            if (string.IsNullOrWhiteSpace(connectionStr)) throw new ArgumentException("connectionStr不能为空", nameof(connectionStr));
            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentException("outputPath不能为空", nameof(outputPath));

            Directory.CreateDirectory(outputPath);

            string GetNowSql(SqlSugar.DbType type) => type switch
            {
                SqlSugar.DbType.SqlServer => "GETDATE()",
                SqlSugar.DbType.MySql => "CURRENT_TIMESTAMP",
                SqlSugar.DbType.MySqlConnector => "CURRENT_TIMESTAMP",
                SqlSugar.DbType.PostgreSQL => "NOW()",
                SqlSugar.DbType.Sqlite => "CURRENT_TIMESTAMP",
                SqlSugar.DbType.Oracle => "SYSDATE",
                _ => "CURRENT_TIMESTAMP"
            };

            var nowSql = GetNowSql(dbType);

            var config = new ConnectionConfig()
            {
                ConnectionString = connectionStr,
                DbType = dbType,
                IsAutoCloseConnection = true
            };

            using var db = new SqlSugarClient(config);

            var tables = db.DbMaintenance.GetTableInfoList();
            foreach (var table in tables)
            {
                var rawTableName = (table.Name ?? "").ToString();
                if (string.IsNullOrWhiteSpace(rawTableName)) continue;

                var shortTableName = rawTableName.Contains('.') ? rawTableName.Split('.').Last() : rawTableName;
                var className = shortTableName.ToPascalCase();

                var columns = db.DbMaintenance.GetColumnInfosByTableName(rawTableName);
                var sb = new StringBuilder();

                sb.AppendLine("using System;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                sb.AppendLine("using SqlSugar;");
                sb.AppendLine("using System.Text.Json.Serialization;");
                sb.AppendLine("using Blog.Core.Commons;");
                sb.AppendLine();
                sb.AppendLine("namespace Blog.Core.Entities");
                sb.AppendLine("{");
                sb.AppendLine($" [SugarTable(\"{shortTableName}\")]\n");
                sb.AppendLine($" public partial class {className}");
                sb.AppendLine(" {");

                foreach (var col in columns)
                {
                    var colName = (col.DbColumnName ?? col.PropertyName ?? "").ToString();
                    if (string.IsNullOrWhiteSpace(colName)) continue;

                    var propName = colName.ToPascalCase();

                    // 尝试读取数据库类型描述
                    var dbTypeName = (col.DataType ?? "").ToString();
                    var csTypeRaw = col.PropertyType != null ? col.PropertyType.Name : "String";

                    // 判断是否可空
                    var isNullable = false;
                    try { isNullable = Convert.ToBoolean(col.IsNullable); } catch { isNullable = false; }

                    // 映射 SQL 类型或 CLR 类型到 C# 类型关键字
                    string MapToCSharpType(string dbTypeString, string clrName)
                    {
                        var t = (dbTypeString ?? string.Empty).ToLowerInvariant();
                        if (string.IsNullOrWhiteSpace(t)) t = (clrName ?? string.Empty).ToLowerInvariant();

                        if (t.Contains("tinyint") && (col.Length == 1 || t.EndsWith("(1)"))) return "bool"; // tinyint(1) as bool
                        if (t.Contains("bool") || t == "bit") return "bool";
                        if (t.Contains("bigint") || t.Contains("int64")) return "long";
                        if (t.Contains("int") || t.Contains("integer") || t.Contains("int32")) return "int";
                        if (t.Contains("smallint") || t.Contains("int16")) return "short";
                        if (t.Contains("decimal")) return "decimal";
                        if (t.Contains("double")) return "double";
                        if (t.Contains("float") || t.Contains("real")) return "float";
                        if (t.Contains("datetimeoffset") || t.Contains("timestamp") || t.Contains("datetime") || t.Contains("date")) return "DateTime";
                        if (t.Contains("uniqueidentifier") || t.Contains("uuid") || t.Contains("guid")) return "Guid";
                        if (t.Contains("char") || t.Contains("text") || t.Contains("varchar") || t.Contains("nvarchar")) return "string";

                        // fallback to CLR name
                        var clr = (clrName ?? "").ToLowerInvariant();
                        if (clr.Contains("datetime")) return "DateTime";
                        if (clr.Contains("int64") || clr.Contains("long")) return "long";
                        if (clr.Contains("int32") || clr.Contains("int")) return "int";
                        if (clr.Contains("boolean") || clr.Contains("bool")) return "bool";

                        return "string";
                    }

                    var mappedType = MapToCSharpType(dbTypeName, csTypeRaw);

                    // 值类型在可空时加 '?'
                    var valueTypes = new[] { "int", "long", "short", "byte", "decimal", "double", "float", "bool", "DateTime", "Guid" };
                    string typeForProp = mappedType;
                    if (isNullable && valueTypes.Contains(mappedType) && !mappedType.EndsWith("?"))
                    {
                        typeForProp = mappedType + "?";
                    }

                    // 属性描述注释
                    var colDesc = (col.ColumnDescription ?? "").ToString();
                    if (!string.IsNullOrWhiteSpace(colDesc))
                    {
                        sb.AppendLine($" /// <summary>");
                        sb.AppendLine($" /// {colDesc}");
                        sb.AppendLine($" /// </summary>");
                    }

                    // 特性集合
                    var attrs = new List<string>();

                    // 主键/自增
                    var isPrimary = false;
                    var isIdentity = false;
                    try
                    {
                        isPrimary = Convert.ToBoolean(col.IsPrimarykey);
                    }
                    catch { try { isPrimary = Convert.ToBoolean(col.IsPrimarykey); } catch { isPrimary = false; } }
                    try
                    {
                        isIdentity = Convert.ToBoolean(col.IsIdentity);
                    }
                    catch { isIdentity = false; }

                    if (isPrimary)
                    {
                        attrs.Add("[Key]");
                    }

                    // 时间类型：添加 DisplayFormat + JsonConverter
                    if (typeForProp.IndexOf("DateTime", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        attrs.Add("[DisplayFormat(DataFormatString = \"{0:yyyy-MM-dd HH:mm:ss}\", ApplyFormatInEditMode = true)]");
                        var converterName = typeForProp.EndsWith("?") ? "JsonNullableDateTimeConverter" : "JsonDateTimeConverter";
                        attrs.Add($"[JsonConverter(typeof({converterName}))]");
                    }

                    // 字符串长度限制
                    if (mappedType == "string")
                    {
                        try
                        {
                            if (col.Length > 0)
                            {
                                attrs.Add($"[MaxLength({col.Length})]");
                            }
                        }
                        catch { }

                        if (!isNullable)
                        {
                            attrs.Add("[Required]");
                        }
                    }

                    // 构建 SugarColumn 特性
                    var sugarSb = new StringBuilder();
                    sugarSb.Append("[SugarColumn(");
                    sugarSb.Append($"ColumnName = \"{colName}\"");
                    if (isPrimary)
                    {
                        sugarSb.Append(", IsPrimaryKey = true");
                    }
                    if (isIdentity)
                    {
                        sugarSb.Append(", IsIdentity = true");
                    }
                    if (string.Equals(colName, "create_at", StringComparison.OrdinalIgnoreCase))
                    {
                        sugarSb.Append($", InsertSql = \"{nowSql}\"");
                    }
                    else if (string.Equals(colName, "update_at", StringComparison.OrdinalIgnoreCase))
                    {
                        sugarSb.Append($", InsertSql = \"{nowSql}\", UpdateSql = \"{nowSql}\"");
                    }

                    sugarSb.Append(")]");
                    attrs.Add(sugarSb.ToString());

                    // 写入特性
                    foreach (var a in attrs)
                    {
                        sb.AppendLine($" {a}");
                    }

                    sb.AppendLine($" public {typeForProp} {propName} {{ get; set; }}");
                    sb.AppendLine();
                }

                sb.AppendLine(" }");
                sb.AppendLine("}");

                var filePath = Path.Combine(outputPath, $"{className}.cs");
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
        }
    }
}
