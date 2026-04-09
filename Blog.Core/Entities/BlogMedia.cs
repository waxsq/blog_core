using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_media")]

    public partial class BlogMedia
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_media_id", IsPrimaryKey = true)]
        public long BlogMediaId { get; set; }

        /// <summary>
        /// 文件原始名
        /// </summary>
        [MaxLength(255)]
        [Required]
        [SugarColumn(ColumnName = "file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// 存储路径（相对或绝对）
        /// </summary>
        [MaxLength(500)]
        [SugarColumn(ColumnName = "storage_path")]
        public string StoragePath { get; set; }

        /// <summary>
        /// 可访问 URL（CDN 或直连）
        /// </summary>
        [MaxLength(1000)]
        [SugarColumn(ColumnName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// MIME 类型
        /// </summary>
        [MaxLength(100)]
        [SugarColumn(ColumnName = "mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        [SugarColumn(ColumnName = "size")]
        public long? Size { get; set; }

        /// <summary>
        /// 图片宽度（若适用）
        /// </summary>
        [SugarColumn(ColumnName = "width")]
        public int? Width { get; set; }

        /// <summary>
        /// 图片高度（若适用）
        /// </summary>
        [SugarColumn(ColumnName = "height")]
        public int? Height { get; set; }

        /// <summary>
        /// 上传者用户 id
        /// </summary>
        [SugarColumn(ColumnName = "uploader_id")]
        public long? UploaderId { get; set; }

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
