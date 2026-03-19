namespace Blog.MvcWeb.Models.Tag
{
    public class TagAddOrEditVo
    {
        public long BlogTagId {  get; set; }
        public string TagName { get; set; }
        public string TagCode { get; set; }
        public int IsValid { get; set; }
    }
}
