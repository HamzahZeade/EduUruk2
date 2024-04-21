using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Entities
{



    [Table("Library")]
    public class Library : _GlobalColumn
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; } // String property for type
        public LibraryType LibraryType { get; set; } // Enum property for type
        public StatusDocument StatusDocument { get; set; } // Enum property for type
        public PriceType PriceType { get; set; } // Enum property for type
        public decimal? Price { get; set; } // Price field

        public string Description { get; set; }

        public string BookCover { get; set; }

        public Guid CategoryLibraryId { get; set; }
        [ForeignKey("CategoryLibraryId")]
        public virtual CategoryLibrary? CategoryLibrary { get; set; }
    }

    public enum LibraryType
    {
        Book,
        Summary
    }

    public enum StatusDocument
    {
        New,
        Used
    }
    public enum PriceType
    {
        price,
        Free
    }


}
