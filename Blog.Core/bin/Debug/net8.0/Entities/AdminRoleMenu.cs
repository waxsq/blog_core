using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("admin_role_menu")]
    public partial class AdminRoleMenu
    {
        [SugarColumn(ColumnName = "admin_role_menu_id")]
        public string AdminRoleMenuId { get; set; }

        [SugarColumn(ColumnName = "admin_role_id")]
        public string AdminRoleId { get; set; }

        [SugarColumn(ColumnName = "admin_menu_id")]
        public string AdminMenuId { get; set; }

        [SugarColumn(ColumnName = "create_by")]
        public string CreateBy { get; set; }

        [SugarColumn(ColumnName = "update_by")]
        public string UpdateBy { get; set; }

        [SugarColumn(ColumnName = "create_date")]
        public string CreateDate { get; set; }

        [SugarColumn(ColumnName = "update_date")]
        public string UpdateDate { get; set; }

        [SugarColumn(ColumnName = "is_valid")]
        public string IsValid { get; set; }

    }
}
