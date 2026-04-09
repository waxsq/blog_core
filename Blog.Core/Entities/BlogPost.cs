using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_post")]

    public partial class BlogPost
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_post_id", IsPrimaryKey = true)]
        public long BlogPostId { get; set; }

        /// <summary>
        /// 作者 id（引用 blog_user）
        /// </summary>
        [SugarColumn(ColumnName = "author_id")]
        public long? AuthorId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [MaxLength(255)]
        [Required]
        [SugarColumn(ColumnName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 文章 URL 友好标识，唯一
        /// </summary>
        [MaxLength(255)]
        [SugarColumn(ColumnName = "slug")]
        public string Slug { get; set; }

        /// <summary>
        /// 文章摘要
        /// </summary>
        [SugarColumn(ColumnName = "summary")]
        public string Summary { get; set; }

        /// <summary>
        /// 文章内容（Markdown 或 HTML）
        /// </summary>
        [SugarColumn(ColumnName = "content")]
        public string Content { get; set; }

        /// <summary>
        /// 文章状态：0=草稿，1=已发布，2=已归档
        /// </summary>
        [SugarColumn(ColumnName = "status")]
        public int Status { get; set; } = 1;

        /// <summary>
        /// 是否推荐/精选
        /// </summary>
        [SugarColumn(ColumnName = "is_featured")]
        public int IsFeatured { get; set; } = 0;

        /// <summary>
        /// 是否置顶
        /// </summary>
        [SugarColumn(ColumnName = "is_top")]
        public int IsTop { get; set; } = 0;

        /// <summary>
        /// 分类 id（引用 blog_category）
        /// </summary>
        [SugarColumn(ColumnName = "category_id")]
        public long? CategoryId { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [SugarColumn(ColumnName = "views_count")]
        public long ViewsCount { get; set; } = 0;

        /// <summary>
        /// 评论数量
        /// </summary>
        [SugarColumn(ColumnName = "comments_count")]
        public int CommentsCount { get; set; } = 0;

        /// <summary>
        /// 点赞数量
        /// </summary>
        [SugarColumn(ColumnName = "likes_count")]
        public int LikesCount { get; set; } = 0;

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
