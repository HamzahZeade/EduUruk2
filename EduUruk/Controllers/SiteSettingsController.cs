//using MediaAcademy.Models.Controls;
using MediaAcademy.DAL.Repositories;
using MediaAcademy.Models.Entities.Auth_Tables;
using MediaAcademy.Models.Resources;
using MediaAcademy.Models.ViewModels;
using MediaAcademy.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Security.Claims;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MediaAcademy.Web.Controllers
{
    public class SiteSettings : _BaseController
    {
        private readonly IStringLocalizer<BusRes> _localizer;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserRepo userRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        IServiceProvider _ServiceProvider { get; }
        EmailService _emailService;
        SMSService _SMSService;
        private IConfiguration _config;

        public SiteSettings(UserManager<User> userManager, UserRepo userRepository,
            DAL.EnitiyDAL.ApplicationDbContext _db,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IConfiguration _config
            , IStringLocalizer<BusRes> localizer
            , EmailService emailService, SMSService SMSService, IServiceProvider _ServiceProvider, IHostingEnvironment hostingEnvironment)
        {
            db = _db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.userRepository = userRepository;

            this._ServiceProvider = _ServiceProvider;
            _emailService = emailService;
            _SMSService = SMSService;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
            this._config = _config;
        }
        public async Task<IActionResult> sendEmailTest(string email)
        {
            email = email ?? "abdullah.siary@gmail.com";
            string host = _config.GetValue<string>("Smtp:Server", "defaultmailserver");
            int port = _config.GetValue<int>("Smtp:Port", 25);
            string fromAddress = _config.GetValue<string>("Smtp:FromAddress", "defaultfromaddress");
            string mailPassword = _config.GetValue<string>("Smtp:Password", "defaultfromaddress");
            var return_result = new ResponseResult() { status = "success", message = String.Format("host:{0},port:{1},fromAddress:{2}", host, port, fromAddress) };

            try
            {
                string emailHtml = "";
                using (var scope = _ServiceProvider.CreateScope())
                {

                    IRazorViewEngine _razorViewEngine = scope.ServiceProvider.GetService<IRazorViewEngine>();
                    ITempDataProvider _tempDataProvider = scope.ServiceProvider.GetService<ITempDataProvider>();
                    IServiceProvider _serviceProvider = scope.ServiceProvider.GetService<IServiceProvider>();


                    EmailVM emailVM = new EmailVM();
                    emailVM.EmailStats = new List<EmailStat>();

                    emailVM.EmailStats.Add(new EmailStat() { tot = "ArabicTitle", desc = "ArabicSubject" });
                    emailVM.EmailStats.Add(new EmailStat() { tot = "EnglishTitle", desc = "EnglishSubject" });

                    emailVM.HeadOne = "المركز الإعلامي الافتراضي";
                    emailVM.HeadDesc = "إعلان جديد";


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


                    // headerImage = Path.Combine(_hostingEnvironment.WebRootPath, "assets", "media", "logos", "mailHeader.jpg");
                    headerImage = "https://momakssa.blob.core.windows.net/ad-dev/mailheader.png";// String.Format("data:image/jpg;base64,{0}", headerImage);

                    emailVM.headerImage = headerImage;

                    emailHtml = new ViewRenderService(_razorViewEngine, _tempDataProvider, _serviceProvider).RenderToStringAsync("_NotifyEmail", emailVM).Result;
                }


                // var res_list= _emailService.SendEmailAsync("إعلان جديدة", emailHtml, new List<string>() { email }, new List<string>() { "aalsaiari@media.gov.sa" });
                var res = _emailService.SendEmailToJournal("إعلان جديدة", emailHtml, email).Result;
                return Json(res);
            }
            catch (Exception ex)
            {
                return_result = new ResponseResult()
                {
                    status = "error",
                    btnClass = "btn btn-danger",
                    title = GeneralRes.ErrorTitle,
                    message = String.Format("host:{0},port:{1},fromAddress:{2}", host, port, fromAddress) + "   ," + ex.Message,
                    close = false
                };
            }
            return Json(return_result);
        }
        public ActionResult _NotifyEmail(long? sub_id)
        {

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


            headerImage = "https://momakssa.blob.core.windows.net/ad-dev/mailheader.png";// String.Format("data:image/jpg;base64,{0}", headerImage);
            //headerImage = String.Format("data:image/jpg;base64,{0}", headerImage);

            var ssss = new List<EmailStat>();
            ssss.Add(new EmailStat { desc = "محتوى ثاني", tot = "520" });
            ssss.Add(new EmailStat { desc = "محتوى أول", tot = "20" });
            ssss.Add(new EmailStat { desc = "محتوى ثالث", tot = "30" });
            ssss.Add(new EmailStat { desc = "محتوى رابع", tot = "40" });
            return View(new EmailVM { headerImage = headerImage, HeadDesc = "تم اعتماد الإفادة في النظام ، كما تم التوجيه من قبل معالي الوزير", HeadOne = "إشعار لملف ", EmailStats = ssss });
        }

        public IActionResult ChangeThame(string id)
        {
            var myCookie = Request.Cookies["SiteTheme"] ?? "blue";
            SetCookie("SiteTheme", myCookie, 500);
            //CookieOptions
            //HttpCookie myCookie = new HttpCookie("SiteTheme");
            //myCookie.Expires = DateTime.Now.AddDays(30);
            //myCookie.Value = id;
            //Response.Cookies.Add(myCookie);
            var result = new { isValid = true };
            return Json(result);
        }

        public IActionResult ChangeLanguage(string CurrentLang, string URL)
        {
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLang??"ar-sa");
            var myCookie = Request.Cookies["SiteLang"] ?? "ar-sa";
            SetCookie("SiteLang", myCookie, 500);
            //HttpCookie myCookie = new HttpCookie("SiteLang");
            //myCookie.Expires = DateTime.Now.AddDays(30);
            //myCookie.Value = CurrentLang ?? "ar-sa";
            //Response.Cookies.Add(myCookie);
            return Redirect(URL);
            //switch (CurrentLang)
            //{
            //    case "English":
            //        //What would I do here?
            //        break;

            //    case "French":
            //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-sa");
            //        break;
            //}

            var result = new { isValid = true };
            return Json(result);
        }
        [HttpPost]
        [AllowAnonymous()]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {

            var cultureInfo = new CultureInfo(culture);

            cultureInfo.DateTimeFormat.DateSeparator = "/";
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";// "dd/MM/yyyy";
            cultureInfo.DateTimeFormat.LongDatePattern = "yyyy/MM/dd HH:mm";//"dd/MM/yyyy";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Response.Cookies.Append("Culture", culture);
            return LocalRedirect(returnUrl);
        }
        private void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }
        public IActionResult ControlMenu2()
        {
            string Id = getUserId();// User.Identity.GetUserId();
            List<ControlMenu> menuLinks = new List<ControlMenu>();
            return PartialView(menuLinks);
        }
        public IActionResult Breadcrumb()
        {
            var ccc = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims;
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dd = User.Identity.Name;
            List<ControlMenu> menuLinks = new List<ControlMenu>();

            return PartialView(menuLinks);
        }
        public IActionResult ControlProfile()
        {
            string Id = getUserId();
            var s = db.Users.FirstOrDefault(x => x.Id == Id);
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var sss = User.FindFirstValue("EventId");

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
            }
            LoggedUser logged = new LoggedUser
            {
                Email = s.Email,
                FullName = s.UserFullName,
                Mobile = s.PhoneNumber,
                UserID = Id,
                UserName = s.UserName,
            };
            return PartialView(logged);
        }
        public IActionResult Nav()
        {
            var ccc = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims;
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dd = User.Identity.Name;
            List<ControlMenu> menuLinks = new List<ControlMenu>();

            return PartialView(menuLinks);
        }



    }

}