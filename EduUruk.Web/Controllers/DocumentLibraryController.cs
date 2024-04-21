using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduUruk.Web.Controllers
{



    public class DocumentLibraryController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly VideoLibraryRepo _videoLibraryRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DocumentLibraryController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        //public IActionResult Index()
        //{
        //    var allDocuments = _context.Libraries.ToList();
        //    return View(allDocuments);
        //}

        public IActionResult Index()
        {
            var videosByCategory = _context.CategoryLibraries
                .Include(category => category.Libraries.Where(x => x.IsActive == true))  // Assuming Category has a navigation property 'Librarues'
                .ToDictionary(category => category, category => category.Libraries.ToList());

            return View(videosByCategory);
        }
        public IActionResult Download(Guid id)
        {
            var library = _context.Libraries.Find(id);

            if (library == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents", library.FilePath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", library.FilePath);
        }


        // Action to get image URL by document ID
        public IActionResult GetImage(Guid id)
        {
            // Fetch the document from the database by ID
            var document = _context.Libraries.FirstOrDefault(d => d.Id == id);

            if (document != null)
            {
                // Combine the file path with the uploads/documents directory
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents", document.BookCover);

                // Check if the file exists in the specified directory
                if (System.IO.File.Exists(filePath))
                {
                    // Return the local file path as the image URL
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "image/jpeg"); // Adjust the content type based on your file type
                }
            }

            // Return a default or error image if document is not found or file does not exist
            return File("/images/default.jpg", "image/jpeg"); // Example default image URL
        }

    }
}
