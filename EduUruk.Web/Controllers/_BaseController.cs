using EduUruk.DAL.EnitityDAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduUruk.Web.Controllers
{
    [Authorize()]
    public class _BaseController : Controller
    {
        public ApplicationDbContext db;

        public string getUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Id;
        }
        public string getEventId()
        {
            var Id = User.FindFirstValue("EventId");

            return Id;
        }
        public string getUserFullName()
        {
            var Id = User.FindFirstValue("FullName");
            return Id;
        }
        public string getUserEmail()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return email;
        }
    }
    public static class StatusSubTaskFlag
    {
        public static string MINI { get; set; } = "MINI";
        public static string OFFI { get; set; } = "OFFI";
        public static string EXEC { get; set; } = "EXEC";
        public static string DONE { get; set; } = "DONE";
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