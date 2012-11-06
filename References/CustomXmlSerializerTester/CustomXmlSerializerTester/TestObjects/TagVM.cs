
namespace CallView360_WebApp.ViewModels
{
    public class TagVM
    {
        public string Id { get; set; }
        public string TagName { get; set; }
        public string Tag { get; set; }
        public TagVM()
        { }
        public TagVM(string Id, string Name, string Tag)
        {
            this.Id = Id;
            this.TagName = Name;
            this.Tag = Tag;
        }
    }
}