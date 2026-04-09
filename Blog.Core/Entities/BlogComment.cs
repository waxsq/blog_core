using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_comment")]

    public partial class BlogComment
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_comment_id", IsPrimaryKey = true)]
        public long BlogCommentId { get; set; }

        /// <summary>
        /// 文章 id（引用 blog_post）
        /// </summary>
        [SugarColumn(ColumnName = "post_id")]
        public long PostId { get; set; }

        /// <summary>
        /// 评论者用户 id（游客可为空）
        /// </summary>
        [SugarColumn(ColumnName = "user_id")]
        public long? UserId { get; set; }

        /// <summary>
        /// 父评论 id（用于回复树）
        /// </summary>
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Required]
        [SugarColumn(ColumnName = "content")]
        public string Content { get; set; }

        /// <summary>
        /// 是否审核通过：1=通过，0=待审/拒绝
        /// </summary>
        [SugarColumn(ColumnName = "is_approved")]
        public int IsApproved { get; set; }

        /// <summary>
        /// 评论点赞数
        /// </summary>
        [SugarColumn(ColumnName = "likes_count")]
        public int LikesCount { get; set; }

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
