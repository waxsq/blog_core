using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_visit")]

    public partial class BlogVisit
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_visit_id", IsPrimaryKey = true)]
        public long BlogVisitId { get; set; }

        /// <summary>
        /// 访问对应的文章 id（可空，记录其他 path 时为空）
        /// </summary>
        [SugarColumn(ColumnName = "post_id")]
        public long? PostId { get; set; }

        /// <summary>
        /// 访问路径或页面标识
        /// </summary>
        [MaxLength(255)]
        [SugarColumn(ColumnName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// 访问者 IP
        /// </summary>
        [MaxLength(45)]
        [SugarColumn(ColumnName = "ip")]
        public string Ip { get; set; }

        /// <summary>
        /// User-Agent 信息
        /// </summary>
        [MaxLength(512)]
        [SugarColumn(ColumnName = "user_agent")]
        public string UserAgent { get; set; }

        /// <summary>
        /// 来源页面 URL
        /// </summary>
        [MaxLength(512)]
        [SugarColumn(ColumnName = "referrer")]
        public string Referrer { get; set; }

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
