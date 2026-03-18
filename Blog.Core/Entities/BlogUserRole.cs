using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_user_role")]

    public partial class BlogUserRole
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_user_role_id", IsPrimaryKey = true)]
        public long BlogUserRoleId { get; set; }

        /// <summary>
        /// 用户 id（引用 blog_user）
        /// </summary>
        [SugarColumn(ColumnName = "user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 角色 id（引用 blog_role）
        /// </summary>
        [SugarColumn(ColumnName = "role_id")]
        public long RoleId { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        [SugarColumn(ColumnName = "create_at", InsertSql = "CURRENT_TIMESTAMP")]
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 记录创建者（用户 id）
        /// </summary>
        [SugarColumn(ColumnName = "create_by")]
        public long? CreateBy { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        [SugarColumn(ColumnName = "update_at", InsertSql = "CURRENT_TIMESTAMP", UpdateSql = "CURRENT_TIMESTAMP")]
        public DateTime UpdateAt { get; set; }

        /// <summary>
        /// 记录更新者（用户 id）
        /// </summary>
        [SugarColumn(ColumnName = "update_by")]
        public long? UpdateBy { get; set; }

    }
}
