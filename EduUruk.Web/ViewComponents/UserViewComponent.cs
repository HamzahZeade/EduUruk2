using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduUruk.Web.ViewComponents
{
	public class UserViewComponent : ViewComponent
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserViewComponent(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		public IViewComponentResult Invoke()
		{
			var user = new LoggedUser();
			var Id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (Id != null)
			{
				using (UserRepo userRepository = new UserRepo(_context, _userManager, _roleManager))
				{
					user = userRepository.GetUserProfileFormByID(Id);

					var roleNames = (_userManager.GetRolesAsync(userRepository.GetByID(Id)).Result).ToArray();
					user.Roles = string.Join(", ", _roleManager.Roles.Where(r => roleNames.AsEnumerable().Contains(r.Name)).Select(r => r.ArabicName).ToArray());
				}
			}
			return View(user);
		}
	}
}
