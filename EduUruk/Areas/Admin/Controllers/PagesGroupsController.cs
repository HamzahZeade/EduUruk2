
using Microsoft.AspNetCore.Mvc;
using System.Net;
//using MediaAcademy.Models.Controls;

namespace EduUruk.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesGroupsController : _BaseAdminController
    {


        public PagesGroupsController(DAL.EnitiyDAL.ApplicationDbContext context)
        {
            db = context;

        }
        // GET: PagesGroups      
        public ActionResult Index()
        {
            return View();
        }

        // GET: /PagesGroups/GetCustoms/
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
            using (PageGroupRepo groupRepository = new PageGroupRepo(db))
            {
                data = from a in (groupRepository.GetCustom(filter, out TotalRecords, out RecordsFiltered))
                       select new
                       {
                           id = a.GroupID,
                           name = a.GroupName,
                           name_ar = a.ArabicGroupName,
                           name_en = a.EnglishGroupName,
                           ico = a.GroupIco,
                           gorder = a.GroupOrder
                       };
            }
            return Json(new { draw = draw, recordsFiltered = RecordsFiltered, recordsTotal = TotalRecords, data = data });
        }

        // GET: /PagesGroups/Create
        public ActionResult Create()
        {
            return PartialView(new PageGroup());
        }

        // POST: /PagesGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(PageGroup group)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                using (PageGroupRepo groupRepository = new PageGroupRepo(ModelState.IsValid, db))
                {
                    result = groupRepository.Add(group);
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

        // GET: /PagesGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            PageGroup group;
            using (PageGroupRepo groupRepository = new PageGroupRepo(db))
            {
                group = groupRepository.GetByID(id.Value);
            }
            if (group == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return PartialView(group);
        }

        // POST: /PagesGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageGroup group)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                using (PageGroupRepo groupRepository = new PageGroupRepo(ModelState.IsValid, db))
                {
                    result = groupRepository.Edit(group);
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

        // GET: /PagesGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            PageGroup group;
            using (PageGroupRepo groupRepository = new PageGroupRepo(db))
            {
                group = groupRepository.GetByID(id.Value);
            }
            if (group == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return PartialView(group);
        }

        // POST: /PagesGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                using (PageGroupRepo groupRepository = new PageGroupRepo(ModelState.IsValid, db))
                {
                    result = groupRepository.Delete(id);
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

    }
}