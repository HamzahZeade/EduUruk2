using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduUruk.Web.ViewComponents
{
	public class NavViewComponent : ViewComponent
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public NavViewComponent(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		public IViewComponentResult Invoke()
		{
			var Username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			List<ControlMenu> menuLinks = new List<ControlMenu>();

			using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
			{
				menuLinks = userRepository.GetMenu(Username);
			}
			return View(menuLinks.ToList());
		}
	}
}
