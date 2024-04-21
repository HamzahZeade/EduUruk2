using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Entities;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
namespace EduUruk.Web.Controllers
{

    [Authorize(Roles = "Admin")]
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VideoController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var videos = _context.Videos.Include(x => x.Category).ToList(); // Fetch all videos from the database
            return View(videos);
        }
        //public IActionResult Index()
        //{
        //	var videosByCategory = _context.Categories
        //		.Include(category => category.Videos)  // Assuming Category has a navigation property 'Videos'
        //		.ToDictionary(category => category, category => category.Videos.ToList());

        //	return View(videosByCategory);
        //}
        public IActionResult Edit(Guid id)
        {
            var video = _context.Videos.Find(id); // Fetch video by id from the database
            var categories = _context.Categories.ToList(); // Fetch all categories

            ViewBag.Categories = categories;

            return View(video);
        }

        public IActionResult Delete(Guid id)
        {
            var video = _context.Videos.Find(id); // Fetch video by id from the database
            return View(video);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var video = await _context.Videos.FindAsync(id); // Fetch video by id from the database

            if (video == null)
            {
                return NotFound(); // Return a 404 Not Found response if video is not found
            }

            _context.Videos.Remove(video); // Mark the video for deletion
            await _context.SaveChangesAsync(); // Save changes to the database

            TempData["Message"] = "Video deleted successfully!"; // Use TempData to show success message on redirect

            return RedirectToAction("Index"); // Redirect to the list of videos after successful deletion
        }

        [HttpPost]
        public async Task<IActionResult> Update(Video model)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            model.CreatedBy = userId;
            model.ChangedBy = userId;
            model.ChangedOn = DateTime.Now;
            try
            {
                {
                    var existingVideo = _context.Videos.Find(model.Id); // Fetch the existing video by ID

                    if (existingVideo != null)
                    {
                        // Update properties of the existing video with values from the model
                        existingVideo.Title = model.Title;
                        existingVideo.Description = model.Description;
                        existingVideo.CategoryId = model.CategoryId;
                        existingVideo.IsActive = model.IsActive == null ? false : model.IsActive;
                        // Save changes to the database
                        _context.Videos.Update(existingVideo);
                        await _context.SaveChangesAsync();

                        ViewBag.Message = "Video updated successfully!";
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
            var categories = _context.Categories.ToList(); // Fetch all categories

            ViewBag.Categories = categories; // Pass categories to the view bag
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> Upload(VideoUploadModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Define the allowed file extensions as an array
                    string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv" }; // Add more extensions as needed
                    long fileSizeLimit = 100 * 1024 * 1024; // 100 MB in bytes

                    // Define the allowed file extensions as an array
                    if (model.VideoFile != null && model.VideoFile.Length > 0)
                    {

                        // Check if a file was selected for upload and if the file extension is allowed
                        if (!IsFileExtensionAllowed(model.VideoFile, allowedExtensions))
                            return BadRequest("Invalid file type. Please upload a mp4, avi, or mov file.");
                        if (!IsFileSizeWithinLimit(model.VideoFile, fileSizeLimit))
                            return BadRequest("File size exceeds the maximum allowed size (1 MB).");
                        if (FileWithSameNameExists(model.VideoFile, _context))
                            return BadRequest("Duplicate file name detected. Please upload a file with a different name.");
                        // Save the uploaded video file to a specified directory
                        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
                        var filePath = Path.Combine(uploadsDir, model.VideoFile.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.VideoFile.CopyToAsync(fileStream);
                        }
                        // Here you can save additional information about the video to your database
                        // For example: var video = new Video { Title = model.Title, Description = model.Description, FilePath = filePath };
                        // Save the video object to the database using your DbContext
                        // Save additional information about the video to your database

                        // Call YouTube API to get video information
                        //var videoInfo = await GetYouTubeVideoInfo(model.YouTubeLink);
                        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var video = new Video();
                        if (userId != null)
                        {
                            var thumbnailPath = GetVideoThumbnail(filePath, "path_to_save_thumbnail.jpg");
                            video = new Video
                            {
                                Title = model.Title,
                                Url = filePath,
                                Description = model.Description,
                                CategoryId = model.CategoryId,
                                IsActive = model.IsActive == null ? false : model.IsActive,
                                CreatedBy = userId,
                            };
                        }
                        else
                        {

                            video = new Video
                            {
                                Title = model.Title,
                                Url = filePath,
                                Description = model.Description,
                                CategoryId = model.CategoryId,


                                CreatedBy = "Vistor",
                            };
                        }

                        // Add the video object to the context and save changes
                        await _context.AddAsync(video);
                        await _context.SaveChangesAsync();
                        ViewBag.Message = "Video uploaded successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("VideoFile", "Please select a file to upload.");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return View(model);
        }



        public string GetVideoThumbnail(string videoFilePath, string outputThumbnailPath)
        {
            var ffmpegPath = @"C:\ffmpeg\bin\ffmpeg.exe"; // Update with the correct path to FFmpeg

            // Ensure the output folder exists
            Directory.CreateDirectory(Path.GetDirectoryName(outputThumbnailPath));

            var processStartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoFilePath}\" -ss 00:00:01.000 -vframes 1 \"{outputThumbnailPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                try
                {
                    process.Start();
                    process.WaitForExit();

                    // Check if FFmpeg process exited successfully
                    if (process.ExitCode == 0 && System.IO.File.Exists(outputThumbnailPath))
                    {
                        return outputThumbnailPath;
                    }
                    else
                    {
                        // Thumbnail extraction failed or FFmpeg encountered an error
                        var errorMessage = process.StandardError.ReadToEnd(); // Read FFmpeg's error output
                        throw new Exception($"FFmpeg error: {errorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions during FFmpeg execution
                    throw new Exception($"Error executing FFmpeg: {ex.Message}");
                }
            }
        }




        private async Task<YouTubeVideoInfo> GetYouTubeVideoInfo(string youTubeLink)
        {
            // Implement logic to call YouTube API and fetch video information based on the provided link
            // You may need to use HttpClient or a library like Google.Apis.YouTube.v3
            // Example: https://developers.google.com/youtube/v3/docs/videos/list

            // Placeholder code
            var videoInfo = new YouTubeVideoInfo
            {
                Title = "Sample Video Title",
                Description = "Sample Video Description",
                ThumbnailUrl = "https://example.com/thumbnail.jpg"
            };

            return videoInfo;
        }
        private bool FileWithSameNameExists(IFormFile videoFile, ApplicationDbContext context)
        {
            // Check if a file with the same name exists in the Library table
            return context.Videos.Any(v => v.Url == videoFile.ToString());
        }

        private bool IsFileSizeWithinLimit(IFormFile videoFile, long fileSizeLimit)
        {
            return videoFile.Length <= fileSizeLimit;
        }

        private bool IsFileExtensionAllowed(IFormFile videoFile, string[] allowedExtensions)
        {
            var fileExtension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }




    }
}
