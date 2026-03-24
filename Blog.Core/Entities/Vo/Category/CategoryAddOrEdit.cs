using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Blog.Core.Entities.Vo.Category
{
    public class CategoryAddOrEdit
    {
        public long BlogCategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? IsValid { get; set; }

        public string? Action { get; set; } = "View";
    }
}
