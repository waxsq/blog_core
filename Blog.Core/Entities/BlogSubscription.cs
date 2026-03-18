using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_subscription")]

    public partial class BlogSubscription
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_subscription_id", IsPrimaryKey = true)]
        public long BlogSubscriptionId { get; set; }

        /// <summary>
        /// 订阅邮箱
        /// </summary>
        [MaxLength(200)]
        [Required]
        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// 是否已确认订阅：1=是，0=否
        /// </summary>
        [SugarColumn(ColumnName = "is_confirmed")]
        public int IsConfirmed { get; set; }

        /// <summary>
        /// 确认令牌（用于验证）
        /// </summary>
        [MaxLength(200)]
        [SugarColumn(ColumnName = "confirm_token")]
        public string ConfirmToken { get; set; }

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
