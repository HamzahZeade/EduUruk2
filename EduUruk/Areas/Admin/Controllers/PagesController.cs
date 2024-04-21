
using Microsoft.AspNetCore.Mvc;
using System.Net;
////using MediaAcademy.Models.Controls;

namespace EduUruk.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : _BaseAdminController
    {


        public PagesController(DAL.EnitiyDAL.ApplicationDbContext context)
        {
            db = context;

        }
        // GET: Pages    
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Pages/GetCustoms/
        [HttpPost]
        public ActionResult GetCustoms()
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
            dynamic data = null;
            using (PageRepo PageRepo = new PageRepo(db))
            {
                data = from a in (PageRepo.GetCustom(filter, out TotalRecords, out RecordsFiltered))
                       select new
                       {
                           id = a.PageID,
                           name = a.PageName,
                           group_name = a.GroupID.HasValue ? a.Group.GroupName : "", // فى حالة كانت الصفحة لها مجموعة يعرض اسم المجموعة غير ذلك يعرض نص فارغ
                           name_ar = a.ArabicPageName,
                           name_en = a.EnglishPageName,
                           ico = a.PageURL
                       };
            }
            return Json(new { draw = draw, recordsFiltered = RecordsFiltered, recordsTotal = TotalRecords, data = data });
        }

        // GET: /Pages/Create
        public ActionResult Create()
        {
            PageForm page;
            using (PageRepo PageRepo = new PageRepo(db))
            {
                page = PageRepo.GetPageCreateFormAsync();
            }
            return PartialView(page);
        }

        // POST: /Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PageForm page)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                using (PageRepo PageRepo = new PageRepo(ModelState.IsValid, db))
                {
                    result = PageRepo.Add(page);
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

        // GET: /Pages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            PageForm page;
            using (PageRepo PageRepo = new PageRepo(db))
            {
                page = PageRepo.GetPageEditFormAsync(id.Value);
            }
            if (page == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return PartialView(page);
        }

        // POST: /Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageForm page)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                using (PageRepo PageRepo = new PageRepo(ModelState.IsValid, db))
                {
                    result = PageRepo.Edit(page);
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

        // GET: /Pages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Page page;
            using (PageRepo PageRepo = new PageRepo(db))
            {
                page = PageRepo.GetByID(id.Value);
            }
            if (page == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return PartialView(page);
        }

        // POST: /Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                using (PageRepo PageRepo = new PageRepo(ModelState.IsValid, db))
                {
                    result = PageRepo.Delete(id);
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

    }
}