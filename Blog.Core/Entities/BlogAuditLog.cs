using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_audit_log")]

    public partial class BlogAuditLog
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_audit_log_id", IsPrimaryKey = true)]
        public long BlogAuditLogId { get; set; }

        /// <summary>
        /// 触发操作的用户 id（可空）
        /// </summary>
        [SugarColumn(ColumnName = "user_id")]
        public long? UserId { get; set; }

        /// <summary>
        /// 操作名称或类型
        /// </summary>
        [MaxLength(150)]
        [Required]
        [SugarColumn(ColumnName = "action")]
        public string Action { get; set; }

        /// <summary>
        /// 操作涉及的实体类型
        /// </summary>
        [MaxLength(150)]
        [SugarColumn(ColumnName = "entity")]
        public string Entity { get; set; }

        /// <summary>
        /// 操作涉及的实体 id
        /// </summary>
        [SugarColumn(ColumnName = "entity_id")]
        public long? EntityId { get; set; }

        /// <summary>
        /// 操作相关的数据快照或上下文（JSON）
        /// </summary>
        [SugarColumn(ColumnName = "data")]
        public string Data { get; set; }

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
