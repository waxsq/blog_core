using System.ComponentModel.DataAnnotations;
using Blog.Core.Commons;

namespace Blog.MvcWeb.Areas.Admin.Models.Tag
{
    public class TagListModels
    {
        public long BlogTagId { get; set; }
        public string TagName { get; set; }
        public string TagCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateAt { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
    }
}
