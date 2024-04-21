//using MediaAcademy.Models.Controls;
using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduUruk.Web.Areas.Admin.Controllers
{
    //  [Authorize(Roles="Admin")]

    [Area("Admin")]
    public class UsersController : _BaseAdminController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        UserRepo _userRepository;
        RoleRepo _roleRepository;

        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context, UserRepo userRepository, RoleRepo roleRepository, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _roleManager = roleManager;

            _context = context;
        }

        // GET: Users
        public IActionResult Index()
        {
            List<User> users = _userRepository.GetAll(); ;

            using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
            {
                foreach (var usr in users)
                {
                    var roleNames = (_userManager.GetRolesAsync(userRepository.GetByID(usr.Id)).Result).ToArray();
                    usr.Roles = _roleManager.Roles.Where(r => roleNames.AsEnumerable().Contains(r.Name)).Select(r => r.ArabicName).ToList();
                }
            }

            return View(users);
        }

        public UserForm GetCreateUserForm()
        {
            UserForm form = new UserForm();
            // using (RoleRepo roleRepository = new RoleRepo(_context))
            {
                form.Roles.AddRange(_roleRepository.GetForInput(false));
            }

            return form;
        }

        // GET: /Users/GetCustoms/
        [Produces("application/json")]
        [HttpPost]
        public IActionResult GetCustoms()
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
            List<User> roles = new List<User>();
            roles = _userRepository.GetCustom(filter, out TotalRecords, out RecordsFiltered);
            var data = from a in roles
                       select new
                       {
                           id = a.Id,
                           name = a.UserName,
                           roles = a.Roles,
                           email = a.Email,
                           mobile = a.PhoneNumber,
                           username = a.UserName,
                           adddate = a.CreatedTime?.ToString("dd/MM/yyyy"),

                       };
            return Json(new { draw = draw, recordsFiltered = RecordsFiltered, recordsTotal = TotalRecords, data = data });
        }

        // GET: /Users/Create
        public IActionResult Create()
        {
            UserForm form = new UserForm();
            // //using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
            {
                form = _userRepository.GetCreateUserForm();
            }
            //  form.ContractExpirationDate = DateTime.Now.AddMonths(10);
            return PartialView(form);
        }

        // POST: /Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserForm form)
        {
            if (ajax.IsAjaxRequest(HttpContext.Request))
            {
                ResponseResult result = null;
                try
                {
                    // //using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager, ModelState.IsValid))
                    {
                        form.Createdby = User.Identity.Name;
                        form.UpdatedBy = User.Identity.Name;
                        result = await _userRepository.Add(form);
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                        Close = false
                    };
                }
                return Json(result);
            }
            else return null;//
        }

        // GET: /Users/Edit/5
        public IActionResult Edit(string id)
        {
            UserForm form = new UserForm();
            if (string.IsNullOrEmpty(id))
            {
                return StatusCode(418);
            }
            else
            {
                //using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
                {
                    form = _userRepository.GetEditUserForm(id);
                }
            }

            return PartialView(form);
        }
        [HttpGet]
        public JsonResult GetById(string Id)
        {
            var data = _userRepository.GetById(Id);
            return Json(data);
        }


        [HttpPost]
        public JsonResult UpdateStatus(string id, bool status)
        {
            var result = _userRepository.UpdateStatus(id, status);
            return Json(new { status = "success" });
        }
        // POST: /Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserForm form)
        {
            if (ajax.IsAjaxRequest(HttpContext.Request))

            {
                ResponseResult result = null;
                try
                {
                    form.UpdatedBy = "0";
                    //     ModelState["Password"].Errors.Clear();
                    //   ModelState["ConfirmPassword"].Errors.Clear();
                    //using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager, ModelState.IsValid))
                    {
                        result = _userRepository.Edit(form).Result;
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                        Close = false
                    };
                }
                return Json(result);
            }
            else return null;// else return null ;//
        }

        // GET: /Users/Delete/5
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            User role = null;
            //using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
            {
                role = _userRepository.GetByID(id);
            }
            if (role == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return PartialView(role);
        }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if (ajax.IsAjaxRequest(HttpContext.Request))
            {
                ResponseResult result = null;
                try
                {
                    //using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager, ModelState.IsValid))
                    {
                        result = _userRepository.Delete(id);
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                        Close = false
                    };
                }
                return Json(result);
            }
            else return null;//
        }

        [HttpPost]
        public JsonResult Remove(string id)
        {
            var result = _userRepository.Remove(id);
            return Json(result.Status);
        }

        //[Produces("application/json")]
        [HttpPost]
        public JsonResult Add(UserForm form)
        {
            var x = 0;
            //form.CreatedBy = User.Identity.Name;
            return Json(_userRepository.Add(form));
        }





        [HttpGet]
        public JsonResult GetRoles(string q)
        {
            var titles = _userRepository.GetByRoles(q);

            var data = titles.Select(x => new
            {
                id = x.Id,
                text = x.ArabicName
            });

            return Json(data);
        }

        [HttpPost]
        public IActionResult UploadImage(IList<IFormFile> oFiles)
        {
            ResponseResult result = new ResponseResult();

            //try
            //{

            //    int xPosition = 0, yPosition = 0;
            //    int height = 150, width = 150;
            //    int newHeight = height;
            //    //Get posted file 
            //  //  Icol<IFormFile> oFiles = Request.Form.Files;
            //    var oFile = oFiles[0];
            //    if (oFile.ContentType.ToLower().Contains("image"))
            //    {
            //        //Get file Name info
            //        FileInfo file = new FileInfo(oFile.FileName);
            //        //get folder path ===========================================
            //        string folderName = Server.MapPath("~/Images/Users");
            //        string bgPath = Server.MapPath("~/Images/tempbg.png");
            //        string fileName = Guid.NewGuid().ToString().Substring(0, 10) + ".png";
            //        string fullPath = Path.Combine(folderName, fileName);
            //        System.Drawing.Image mainImage = System.Drawing.Image.FromStream(oFile.InputStream);
            //        System.Drawing.Image bg = System.Drawing.Image.FromFile(bgPath);
            //        int newWidth = (newHeight * mainImage.Width) / mainImage.Height;
            //        if (newWidth > width)
            //        {
            //            newWidth = width;
            //            newHeight = (newWidth * mainImage.Height) / mainImage.Width;
            //        }
            //        System.Drawing.Image imgThumb = mainImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            //        System.Drawing.Image bgThumb = bg.GetThumbnailImage(width, height, null, IntPtr.Zero);

            //        if (newWidth < width)
            //        {
            //            xPosition = (width - newWidth) / 2;
            //        }
            //        if (newHeight < height)
            //        {
            //            yPosition = (height - newHeight) / 2;
            //        }

            //        ImageRepository bgimage = new ImageRepository();
            //        bgimage.CurrentBitmap = new Bitmap(bgThumb);
            //        bgimage.InsertImage(new Bitmap(imgThumb), xPosition, yPosition);

            //        if (System.IO.File.Exists(fullPath))
            //            System.IO.File.Delete(fullPath);
            //        bgimage.CurrentBitmap.Save(fullPath);
            //        mainImage.Dispose();
            //        bg.Dispose();

            //        imgThumb.Dispose();
            //        bgThumb.Dispose();
            //        bgimage.CurrentBitmap.Dispose();

            //        result = new ResponseResult()
            //        {
            //            Close = true,
            //            Status = "success",
            //            Title = "Uploaded success",
            //            Message = "Image uploaded successfully.",
            //            URL = fileName
            //        };
            //    }
            //    else
            //    {
            //        result = new ResponseResult()
            //        {
            //            Close = false,
            //            Status = "error",
            //            Title = "File type error",
            //            Message = "Invalid image file."
            //        };
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Response.Clear();
            //   // HttpContext.Response.Write(0);
            //}

            return Json(result);
        }


    }

    public static class ajax
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}
