using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_page")]

    public partial class BlogPage
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_page_id", IsPrimaryKey = true)]
        public long BlogPageId { get; set; }

        /// <summary>
        /// 作者 id（引用 blog_user）
        /// </summary>
        [SugarColumn(ColumnName = "author_id")]
        public long? AuthorId { get; set; }

        /// <summary>
        /// 页面标题
        /// </summary>
        [MaxLength(255)]
        [Required]
        [SugarColumn(ColumnName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 页面 URL 友好标识，唯一
        /// </summary>
        [MaxLength(255)]
        [SugarColumn(ColumnName = "slug")]
        public string Slug { get; set; }

        /// <summary>
        /// 页面内容（Markdown 或 HTML）
        /// </summary>
        [SugarColumn(ColumnName = "content")]
        public string Content { get; set; }

        /// <summary>
        /// 是否已发布：1=是，0=否
        /// </summary>
        [SugarColumn(ColumnName = "is_published")]
        public int IsPublished { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        [SugarColumn(ColumnName = "published_at")]
        public DateTime? PublishedAt { get; set; }

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
