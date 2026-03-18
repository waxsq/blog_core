using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("admin_user")]
    public partial class AdminUser
    {
        [SugarColumn(ColumnName = "admin_user_id")]
        public string AdminUserId { get; set; }

        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }

        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; }

        [SugarColumn(ColumnName = "is_valid")]
        public string IsValid { get; set; }

        [SugarColumn(ColumnName = "create_by")]
        public string CreateBy { get; set; }

        [SugarColumn(ColumnName = "update_by")]
        public string UpdateBy { get; set; }

        [SugarColumn(ColumnName = "create_date")]
        public string CreateDate { get; set; }

        [SugarColumn(ColumnName = "update_date")]
        public string UpdateDate { get; set; }

    }
}
