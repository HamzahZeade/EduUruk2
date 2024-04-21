using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;

namespace EduUruk.DAL.Repositories
{
    public class RoleRepo : DBProvider
    {
        private readonly ApplicationDbContext DbContext;

        public RoleRepo(ApplicationDbContext context)
        {
            DbContext = context;
        }
        public List<Role> GetCustom(SearchFilters filter, out int totalRecords, out int recordsFiltered)
        {
            totalRecords = recordsFiltered = 0;
            List<Role> Roles = new List<Role>();

            var search = (from a in DbContext.Roles select a);
            totalRecords = recordsFiltered = search.Count();
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                search = search.Where(c => c.ArabicName.Contains(filter.SearchString) || c.Name.Contains(filter.SearchString) ||
                                           c.Id.ToString().Contains(filter.SearchString));
                recordsFiltered = search.Count();
            }
            if (!string.IsNullOrEmpty(filter.SortColumn) && !string.IsNullOrEmpty(filter.SortDirection))
            {
                // search = search.OrderBy(filter.SortColumn + " " + filter.SortDirection);
            }
            return search.Skip(filter.StartIndex).Take(filter.PageLength).ToList();
        }

        public RoleForm GetCreateRoleForm()
        {
            RoleForm form = new RoleForm();

            return form;
        }

        public RoleForm GetEditRoleForm(string roleId)
        {
            RoleForm form = new RoleForm();
            Role role = DbContext.Roles.FirstOrDefault(r => r.Id == roleId);
            form.RoleID = roleId;
            form.ArabicRoleName = role.ArabicName;
            form.EnglishRoleName = role.Name;

            return form;
        }

        public RoleForm GetRolePermissionForm(string roleId)
        {
            RoleForm form = new RoleForm();
            Role role = DbContext.Roles.FirstOrDefault(r => r.Id == roleId);

            form.RoleID = roleId;
            form.ArabicRoleName = role.ArabicName;
            form.EnglishRoleName = role.Name;
            List<RolePermission> permissions = role.Permissions.ToList(); // REHAB 18-4-2019
            List<Page> Pages = DbContext.Pages.ToList();
            foreach (Page page in Pages)
            {
                RolePermission pagePermission = permissions.FirstOrDefault(p => p.PageID == page.PageID);
                if (pagePermission != null)
                {
                    form.Permissions.Add(new RolePermission()
                    {
                        PageID = page.PageID,
                        Page = page,
                        CanActive = pagePermission.CanActive,
                        CanCreate = pagePermission.CanCreate,
                        CanDelete = pagePermission.CanDelete,
                        CanEdit = pagePermission.CanEdit,
                        CanPrint = pagePermission.CanPrint,
                        CanSelect = pagePermission.CanSelect
                    });
                }
                else
                {
                    form.Permissions.Add(new RolePermission()
                    {
                        PageID = page.PageID,
                        Page = page
                    });
                }
            }

            return form;
        }

        public Role GetByID(string id)
        {
            return DbContext.Roles.FirstOrDefault(p => p.Id == id);
        }

