using EduUruk.Models.Auth_Tables;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; } // Foreign key to link to the user who posted the comment
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public Guid VideoId { get; set; } // Foreign key to link to the video
        [ForeignKey("VideoId")]
        public virtual Video Video { get; set; } // Navigation property for video
    }
}
