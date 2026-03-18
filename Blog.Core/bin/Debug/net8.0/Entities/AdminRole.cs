using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Blog.Core.Entities
{
    [SugarTable("admin_role")]
    public partial class AdminRole
    {
        [SugarColumn(ColumnName = "admin_role_id")]
        public string AdminRoleId { get; set; }

        [SugarColumn(ColumnName = "role_name")]
        public string RoleName { get; set; }

        [SugarColumn(ColumnName = "p_role_id")]
        public string PRoleId { get; set; }

    }
}
