using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("admin_user_role")]
    public partial class AdminUserRole
    {
        [SugarColumn(ColumnName = "admin_user_role_id")]
        public string AdminUserRoleId { get; set; }

        [SugarColumn(ColumnName = "admin_user_id")]
        public string AdminUserId { get; set; }

        [SugarColumn(ColumnName = "admin_role_id")]
        public string AdminRoleId { get; set; }

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
