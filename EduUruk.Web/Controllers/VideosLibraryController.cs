using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.Entities;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduUruk.Web.Controllers
{
    public class VideosLibraryController : Controller
    {

        private readonly VideoLibraryRepo _videoLibraryRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public VideosLibraryController(VideoLibraryRepo videoLibraryRepo, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _videoLibraryRepo = videoLibraryRepo;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        //public IActionResult Index()
        //{
        //    var videosByCategory = _context.Categories
        //        .Include(category => category.Videos)  // Assuming Category has a navigation property 'Videos'
        //        .ToDictionary(category => category, category => category.Videos.ToList());

        //    return View(videosByCategory);
        //}

        //   public IActionResult Index()
        //   {
        //       var videosByCategory = _context.Categories
        //.Include(category => category.Videos)  // Include videos for each category
        //    .ThenInclude(video => video.Comments  // Include comments for each video
        //        .Select(comment => comment.User))  // Include the user who committed each comment
        //.ToDictionary(category => category, category => category.Videos.ToList());

        //       return View(videosByCategory);
        //   }
        public IActionResult Index()
        {
            // Assuming you have retrieved the videos grouped by category into a dictionary
            Dictionary<Category, List<Video>> videosByCategory = GetVideosGroupedByCategory();

            return View(videosByCategory);
        }
        //public Dictionary<Category, List<Video>> GetVideosGroupedByCategory()
        //{
        //    // Initialize the dictionary to store videos grouped by category
        //    Dictionary<Category, List<Video>> videosByCategory = new Dictionary<Category, List<Video>>();

        //    // Retrieve all categories including videos and comments
        //    var categoriesWithVideos = _context.Categories
        //        .Include(category => category.Videos)
        //            .ThenInclude(video => video.Comments)
        //        .ToList();

        //    // Iterate through each category and populate the dictionary
        //    foreach (var category in categoriesWithVideos)
        //    {
        //        // Add the category to the dictionary
        //        videosByCategory.Add(category, category.Videos.ToList());
        //    }

        //    return videosByCategory;
        //}

        public Dictionary<Category, List<Video>> GetVideosGroupedByCategory()
        {
            // Initialize the dictionary to store videos grouped by category
            Dictionary<Category, List<Video>> videosByCategory = new Dictionary<Category, List<Video>>();

            // Retrieve all categories including videos and comments
            var categoriesWithVideos = _context.Categories
                .Include(category => category.Videos.Where(x => x.IsActive == true))
                    .ThenInclude(video => video.Comments)
                     .ThenInclude(comment => comment.User) // Include the user who made the comment
                .ToList();

            // Iterate through each category and populate the dictionary
            foreach (var category in categoriesWithVideos)
            {
                // Add the category to the dictionary
                videosByCategory.Add(category, category.Videos.ToList());
            }

            return videosByCategory;
        }

        public IActionResult Index2()
        {
            var videosByCategory = _context.Categories
                .Include(category => category.Videos)
                    .ThenInclude(video => video.Comments)
                .ToDictionary(category => category, category => category.Videos.ToList());

            var videoViewModels = new List<VideoWithCommentsViewModel>();

            foreach (var categoryVideos in videosByCategory)
            {
                foreach (var video in categoryVideos.Value)
                {
                    var videoViewModel = new VideoWithCommentsViewModel
                    {
                        Id = video.Id,
                        Title = video.Title,
                        Url = video.Url,
                        Description = video.Description,
                        Comments = video.Comments != null
                            ? video.Comments.Select(comment => new CommentViewModel
                            {
                                Id = comment.Id,
                                Text = comment.Text,
                                CreatedAt = comment.CreatedAt,
                                UserId = comment.UserId,
                                UserName = comment.User != null ? comment.User.UserName : "Unknown User"
                            }).ToList()
                            : new List<CommentViewModel>()  // Handle null case by creating an empty list
                    };

                    videoViewModels.Add(videoViewModel);
                }
            }

            return View(videoViewModels);
        }



        [HttpPost]
        //[Authorize] // Requires users to be authenticated to post comments
        public async Task<IActionResult> AddComment(Guid videoId, string commentText)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video == null)
            {
                return NotFound();
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(); // Return Unauthorized if the user is not found
            }

            var comment = new Comment
            {
                Text = commentText,
                CreatedAt = DateTime.Now,
                UserId = userId,
                VideoId = videoId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirect to the video index or wherever appropriate
        }


        public IActionResult Download(Guid id)
        {
            var video = _videoLibraryRepo.FindById(id);
            var identity = _httpContextAccessor.HttpContext.User.Identity;
            if (video == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos", video.Url);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", video.Url);
        }
    }
}
