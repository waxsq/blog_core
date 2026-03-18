using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("blog_tag")]
    public partial class BlogTag
    {
        [SugarColumn(ColumnName = "blog_tag_id")]
        public string BlogTagId { get; set; }

        [SugarColumn(ColumnName = "tag_name")]
        public string TagName { get; set; }

        [SugarColumn(ColumnName = "tag_code")]
        public string TagCode { get; set; }

        [SugarColumn(ColumnName = "is_valid")]
        public string IsValid { get; set; }

        [SugarColumn(ColumnName = "create_by")]
        public string CreateBy { get; set; }

        [SugarColumn(ColumnName = "update_by")]
        public string UpdateBy { get; set; }

        [SugarColumn(ColumnName = "create_at", InsertSql = "CURRENT_TIMESTAMP")]
        public string CreateAt { get; set; }

        [SugarColumn(ColumnName = "update_at", InsertSql = "CURRENT_TIMESTAMP", UpdateSql = "CURRENT_TIMESTAMP")]
        public string UpdateAt { get; set; }

    }
}
