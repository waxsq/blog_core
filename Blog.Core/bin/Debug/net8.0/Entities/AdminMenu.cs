using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("admin_menu")]
    public partial class AdminMenu
    {
        [SugarColumn(ColumnName = "admin_menu_id")]
        public string AdminMenuId { get; set; }

        [SugarColumn(ColumnName = "menu_name")]
        public string MenuName { get; set; }

        [SugarColumn(ColumnName = "p_menu_id")]
        public string PMenuId { get; set; }

        [SugarColumn(ColumnName = "order")]
        public string Order { get; set; }

        [SugarColumn(ColumnName = "menu_url")]
        public string MenuUrl { get; set; }

        [SugarColumn(ColumnName = "is_valid")]
        public string IsValid { get; set; }

        [SugarColumn(ColumnName = "menu_icon")]
        public string MenuIcon { get; set; }

        [SugarColumn(ColumnName = "is_default")]
        public string IsDefault { get; set; }

        [SugarColumn(ColumnName = "create_by")]
        public string CreateBy { get; set; }

        [SugarColumn(ColumnName = "create_date")]
        public string CreateDate { get; set; }

        [SugarColumn(ColumnName = "update_by")]
        public string UpdateBy { get; set; }

        [SugarColumn(ColumnName = "update_date")]
        public string UpdateDate { get; set; }

        [SugarColumn(ColumnName = "menu_code")]
        public string MenuCode { get; set; }

    }
}
