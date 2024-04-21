using MediaAcademy.DAL.Repositories;
using MediaAcademy.Models.Entities.B_Tables;
//using Newtonsoft.Json;
using MediaAcademy.Models.Resources;
using MediaAcademy.Models.ViewModels;
using MediaAcademy.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Localization;
using System.Text;

namespace MediaAcademy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<GeneralRes> _sharedLocalizer;
        FileStorageService _fileStorageService;
        NewsRepo _newsRepo;
        ContactUsRepo _contactUsRepo;

        public HomeController(NewsRepo newsRepo, ContactUsRepo contactUsRepo, FileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
            _newsRepo = newsRepo;
            _contactUsRepo = contactUsRepo;

        }

        public IActionResult Index()
        {
            ViewBag.NewsCategory = _newsRepo.getNewsCategory() ?? new List<NewsCategory>();
            ViewBag.NewsLast = _newsRepo.getNewsLast(12, (GeneralRes.Ratelanguage == "ar" ? true : false)) ?? new List<News>();
            return View();
        }

        public IActionResult NewsDetails()
        {
            // Logic to fetch news details and pass them to the view
            return View();
        }
        public IActionResult NewsCat(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<News> news = _newsRepo.getNewsPublishedByCategory(new Guid(id), (GeneralRes.Ratelanguage == "ar" ? true : false));

            ViewBag.NewsCategory = _newsRepo.getNewsCategory();
            return View(news);
        }

        public IActionResult NewsAll()
        {
            HttpContext.Session.SetString("SkipNo", "-1");

            return View();
        }


        public IActionResult NewsPartial(int skip, int take)
        {
            string SkipNo = HttpContext.Session.GetString("SkipNo");

            if (SkipNo != skip.ToString())
            {
                List<News> news = _newsRepo.getNewsAllPublishedWOThumbnail((GeneralRes.Ratelanguage == "ar" ? true : false), skip, take);
                HttpContext.Session.SetString("SkipNo", skip.ToString());
                return PartialView("_NewsPartial", news);
            }
            else
            {
                return Content("");
            }

        }



        public IActionResult News(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            News news = _newsRepo.getNewsById(new Guid(id));



            ViewBag.NewsCategory = _newsRepo.getNewsCategory();

            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var iPAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var httpContext = HttpContext.ToString();

            _newsRepo.NewsLogView(new Guid(id), "V", userAgent, iPAddress, httpContext);

            return View(news);
        }
        [HttpPost]
        public IActionResult HandleFormSubmission(WaitingList model)
        {
            // Process the form data (save to database, etc.)
            // Return appropriate response

            var result = _contactUsRepo.Submission(model);
            return Json(result);
            //return Ok(new { message = "Form data received successfully." });
        }
        public IActionResult ContactUs()
        {

            return View();
        }


        public IActionResult CreateForm()
        {
            //var userId = getUserId();
            //ViewBag.TempSupportId = Guid.NewGuid().ToString();
            ContactUs contactUs = new ContactUs();

            return View(contactUs);
            //if (string.IsNullOrEmpty(userId))
            //{
            //    res = _SupportSystemRepo.GetAllByUserEmail(getUserEmail()).ToList();


            //    return View();
            //}
            //else
            //{
            //    res = _SupportSystemRepo.GetAllByUserEmailOrRequester(getUserEmail(), userId).ToList();

            //    return RedirectToAction(nameof(Index));
            //}

        }
        [HttpPost]
        public JsonResult contactusAdd(ContactUs contactUs)
        {
            var result = _contactUsRepo.add(contactUs);
            return Json(result);
        }

        public ActionResult Index2()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            else if (User.IsInRole("Admin") || User.IsInRole("BusAdmin"))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                if (User.IsInRole("journal"))
                    return RedirectToAction("Index", "journalistic");
                else if (User.IsInRole("org"))
                    return RedirectToAction("Index", "org");
            }

            return View();
        }

        public ActionResult DownloadFile(string id)
        {
            if (id != null)
            {
                var dir = Path.Combine("Uploads");

                byte[] fileBytes;
                {
                    var dd = _fileStorageService.getImage(id, dir);

                    var provider = new FileExtensionContentTypeProvider();
                    string contentType;
                    if (!provider.TryGetContentType(id, out contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    var mimeType = contentType;

                    return File(dd, mimeType);
                }
            }
            return null;
        }

        public async Task<IActionResult> DownloadFile2(string id)
        {
            if (id != null)
            {
                var dd = _fileStorageService.DowloadFile(id, null, null);
                return dd;
            }

            return null;
        }


        //public IActionResult conf01()
        //{
        //    ViewBag.userFullName = "Visitor";
        //    ViewBag.userEmail = "info@media.gov.sa";
        //    var webinars = _webinarRepo.GetAll();

        //    return View(webinars);
        //}

        public ActionResult ZoomWindow()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult share(string id, string app)
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var iPAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var httpContext = HttpContext.Request.ToString();

            var result = _newsRepo.NewsLogShare(new Guid(id), "S", userAgent, iPAddress, httpContext, app);
            return Json(result);
        }


        [HttpPost]
        public string GetNewsThumbnail(string id)
        {
            //var result = _newsRepo.getNewsThumbnail(new Guid(id));
            //var img = ReduceImageSize(Encoding.ASCII.GetBytes(result.Replace("data:image/png;base64,", "")), 50);
            ////uploadedFile.Replace("data:image/png;base64,", "")
            //return "data:image/png;base64," + Encoding.ASCII.GetString(img);

            var result = _newsRepo.getNewsThumbnail(new Guid(id));
            return result;
        }

        //public byte[] ReduceImageSize(byte[] imageBytes, int jpegQuality)
        //{
        //    using var inputStream = new MemoryStream(imageBytes);
        //    var image = System.Drawing.Image.FromStream(inputStream);
        //    var jpegEncoder = ImageCodecInfo.GetImageDecoders()
        //        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
        //    var encoderParameters = new EncoderParameters(1);
        //    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (int)jpegQuality);

        //    using var outputStream = new MemoryStream();
        //    image.Save(outputStream, jpegEncoder, encoderParameters);
        //    return outputStream.ToArray();
        //}

        //public IActionResult MediaView(string p)
        //{
        //    //ViewBag.mediaLibraries = _mediaLibraryRepo.getMLFoldersViewHome(p);
        //    ViewBag.mediaLibraryFiles = _mediaLibraryRepo.getMLFilesViewHome();
        //    return View();
        //}

        //[HttpGet]
        //public ContentResult getFile(string id)
        //{
        //    var _file = _mediaLibraryRepo.getFile(id);
        //    var _data = JsonSerializer.Serialize(_file);

        //    return Content(_data, "application/json");
        //}

        //[HttpPost]
        //public bool logFile(Guid guid)
        //{
        //    MediaLibraryLog mediaLibraryLog = new MediaLibraryLog()
        //    {
        //        FileId = guid,
        //        LogType = "DH",
        //        UserAgent = HttpContext.Request.Headers["User-Agent"],
        //        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
        //        CreateDate = DateTime.Now,
        //        CreatedBy = "VISITOR"
        //    };
        //    return _mediaLibraryRepo.logFile(mediaLibraryLog);
        //}
    }
}
