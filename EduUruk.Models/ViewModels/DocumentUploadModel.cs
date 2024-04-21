using EduUruk.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace EduUruk.Models.ViewModels
{
    public class DocumentUploadModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid CategoryLibraryId { get; set; }
        public IFormFile DocumentFile { get; set; }
        public IFormFile BookCover { get; set; }

        public LibraryType LibraryType { get; set; } // Add LibraryType property

        public StatusDocument StatusDocument { get; set; } // Enum property for type
        public PriceType PriceType { get; set; } // Enum property for type
        public decimal? Price { get; set; } // Price field


        public bool? IsActive { get; set; } = false;
    }

}
