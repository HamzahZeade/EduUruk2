
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
//using MediaAcademy.Models.Controls;

namespace EduUruk.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserRolesController : _BaseAdminController

    {
        // private readonly ApplicationDbContext _context;
        UserRoleRepo roleRepository;
        public UserRolesController(UserRoleRepo roleRepository)
        {
            this.roleRepository = roleRepository;
            if (CultureInfo.DefaultThreadCurrentUICulture == null)
            {
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-us");
            }
        }
        // GET: UsersRoles
        public IActionResult Index()
        {
            return View();
        }

        // GET: /UsersRoles/GetCustoms/
        [HttpPost]
        public IActionResult GetCustoms()
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
            List<Role> roles = new List<Role>();
            //using (UserRoleRepo roleRepository = new UserRoleRepo(_context))
            {
                roles = roleRepository.GetCustom(filter, out TotalRecords, out RecordsFiltered);
            }

            var data = from a in roles
                       select new
                       {
                           id = a.Id,
                           name = EduUruk.Models.Resources.GeneralRes.Ratelanguage == "ar" ? a.ArabicName : a.Name,
                           name_ar = a.ArabicName,
                           name_en = a.Name
                       };

            return Json(new { draw = draw, recordsFiltered = RecordsFiltered, recordsTotal = TotalRecords, data = data });
        }

        // GET: /UsersRoles/Create
        public IActionResult Create()
        {

            RoleForm form = new RoleForm();
            //using (UserRoleRepo roleRepository = new UserRoleRepo(_context))
            {
                form = roleRepository.GetCreateRoleForm();
            }
            return PartialView(form);
        }
        [HttpPost]
        public IActionResult Create1(int dd)
        {
            return Json(new ResponseResult());
        }
        // POST: /UsersRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public IActionResult Create(RoleForm group)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                try
                {
                    //using (UserRoleRepo roleRepository = new UserRoleRepo(_context, ModelState.IsValid))
                    {
                        result = roleRepository.Add(group);
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                        Close = false
                    };
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

        // GET: /UsersRoles/Edit/5
        public IActionResult Edit(string id)
        {
            RoleForm form = new RoleForm();
            if (string.IsNullOrEmpty(id))
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            else
            {
                //using (UserRoleRepo roleRepository = new UserRoleRepo(_context))
                {
                    form = roleRepository.GetEditRoleForm(id);

                }
            }

            return PartialView(form);
        }

        // POST: /UsersRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleForm group)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                try
                {
                    //using (UserRoleRepo roleRepository = new UserRoleRepo(_context, ModelState.IsValid))
                    {
                        result = roleRepository.Edit(group);
                    }

                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                        Close = false
                    };
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }

        // GET: /UsersRoles/Permissions/5
        public IActionResult Permissions(string id)
        {
            RoleForm form = new RoleForm();
            if (string.IsNullOrEmpty(id))
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            else
            {
                //using (UserRoleRepo groupRepository = new UserRoleRepo(_context))
                {
                    form = roleRepository.GetRolePermissionForm(id);

                }
            }

            return PartialView(form);
        }

        // POST: /UsersRoles/Permissions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Permissions(RoleForm group)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                try
                {
                    //using (UserRoleRepo roleRepository = new UserRoleRepo(_context, true))
                    {
                        result = roleRepository.EditPermission(group);
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                        Close = false
                    };
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }


        // GET: /UsersRoles/Delete/5
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Role role = null;
            //using (UserRoleRepo roleRepository = new UserRoleRepo(_context))
            {
                role = roleRepository.GetByID(id);
            }
            if (role == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return PartialView(role);
        }

        // POST: /UsersRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ResponseResult result = null;
                try
                {
                    //using (UserRoleRepo roleRepository = new UserRoleRepo(_context, ModelState.IsValid))
                    {
                        result = roleRepository.Delete(id);
                    }
                }
                catch (Exception ex)
                {
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                        Close = false
                    };
                }
                return Json(result);
            }
            else return StatusCode((int)HttpStatusCode.BadRequest);
        }
    }


}
