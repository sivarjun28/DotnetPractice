namespace BlogSystem.Models.Entities
{
    public class Tag
    {
        public int Id{get; set;}
        public string Name{get; set;} = string.Empty;
        public int UsageCount{get; set;}

        public List<Post> Posts{get; set;} = new();

    }
}