using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EduUruk.Web.ViewComponents
{
	public class BreadcrumbViewComponent : ViewComponent
	{
		private PageRepo _pageRepo;
		private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public BreadcrumbViewComponent(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_pageRepo = new PageRepo(_context); // pageRepo;
			_httpContextAccessor = httpContextAccessor;
		}

		public IViewComponentResult Invoke()
		{
			var currentPage = _httpContextAccessor.HttpContext.Request.Path;
			if (currentPage == "/")
				currentPage = "/Home/Index";

			var bc = _pageRepo.GetBreadcrumb(currentPage);

			return View(bc);
		}
	}
}
