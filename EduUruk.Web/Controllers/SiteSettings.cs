using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduUruk.Web.Controllers
{
    public class SiteSettings : _BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly ApplicationDbContext _context;

        public SiteSettings(ApplicationDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
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
            string Id = "JavaMan";// User.Identity.GetUserId();
            List<ControlMenu> menuLinks = new List<ControlMenu>();

            using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
            {
                menuLinks = userRepository.GetMenu(Id);
            }

            // string Id = User.Identity.GetUserId();
            //   List<ControlMenu> menuLinks = new List<ControlMenu>();
            //menuLinks.Add(new Models.Controls.ControlMenu
            //{
            //    GroupID = 1
            //    ,
            //    GroupOrder = 0,
            //    MenuID = 1,
            //    MenuIcon = "icon-home",
            //    MenuTitle = "الرئيسية"
            //});
            //menuLinks.Add(new Models.Controls.ControlMenu
            //{
            //    GroupID = 1
            //   ,
            //    GroupOrder = 1,
            //    MenuID = 2,
            //    MenuIcon = "icon-home",
            //    MenuTitle = "جديد"
            //});
            //using (EmployeeRepository userRepository = new EmployeeRepository())
            //{
            //    menuLinks = userRepository.GetMenu(Id);
            //}

            return PartialView(menuLinks);
        }
        //public IActionResult ControlMenu()
        //{
        //    var ccc = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims;
        //    var full = ccc.SingleOrDefault(m => m.Type == "FullName");
        //    //  /string ssId =   User.Identity.Name;
        //    var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    List<ControlMenu> menuLinks = new List<ControlMenu>();

        //    using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
        //    {
        //        menuLinks = userRepository.GetMenu(Id);
        //    }
        //    //menuLinks.Add(new Models.Controls.ControlMenu
        //    //{
        //    //    GroupID = 1
        //    //    ,
        //    //    GroupOrder = 0,
        //    //    MenuID = 1,
        //    //    MenuIcon = "icon-home",
        //    //    MenuTitle = "الرئيسية"
        //    //});
        //    //menuLinks.Add(new Models.Controls.ControlMenu
        //    //{
        //    //    GroupID = 1
        //    //   ,
        //    //    GroupOrder = 1,
        //    //    MenuID = 2,
        //    //    MenuIcon = "icon-home",
        //    //    MenuTitle = "جديد"
        //    //});
        //    //using (EmployeeRepository userRepository = new EmployeeRepository())
        //    //{
        //    //    menuLinks = userRepository.GetMenu(Id);
        //    //}

        //    return PartialView(menuLinks);
        //}
        public IActionResult ControlMenu()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ControlMenu> menuLinks = new List<ControlMenu>();

            using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
            {
                menuLinks = userRepository.GetMenu(Id);
            }

            return View("_SideNav", menuLinks);
        }
        //public IActionResult ControlProfile()
        //{
        //    string Id = "";// User.Identity.GetUserId();

        //    //LoggedUser logged = new LoggedUser
        //    //{
        //    //    Email = "My Emmail",
        //    //    FullName = "My Name",
        //    //    Mobile = "96666",
        //    //    UserID = "1",
        //    //    UserName = "1",
        //    //};
        //    //using (UserRep employeeRepository = new UserRep())
        //    //{
        //    //    logged = employeeRepository.GetUserProfileFormByID(Id);
        //    //}

        //    return PartialView(logged);
        //}
    }
}
