using EduUruk.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace EduUruk.Models.ViewModels
{
    public class VideoUploadModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile VideoFile { get; set; }

        public bool? IsActive { get; set; } = false;

        public List<Comment>? Comments { get; set; } // Navigation property for comments

    }

}
