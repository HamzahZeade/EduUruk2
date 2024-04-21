using System.Drawing.Imaging;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Localization;
using MediaAcademy.DAL.Repositories;
using MediaAcademy.Models.Entities.B_Tables;
using MediaAcademy.Models.Resources;
using MediaAcademy.Models.ViewModels;
using MediaAcademy.Web.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MediaAcademy.Web.Controllers
{
    public class NewsController : _BaseController
    {
        NewsRepo _newsRepo;
        NewsCategoryRepo _newsCategoryRepo;
        FileStorageService _fileStorageService;

        public NewsController(NewsRepo newsRepo, NewsCategoryRepo newsCategoryRepo, FileStorageService FileStorageService)
        {
            _newsRepo = newsRepo;
            _newsCategoryRepo = newsCategoryRepo;
            _fileStorageService = FileStorageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCustoms(int? folder_id)
        {
            var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            var draw = dict["draw"];
            var page_length = (dict["length"]);
            SearchFilters filter = new SearchFilters()
            {
                PageLength = -1,
                SearchString = !string.IsNullOrEmpty(dict["search[value]"]) ? dict["search[value]"] : string.Empty,
            };

            int TotalRecords = 0, RecordsFiltered = 0;
            int i = 1;
            var data = (from a in (_newsRepo.getNewsAll().ToList())
                            //let Agencies = a.NewsAgancyCategories.Select(c => c.AgancyCategory.Category_Name) ?? new List<string>() { "" }
                        select new
                        {
                            //Subject , Subject_eng
                            Record = i++,
                            id = a.Id.ToString(),
                            a.Topic,
                            newsCategoryTitle = a.NewsCategory?.Title,
                            IsPublished = (a.IsPublished == true ? BusRes.News_Published : BusRes.News_Draft),
                            IsArabicLang = (a.IsArabicLang == true ? BusRes.News_Template_Ar : BusRes.News_Template_En),
                            CreatedOn = string.Format("{0:yy-MM-dd hh:mm tt}", a.CreatedOn)
                        }
                      ).ToList();
            return Json(new { draw = draw, recordsFiltered = RecordsFiltered, recordsTotal = TotalRecords, data = data });
        }

        public IActionResult Create()
        {
            ViewBag.NewsCategories = _newsCategoryRepo.GetAll().ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(News news)
        {
            news.CreatedOn = DateTime.Now;
            news.CreatedBy = getUserId();
            news.EventId = new Guid(getEventId());
            if (news.IsPublished == true)
            {
                news.PublishDate = DateTime.Now;
                news.PublishedBy = getUserId();
            }
            var res = _newsRepo.addNews(news);
            return Json(res);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(News news, List<IFormFile> attachedFiles)
        //{
        //    news.CreatedBy = getUserId(); ;
        //    news.ChangedBy = "";
        //    news.ChangedOn = DateTime.Now;
        //    news.EventId = new Guid("aec7c0cc-cdfc-4189-1d9d-08da4a20bd8a");

        //    var res = _newsRepo.addNews(news);

        //    if (attachedFiles != null)
        //    {
        //        #region Uload :
        //        foreach (IFormFile file in attachedFiles)
        //        {
        //            var uniqueFileName = GetUniqueFileName(file.FileName);
        //            var uploads = Path.Combine("uploads");
        //            var filePath = Path.Combine(uploads, uniqueFileName);
        //            if (file != null)
        //            {
        //                if (file.Length > 0)
        //                {
        //                    NewsAttachment newsAttachment = new NewsAttachment();
        //                    using (var stream = new MemoryStream())
        //                    {
        //                        file.CopyTo(stream);
        //                        var bytes = stream.ToArray();
        //                        var dd = _fileStorageService.Createfile(stream, uniqueFileName, uploads).Result;
        //                        if (dd.status != "success")
        //                        {
        //                            //return Json(dd);
        //                        }
        //                    }
        //                    newsAttachment.AttachNote = "";
        //                    newsAttachment.NewsId = news.Id;
        //                    newsAttachment.AttachmentFilePath = uniqueFileName;
        //                    _newsRepo.AddAttach(newsAttachment);
        //                }
        //            }
        //        }
        //    }
        //    #endregion
        //    return Json(res);
        //}

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public IActionResult Edit(Guid id)
        {
            var res = _newsRepo.getNewsById(id);
            ViewBag.NewsCategories = _newsCategoryRepo.GetAll().ToList();
            return View(res);
        }

        // POST: /Users/Edit/5
        [HttpPost]
        public ActionResult Edit(News news)
        {
            news.CreatedBy = "";
            news.EventId = new Guid(getEventId());
            news.ChangedBy = getUserId();
            news.ChangedOn = DateTime.Now;
            if (news.IsPublished == true)
            {
                news.PublishDate = DateTime.Now;
                news.PublishedBy = getUserId();
            }
            var res = _newsRepo.EditNews(news);
            return Json(res);
        }

        public IActionResult Delete(Guid id)
        {
            return PartialView(new News() { Id = id });
        }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (ajax.IsAjaxRequest(HttpContext.Request))
            {
                ResponseResult result = null;
                try
                {
                    //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager, ModelState.IsValid))
                    {
                        result = _newsRepo.deleteNews(id, getUserId());
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        status = "error",
                        btnClass = "btn btn-danger",
                        title = Models.Resources.GeneralRes.ErrorTitle,
                        message = Models.Resources.GeneralRes.ErrorWhileSaving,
                        close = false
                    };
                }
                return Json(result);
            }
            else return null;//
        }

        public IActionResult DeleteAttach(int id)
        {
            if (ajax.IsAjaxRequest(HttpContext.Request))
            {
                ResponseResult result = null;
                try
                {
                    //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager, ModelState.IsValid))
                    {
                        result = _newsRepo.deleteNewsAttach(id, getUserId());
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        status = "error",
                        btnClass = "btn btn-danger",
                        title = Models.Resources.GeneralRes.ErrorTitle,
                        message = Models.Resources.GeneralRes.ErrorWhileSaving,
                        close = false
                    };
                }
                return Json(result);
            }
            else return null;//
        }

        [HttpPost]
        public IActionResult GetNewsAttach(Guid id)
        {

            var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            var draw = dict["draw"];
            SearchFilters filter = new SearchFilters()
            {
                PageLength = !string.IsNullOrEmpty(dict["length"]) ? Convert.ToInt32(dict["length"]) : 0,
                StartIndex = !string.IsNullOrEmpty(dict["start"]) ? Convert.ToInt32(dict["start"]) : 0,
                SearchString = !string.IsNullOrEmpty(dict["search[value]"]) ? dict["search[value]"] : string.Empty,
                SortColumn = !string.IsNullOrEmpty(dict["columns[" + dict["order[0][column]"] + "][name]"]) ?
                            dict["columns[" + dict["order[0][column]"] + "][name]"] : string.Empty,
                SortDirection = !string.IsNullOrEmpty(dict["order[0][dir]"]) ? dict["order[0][dir]"] : string.Empty

            };

            int TotalRecords = 0, RecordsFiltered = 0;
            List<NewsAttachment> roles = new List<NewsAttachment>();


            roles = _newsRepo.GetTaskDepartmentAttach(filter, id, out TotalRecords, out RecordsFiltered);


            int i = 1;
            var data = from a in roles

                       select new
                       {
                           Record = i++,
                           Id = a.Id,
                           AttachNote = a.AttachNote,
                           AttachmentFilePath = a.AttachmentFilePath,
                           TaskDepartmentId = a.NewsId
                       };

            return Json(new
            {
                draw = draw,
                recordsFiltered = RecordsFiltered,
                recordsTotal = TotalRecords,
                data = data
            });
        }

        public async Task<IActionResult> DownloadFile(string file_name, string type)
        {
            var dd = _fileStorageService.DowloadFile(file_name, null, null);
            return dd;
        }

        [HttpPost]
        public IActionResult UploadTaskDepartmentAttatch(IFormFile AttatchedFile, Guid NewsId, string AttachNote)
        {
            int validationResult = UploadUserFileValidation(AttatchedFile);
            if (validationResult < 0)
            {
                return Content(validationResult.ToString());
            }

            NewsAttachment newsAttachment = new NewsAttachment();

            newsAttachment.NewsId = NewsId;
            newsAttachment.AttachNote = AttachNote ?? "";

            if (AttatchedFile != null)
            {
                #region Uload :
                var uniqueFileName = GetUniqueFileName(AttatchedFile.FileName);
                var uploads = Path.Combine("uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                // foreach (IFormFile file in files)
                {
                    if (AttatchedFile != null)
                        if (AttatchedFile.Length > 0)
                        {
                            //string filePath = Path.Combine(uploads, filename);
                            //using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                            //{
                            //    await file.CopyToAsync(fileStream);
                            //}
                            using (var stream = new MemoryStream())
                            {
                                AttatchedFile.CopyTo(stream);
                                var bytes = stream.ToArray();
                                var dd = _fileStorageService.Createfile(stream, uniqueFileName, uploads).Result;
                                if (dd.status != "success")
                                {
                                    return Json(dd);
                                }
                            }
                        }
                }
                #endregion

                newsAttachment.AttachmentFilePath = uniqueFileName;
                _newsRepo.AddAttach(newsAttachment);
            }

            return Content("Success");
        }

        public int UploadUserFileValidation(IFormFile file)
        {
            int ErrorMessage = 0;
            try
            {
                var supportedTypes = new[] { "xlsx", "xls", "bmp", "jpg", "jpeg", "png", "gif", "doc", "docx", "ppt", "pptx", "txt", "pdf" };
                var fileExt = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    ErrorMessage = -1;// "File Extension Is InValid";
                    return ErrorMessage;
                }
                else if (file.Length * 1e-6 > 4096)
                {
                    ErrorMessage = -2; //"File size Should Be UpTo " + filesize + "KB";
                    return ErrorMessage;
                }
                else
                {
                    ErrorMessage = 1; //"File Is Successfully Uploaded";
                    return ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = -3; //"Upload Container Should Not Be Empty or Contact Admin";
                return ErrorMessage;
            }
        }

    }
}
