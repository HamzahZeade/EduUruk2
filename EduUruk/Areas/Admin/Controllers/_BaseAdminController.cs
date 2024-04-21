using EduUruk.DAL.EnitityDAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduUruk.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class _BaseAdminController : Controller
    {
        public ApplicationDbContext db;
        // GET: _Base
        //public _BaseController(DAL.EnitiyDAL.ApplicationDbContext prm_db)
        //{
        //    db = prm_db;
        //}
        public string getUserId()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Id;
        }
    }
}
