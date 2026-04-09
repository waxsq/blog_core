using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_oauth_account")]

    public partial class BlogOauthAccount
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_oauth_account_id", IsPrimaryKey = true)]
        public long BlogOauthAccountId { get; set; }

        /// <summary>
        /// 关联的用户 id（可空）
        /// </summary>
        [SugarColumn(ColumnName = "user_id")]
        public long? UserId { get; set; }

        /// <summary>
        /// 第三方提供商名称（如 github）
        /// </summary>
        [MaxLength(100)]
        [Required]
        [SugarColumn(ColumnName = "provider")]
        public string Provider { get; set; }

        /// <summary>
        /// 第三方用户唯一 id
        /// </summary>
        [MaxLength(200)]
        [Required]
        [SugarColumn(ColumnName = "provider_user_id")]
        public string ProviderUserId { get; set; }

        /// <summary>
        /// 访问令牌（可能为长文本）
        /// </summary>
        [SugarColumn(ColumnName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [SugarColumn(ColumnName = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 令牌到期时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        [SugarColumn(ColumnName = "expires_at")]
        public DateTime? ExpiresAt { get; set; }

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
