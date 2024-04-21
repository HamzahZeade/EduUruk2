//using MediaAcademy.Models.Controls;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;

namespace EduUruk.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,BusAdmin")]
    [Area("Admin")]
    public class ManageUsersController : _BaseController
    {
        //private readonly SignInManager<User> _signInManager;
        //private readonly UserManager<User> _userManager;
        //private readonly IEmailSender _emailSender;
        //private readonly RoleManager<Role> _roleManager;
        private IConfiguration _config;
        IServiceProvider _ServiceProvider { get; }
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        UserRepo userRepository;
        RoleRepo roleRepository;
        public ManageUsersController(UserRepo userRepository, RoleRepo roleRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration config
            , EmailService emailService, SMSService SMSService, IServiceProvider serviceProvider
            )
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            _hostingEnvironment = hostingEnvironment;
            _emailService = emailService;
            _SMSService = SMSService;
            this._config = config;
            _ServiceProvider = serviceProvider;

        }
        //public UsersController(DAL.EnitiyDAL.ApplicationDbContext context, UserManager<User> userManager,
        //    SignInManager<User> signInManager,
        //    IEmailSender emailSender,
        //    RoleManager<Role> roleManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _emailSender = emailSender;
        //    _roleManager = roleManager;
        //    db = context;
        //}

        // GET: Users
        public IActionResult Index()
        {
            //    AD.GetAllUsers();
            //    var img = AD.GetUserPicture("aabbas");
            // img.Save("");
            //                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "_Storage", "Users");
            //    var filePath = Path.Combine(uploads, "img.jpg");
            //    img.Save(filePath);


            return View();
        }
        public IActionResult Index_Activeted()
        {
            //    AD.GetAllUsers();
            //    var img = AD.GetUserPicture("aabbas");
            // img.Save("");
            //                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "_Storage", "Users");
            //    var filePath = Path.Combine(uploads, "img.jpg");
            //    img.Save(filePath);


            return View();
        }
        public UserForm GetCreateUserForm()
        {
            UserForm form = new UserForm();
            // using (RoleRepo roleRepository = new RoleRepo(db))
            {
                form.Roles.AddRange(roleRepository.GetForInput(false));
            }

            form.Emps.Add(new SelectControl()
            {
                Text = Models.Resources.SysModelRes.ManagerPlaceholder,
                Value = string.Empty
            });
            form.Orgs.Add(new SelectControl()
            {
                Text = Models.Resources.SysModelRes.ManagerPlaceholder,
                Value = string.Empty
            });
            return form;
        }


        // GET: /Users/GetCustoms/
        [HttpPost]
        public IActionResult GetCustoms(string userStatus)
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
            //    using (UserRep userRepository = new UserRep())
            {
                roles = userRepository.GetCustom_journal(filter, userStatus, out TotalRecords, out RecordsFiltered);
            }
            //var cultureInfo = new CultureInfo("en-US");
            //cultureInfo.DateTimeFormat.DateSeparator = "/";
            //cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //cultureInfo.DateTimeFormat.LongDatePattern = "dd/MM/yyyy";
            //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            //CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            var data = from a in roles
                       select new
                       {
                           id = a.Id,
                           name = a.UserFullName,
                           roles = a.Roles,
                           dt = "",// a.ContractExpirationDate.Value.ToShortDateString(),
                           // user_id = a.UserId,
                           email = a.Email,
                           mobile = a.PhoneNumber,
                           photo = a.Photo,
                           username = a.UserName,
                           use_local_pass = a.use_local_pass ? "دخول محلي" : "دخول عن طريق الشبكة",
                           req_change_pass = a.RequierdChangePass ? "مطلوب" : "غير مطلوب",
                           has_send_sms = a.SendDT == null ? "" : a.SendDT.Value.ToLongDateString(),
                           created_on = a.CreatedTime == null ? "" : a.CreatedTime.Value.ToLongDateString(),
                           is_active = a.IsActivated ? "مفعل" : "غير مفعل",
                           active_time = a.ActivatedTime,
                           job_title = a.JobTitle,

                       };

            return Json(new { draw = draw, recordsFiltered = RecordsFiltered, recordsTotal = TotalRecords, data = data });
        }



        // GET: /Users/Create
        public IActionResult Create()
        {
            //UserForm form = new UserForm();
            //// //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager))
            //{
            //    form = userRepository.GetCreateUserForm();
            //}
            //  form.ContractExpirationDate = DateTime.Now.AddMonths(10);
            return PartialView();
            //return PartialView(form);
        }

        // POST: /Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserForm form)
        {
            if (ajax.IsAjaxRequest(HttpContext.Request))

            {
                ResponseResult result = null;
                try
                {
                    form.Createdby = getUserId();
                    form.UpdatedBy = getUserId();
                    try
                    {
                        //var ad_u = AD.GetAUser(form.UserName);
                        //if(ad_u != null)
                        //{
                        //    var img = AD.GetUserPicture(form.UserName);
                        //}
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    // //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager, ModelState.IsValid))
                    {
                        result = await userRepository.Add(form);
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        status = "error",
                        btnClass = "btn btn-danger",
                        title = "error",
                        message = "error",
                        close = false
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
                //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager))
                {
                    form = userRepository.GetEditUserForm(id);
                }
            }

            return PartialView(form);
        }

        // POST: /Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager, ModelState.IsValid))
                    {
                        result = userRepository.Edit2(form).Result;
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        status = "error",
                        btnClass = "btn btn-danger",
                        title = "error",
                        message = "error",
                        close = false
                    };
                }
                return Json(result);
            }
            else return null;// else return null ;//
        }
        [HttpPost]
        public async Task<IActionResult> SendSMS(string id)
        {
            if (ajax.IsAjaxRequest(HttpContext.Request))

            {
                ResponseResult result = null;
                try
                {
                    var user = userRepository.GetByID(id);
                    var mob = user.PhoneNumber;
                    var msg_ar = "";
                    var msg_eng = "";
                    string email = user.Email;
                    string uel = _config.GetValue<string>("AppSettings:baseUrl", "https://masterlist.media.gov.sa");// _config.GetSection("SMS")["token"];
                    msg_ar = "يمكن الدخول الى نظام " + "(المركز الإعلامي الإفتراضي)"
                              + System.Environment.NewLine
                               + "عن طريق الرابط :"
                               + System.Environment.NewLine
                               + uel//"https://masterlist.media.gov.sa"
                                + System.Environment.NewLine
                               + "اسم المستخدم:"
                              + System.Environment.NewLine
                               + user.UserName;
                    ;
                    msg_eng = user.UserName
                                + System.Environment.NewLine
                                + ":username"
                                + System.Environment.NewLine
                                + uel//"https://masterlist.media.gov.sa"
                                + System.Environment.NewLine
                                + ":via"
                                + System.Environment.NewLine
                                + "you can login to " + "(VPO)";

                    if (mob != null)
                    {
                        msg_ar += System.Environment.NewLine
                        //   + "كلمة المرور :"
                        //+ System.Environment.NewLine
                        + "{0}"
                        + System.Environment.NewLine
                        + "وزارة الإعلام";
                        if (user.use_local_pass)
                        {
                            if (user.RequierdChangePass)
                            {
                                msg_ar = String.Format(msg_ar,
                                      "كلمة المرور المؤقتة :"
                              + System.Environment.NewLine +
                                    user.TempPassword
                              + System.Environment.NewLine +
                              "يجب تغيير كلمة المرور عند تسجيل الدخول"
                                    );
                            }
                            else
                            {
                                msg_ar = String.Format(msg_ar, "كلمة المرور الخاصة بالنظام");

                            }
                        }
                        else
                        {
                            msg_ar = String.Format(msg_ar, "كلمة المرور الخاصة بالبريد الإلكتروني");
                        }
                        result = _SMSService.SendSMS(msg_ar, mob);
                        if (result.status == "success")
                        {
                            result = userRepository.EditSendSMS(user.Id).Result;
                        }
                    }
                    else
                    {
                        result = new ResponseResult()
                        {
                            status = "error",
                            btnClass = "btn btn-danger",
                            title = "error",
                            message = "لايوجد رقم جوال للمستخدم",
                            close = false
                        };
                    }

                    if (email != null)
                    {
                        string emailHtml = "";
                        using (var scope = _ServiceProvider.CreateScope())
                        {

                            IRazorViewEngine _razorViewEngine = scope.ServiceProvider.GetService<IRazorViewEngine>();
                            ITempDataProvider _tempDataProvider = scope.ServiceProvider.GetService<ITempDataProvider>();
                            IServiceProvider _serviceProvider = scope.ServiceProvider.GetService<IServiceProvider>();


                            EmailVM emailVM = new EmailVM();
                            emailVM.EmailStats = new List<EmailStat>();

                            emailVM.EmailStats.Add(new EmailStat() { tot = "بيانات الدخول", desc = msg_ar });
                            emailVM.EmailStats.Add(new EmailStat() { tot = "Login Data", desc = msg_eng });

                            // emailVM.HeadOne = msg_ar;
                            //  emailVM.HeadDesc = "إعلان جديد";


                            string headerImage = "";
                            //  string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "logos", "logo-demo2.png");
                            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "media", "logos", "mailHeader.jpg");
                            System.IO.File.ReadAllBytes(imagePath);
                            // using (Image image = Image.FromFile(imagePath))
                            {
                                using (MemoryStream m = new MemoryStream())
                                {
                                    //  image.Save(m, image.RawFormat);
                                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath); ;// m.ToArray();

                                    // Convert byte[] to Base64 String
                                    headerImage = Convert.ToBase64String(imageBytes);

                                }
                            }


                            //  headerImage = String.Format("data:image/jpg;base64,{0}", headerImage);
                            headerImage = "https://momakssa.blob.core.windows.net/ad-dev/mailheader.png";// String.Format("data:image/jpg;base64,{0}", headerImage);

                            emailVM.headerImage = headerImage;

                            emailHtml = new ViewRenderService(_razorViewEngine, _tempDataProvider, _serviceProvider).RenderToStringAsync("_NotifyEmail", emailVM).Result;
                        }
                        _emailService.SendEmailToJournal("بيانات الدخول", emailHtml, email);


                    }
                    else
                    {
                        result = new ResponseResult()
                        {
                            status = "error",
                            btnClass = "btn btn-danger",
                            title = "error",
                            message = "لايوجد بريد الكتروني للمستخدم",
                            close = false
                        };
                    }


                    //result = userRepository.AddMessageLog(messageLog);
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        status = "error",
                        btnClass = "btn btn-danger",
                        title = "error",
                        message = "error",
                        close = false
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
            //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager))
            {
                role = userRepository.GetByID(id);
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
                    //using (UserRepo userRepository = new UserRepo(db, _userManager, _roleManager, ModelState.IsValid))
                    {
                        result = userRepository.Delete(id);
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        status = "error",
                        btnClass = "btn btn-danger",
                        title = "error",
                        message = "error",
                        close = false
                    };
                }
                return Json(result);
            }
            else return null;//
        }



        [HttpPost]
        // public IActionResult UploadImage(IList<IFormFile> oFiles)
        public async Task<IActionResult> UploadImage(IList<IFormFile> files)
        {
            ResponseResult result = new ResponseResult();
            result = new ResponseResult()
            {
                close = true,
                status = "success",
                title = "Uploaded success",
                message = "Image uploaded successfully.",
                url = "",
                data = "file_new_name.png"
            };

            //}

            return Json(result);
        }


    }

}