        public List<SelectControl> GetForInput(bool placeholder)
        {
            List<SelectControl> Groups = new List<SelectControl>();
            try
            {
                List<Role> groups = DbContext.Roles.ToList();

                if (placeholder)
                {
                    Groups.Add(new SelectControl()
                    {
                        Text = EduUruk.Models.Resources.GeneralRes.Ratelanguage == "ar" ? "حدد الصلاحية ...." : "Select role ....",
                        Value = null
                    });
                }
                Groups.AddRange((from g in groups
                                 select new SelectControl
                                 {
                                     Text = EduUruk.Models.Resources.GeneralRes.Ratelanguage == "ar" ? g.ArabicName : g.Name,
                                     Value = g.Id.ToString()
                                 }).ToList());
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "UserRoleRep/GetForInput", "");
                Groups = new List<SelectControl>();
            }
            return Groups;
        }

        public ResponseResult Add(RoleForm form)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    Role role = new Role(form.EnglishRoleName, form.ArabicRoleName);
                    DbContext.Roles.Add(role);
                    DbContext.SaveChanges();
                    result = new ResponseResult()
                    {
                        Status = "success",
                        BtnClass = "btn btn-success",
                        Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
                        Message = EduUruk.Models.Resources.SysModelRes.AddingSuccessfully,
                        Close = true
                    };
                }
                else
                {
                    result = new ResponseResult()
                    {
                        Status = "warning",
                        BtnClass = "btn btn-warning",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileProcessingData,
                        Close = false
                    };
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "UserRoleRep/Add", "");
                result = new ResponseResult()
                {
                    Status = "error",
                    BtnClass = "btn btn-danger",
                    Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                    Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                    Close = false
                };
            }
            return result;
        }

        public ResponseResult Edit(RoleForm form)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    Role role = DbContext.Roles.FirstOrDefault(r => r.Id == form.RoleID);
                    role.Name = form.EnglishRoleName;
                    role.NormalizedName = form.EnglishRoleName;
                    role.ArabicName = form.ArabicRoleName;
                    DbContext.SaveChanges();
                    result = new ResponseResult()
                    {
                        Status = "success",
                        BtnClass = "btn btn-success",
                        Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
                        Message = EduUruk.Models.Resources.SysModelRes.EditingSuccessfully,
                        Close = true
                    };
                }
                else
                {
                    result = new ResponseResult()
                    {
                        Status = "warning",
                        BtnClass = "btn btn-warning",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileProcessingData,
                        Close = false
                    };
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "UserRoleRep/Edit", "");
                result = new ResponseResult()
                {
                    Status = "error",
                    BtnClass = "btn btn-danger",
                    Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                    Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                    Close = false
                };
            }

            return result;
        }

        public ResponseResult EditPermission(RoleForm form)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    List<RolePermission> permissions = DbContext.RolePermissions.Where(p => p.RoleId == form.RoleID).ToList();
                    DbContext.RolePermissions.RemoveRange(permissions);
                    foreach (RolePermission perm in form.Permissions)
                    {
                        DbContext.RolePermissions.Add(new RolePermission()
                        {
                            PageID = perm.PageID,
                            RoleId = form.RoleID,
                            CanActive = perm.CanActive,
                            CanCreate = perm.CanCreate,
                            CanDelete = perm.CanDelete,
                            CanEdit = perm.CanEdit,
                            CanPrint = perm.CanPrint,
                            CanSelect = perm.CanSelect
                        });
                    }
                    DbContext.SaveChanges();
                    result = new ResponseResult()
                    {
                        Status = "success",
                        BtnClass = "btn btn-success",
                        Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
                        Message = EduUruk.Models.Resources.SysModelRes.EditingSuccessfully,
                        Close = true
                    };
                }
                else
                {
                    result = new ResponseResult()
                    {
                        Status = "warning",
                        BtnClass = "btn btn-warning",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileProcessingData,
                        Close = false
                    };
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "UserRoleRep/Edit", "");
                result = new ResponseResult()
                {
                    Status = "error",
                    BtnClass = "btn btn-danger",
                    Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                    Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileSaving,
                    Close = false
                };
            }

            return result;
        }

        public ResponseResult Delete(string roleId)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    Role role = DbContext.Roles.Find(roleId);
                    List<RolePermission> permissions = DbContext.RolePermissions.Where(p => p.RoleId == roleId).ToList();
                    DbContext.RolePermissions.RemoveRange(permissions);
                    DbContext.Roles.Remove(role);
                    DbContext.SaveChanges();
                    result = new ResponseResult()
                    {
                        Status = "success",
                        BtnClass = "btn btn-success",
                        Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
                        Message = EduUruk.Models.Resources.SysModelRes.DeletingSuccessfully,
                        Close = true
                    };
                }
                else
                {
                    result = new ResponseResult()
                    {
                        Status = "warning",
                        BtnClass = "btn btn-warning",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.ErrorWhileProcessingData,
                        Close = false
                    };
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("REFERENCE"))  // REHAB 20-4-2019
                {
                    result = new ResponseResult()
                    {
                        Status = "warning",
                        BtnClass = "btn btn-warning",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.SysModelRes.RelatedDataException,
                        Close = false
                    };
                }
                else
                {
                    ExceptionLog.AddException(ex, "UserRoleRep/Delete", "");
                    result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
                        Message = EduUruk.Models.Resources.GeneralRes.GeneralExceptionMessage,
                        Close = false
                    };
                }
            }

            return result;
        }
    }
}
