using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Blog.Core.Commons;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_category")]

    public partial class BlogCategory
    {
        /// <summary>
        /// 主键（应用生成的 long）
        /// </summary>
        [Key]
        [SugarColumn(ColumnName = "blog_category_id", IsPrimaryKey = true)]
        public long BlogCategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [MaxLength(150)]
        [Required]
        [SugarColumn(ColumnName = "category_name")]
        public string CategoryName { get; set; }

        /// <summary>
        /// URL 友好标识
        /// </summary>
        [MaxLength(150)]
        [SugarColumn(ColumnName = "slug")]
        public string Slug { get; set; }

        /// <summary>
        /// 父级分类 id
        /// </summary>
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }

        /// <summary>
        /// 排序权重（越大越靠前）
        /// </summary>
        [SugarColumn(ColumnName = "sort_order")]
        public int? SortOrder { get; set; }

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

        [SugarColumn(ColumnName = "is_valid")]
        public int isValid { get; set; } = 1;

        [SugarColumn(ColumnName = "ref_count")]
        public int RefCount { get; set; } = 0;
    }
}
