using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_setting")]

    public partial class BlogSetting
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_setting_id", IsPrimaryKey = true)]
        public long BlogSettingId { get; set; }

        /// <summary>
        /// 配置项键名（唯一）
        /// </summary>
        [MaxLength(150)]
        [Required]
        [SugarColumn(ColumnName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// 配置项值（JSON 或字符串）
        /// </summary>
        [SugarColumn(ColumnName = "value")]
        public string Value { get; set; }

        /// <summary>
        /// 配置类型/分类
        /// </summary>
        [MaxLength(50)]
        [SugarColumn(ColumnName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// 配置说明
        /// </summary>
        [MaxLength(255)]
        [SugarColumn(ColumnName = "description")]
        public string Description { get; set; }

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
