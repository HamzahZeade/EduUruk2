using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Entities;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduUruk.Web.Controllers
{


    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DocumentController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult Index()
        {
            var libraries = _context.Libraries.ToList(); // Fetch all library from the database
            var categories = _context.CategoryLibraries.ToList(); // Fetch all categories

            ViewBag.Categories = categories;
            return View(libraries);
        }

        public IActionResult Edit(Guid id)
        {
            var library = _context.Libraries.Find(id); // Fetch library by id from the database

            var categories = _context.CategoryLibraries.ToList(); // Fetch all categories

            ViewBag.CategoryLibraries = categories;
            return View(library);
        }

        public IActionResult Delete(Guid id)
        {
            var library = _context.Libraries.Find(id); // Fetch library by id from the database
            return View(library);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var library = await _context.Libraries.FindAsync(id); // Fetch video by id from the database

            if (library == null)
            {
                return NotFound(); // Return a 404 Not Found response if video is not found
            }

            _context.Libraries.Remove(library); // Mark the video for deletion
            await _context.SaveChangesAsync(); // Save changes to the database

            TempData["Message"] = "library deleted successfully!"; // Use TempData to show success message on redirect

            return RedirectToAction("Index"); // Redirect to the list of videos after successful deletion
        }

        [HttpPost]
        public async Task<IActionResult> Update(Library model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            model.CreatedBy = userId;
            model.ChangedBy = userId;
            model.ChangedOn = DateTime.Now;
            try
            {
                {
                    var existingLibraries = _context.Libraries.Find(model.Id); // Fetch the existing video by ID

                    if (existingLibraries != null)
                    {
                        // Update properties of the existing video with values from the model
                        existingLibraries.Title = model.Title;
                        existingLibraries.LibraryType = model.LibraryType;
                        existingLibraries.Description = model.Description;
                        existingLibraries.CategoryLibraryId = model.CategoryLibraryId;
                        existingLibraries.StatusDocument = model.StatusDocument;
                        existingLibraries.PriceType = model.PriceType;
                        existingLibraries.Price = model.Price == null ? null : model.Price;
                        existingLibraries.IsActive = model.IsActive == null ? false : model.IsActive;
                        // Save changes to the database
                        _context.Libraries.Update(existingLibraries);
                        await _context.SaveChangesAsync();

                        ViewBag.Message = "Library updated successfully!";
                        return RedirectToAction("Index"); // Redirect to the list of videos after successful update
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Video not found.");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            // If ModelState is not valid or video was not found, return to the edit view with the model
            return View("Edit", model);
        }
        public IActionResult Upload()
        {

            var categories = _context.CategoryLibraries.ToList(); // Fetch all categories

            ViewBag.CategoryLibraries = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(DocumentUploadModel model)
        {
            if (ModelState.IsValid)
            {


                if (model.DocumentFile != null && model.DocumentFile.Length > 0)
                {
                    // Define the allowed file extensions as an array
                    string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".txt" };
                    if (!IsFileExtensionAllowed(model.DocumentFile, allowedExtensions))
                        return BadRequest("Invalid file type. Please upload a PDF, DOC, or DOCX file.");
                    if (!IsFileSizeWithinLimit(model.DocumentFile, 1024 * 1024))
                        return BadRequest("File size exceeds the maximum allowed size (1 MB).");
                    if (FileWithSameNameExists(model.DocumentFile, _context))
                        return BadRequest("Duplicate file name detected. Please upload a file with a different name.");

                    // Save the uploaded document file to a specified directory
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");
                    var filePath = Path.Combine(uploadsDir, model.DocumentFile.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.DocumentFile.CopyToAsync(fileStream);
                    }
                    // Save the uploaded book cover file if it is not null
                    string bookCoverPath = null;
                    if (model.BookCover != null && model.BookCover.Length > 0)
                    {
                        var bookCoverFilePath = Path.Combine(uploadsDir, model.BookCover.FileName);
                        using (var bookCoverStream = new FileStream(bookCoverFilePath, FileMode.Create))
                        {
                            await model.BookCover.CopyToAsync(bookCoverStream);
                        }
                        bookCoverPath = model.BookCover.FileName;
                    }

                    // Here you can save additional information about the document to your database
                    // For example: var document = new Document { Title = model.Title, Description = model.Description, FilePath = filePath };
                    // Save the document object to the database using your DbContext
                    var extension = Path.GetExtension(filePath)?.TrimStart('.').ToLower();
                    var fileType = FileExtensions(extension);

                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    var library = new Library();
                    if (userId != null)
                    {
                        library = new Library
                        {
                            Title = model.Title,
                            Type = fileType,
                            FilePath = model.DocumentFile.FileName,
                            LibraryType = model.LibraryType,
                            Description = model.Description,
                            CategoryLibraryId = model.CategoryLibraryId,
                            StatusDocument = model.StatusDocument,
                            PriceType = model.PriceType,
                            Price = model.Price == null ? null : model.Price,
                            CreatedBy = userId,
                            BookCover = bookCoverPath,
                            IsActive = model.IsActive == null ? false : model.IsActive

                        };
                    }
                    else
                    {
                        library = new Library
                        {
                            Title = model.Title,
                            Type = fileType,
                            FilePath = model.DocumentFile.FileName,
                            LibraryType = model.LibraryType,
                            Description = model.Description,
                            CategoryLibraryId = model.CategoryLibraryId,
                            StatusDocument = model.StatusDocument,
                            PriceType = model.PriceType,
                            Price = model.Price == null ? null : model.Price,
                            CreatedBy = "Vistor",
                            BookCover = bookCoverPath,
                            IsActive = model.IsActive

                        };
                    }

                    // Add the libary object to the context and save changes
                    await _context.AddAsync(library);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = "Document uploaded successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("DocumentFile", "Please select a file to upload.");
                }
            }

            return View(model);
        }

        private string FileExtensions(string? extension)
        {
            if (extension == "pdf")
            {
                return "pdf";
            }
            else if (extension == "xls" || extension == "xlsx")
            {
                return "excel";
            }
            else if (extension == "doc" || extension == "docx")
            {
                return "word";
            }
            else if (extension == "txt")
            {
                return "text";
            }

            return extension;
        }

        public static bool IsFileExtensionAllowed(IFormFile file, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(extension);
        }

        public static bool IsFileSizeWithinLimit(IFormFile file, long maxSizeInBytes)
        {
            return file.Length <= maxSizeInBytes;
        }

        public static bool FileWithSameNameExists(IFormFile fileName, ApplicationDbContext context)
        {
            // Check if a file with the same name exists in the Library table
            var Check = context.Libraries.Any(v => v.FilePath == fileName.FileName.ToString());
            return Check;
        }
    }
}
