using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_user")]

    public partial class BlogUser
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_user_id", IsPrimaryKey = true)]
        public long BlogUserId { get; set; }

        /// <summary>
        /// 登录用户名，唯一
        /// </summary>
        [MaxLength(100)]
        [Required]
        [SugarColumn(ColumnName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// 用户邮箱，唯一
        /// </summary>
        [MaxLength(200)]
        [Required]
        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// 密码哈希（不可逆）
        /// </summary>
        [MaxLength(255)]
        [Required]
        [SugarColumn(ColumnName = "password_hash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// 显示名称 / 昵称
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(ColumnName = "display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 头像 URL
        /// </summary>
        [MaxLength(255)]
        [SugarColumn(ColumnName = "avatar_url")]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 账户状态：1=激活，0=禁用
        /// </summary>
        [SugarColumn(ColumnName = "status")]
        public int Status { get; set; }

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
        /// 记录更新时间（由数据库自动更新）
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

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        [SugarColumn(ColumnName = "last_login_at")]
        public DateTime? LastLoginAt { get; set; }

    }
}
