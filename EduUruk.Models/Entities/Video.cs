using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Entities
{
    [Table("Video")]
    public class Video : _GlobalColumn
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public string Description { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
