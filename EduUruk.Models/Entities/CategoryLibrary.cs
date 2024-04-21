namespace EduUruk.Models.Entities
{
    public class CategoryLibrary
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Library> Libraries { get; set; }
    }
}

