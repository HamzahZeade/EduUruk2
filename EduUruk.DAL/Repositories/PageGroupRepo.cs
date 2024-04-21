using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduUruk.DAL.Repositories
{
    public class PageGroupRepo : DBProvider
    {
        private readonly ApplicationDbContext DbContext;

        public PageGroupRepo(ApplicationDbContext context)
        {
            DbContext = context;
        }


        public PageGroupRepo(bool isValid, ApplicationDbContext context)
        {
            IsModelStateValid = isValid;
            DbContext = context;
        }

        public List<PageGroup> GetAll()
        {
            try
            {
                return DbContext.PagesGroups.ToList();
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "PageGroupRep/GetAll", "");
                return new List<PageGroup>();
            }
        }

        public List<PageGroup> GetCustom(SearchFilters filter, out int totalRecords, out int recordsFiltered)
        {
            totalRecords = recordsFiltered = 0;
            var search = (from a in DbContext.PagesGroups select a);
            totalRecords = recordsFiltered = search.Count();
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                search = search.Where(c => c.ArabicGroupName.Contains(filter.SearchString) || c.EnglishGroupName.Contains(filter.SearchString) ||
                                           c.GroupID.ToString().Contains(filter.SearchString));
                recordsFiltered = search.Count();
            }
            if (!string.IsNullOrEmpty(filter.SortColumn) && !string.IsNullOrEmpty(filter.SortDirection))
            {
                //  search = search.OrderBy(filter.SortColumn + " " + filter.SortDirection);
            }
            return search.Skip(filter.StartIndex).Take(filter.PageLength).ToList();
        }

        public List<SelectControl> GetForInput(bool placeholder)
        {
            List<SelectControl> Groups = new List<SelectControl>();
            try
            {
                List<PageGroup> groups = DbContext.PagesGroups.ToList();

                if (placeholder)
                {
                    Groups.Add(new SelectControl()
                    {
                        Text = EduUruk.Models.Resources.GeneralRes.Ratelanguage == "ar" ? "حدد المجموعة ...." : "Select Group ....",
                        Value = null
                    });
                }
                Groups.AddRange((from g in groups
                                 select new SelectControl
                                 {
                                     Text = g.GroupName,
                                     Value = g.GroupID.ToString()
                                 }).ToList());
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "PageGroupRep/GetForInput", "");
                Groups = new List<SelectControl>();
            }
            return Groups;
        }

        public PageGroup GetByID(int id)
        {
            return DbContext.PagesGroups.FirstOrDefault(p => p.GroupID == id);
        }

        public ResponseResult Add(PageGroup group)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    DbContext.PagesGroups.Add(group);
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
                ExceptionLog.AddException(ex, "PageGroupRep/Add", "");
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

        public ResponseResult Edit(PageGroup group)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    DbContext.Entry(group).State = EntityState.Modified;
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
                ExceptionLog.AddException(ex, "PageGroupRep/Edit", "");
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

        public ResponseResult Delete(int id)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    PageGroup group = DbContext.PagesGroups.Find(id);
                    DbContext.PagesGroups.Remove(group);
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
                if (ex.InnerException.InnerException.Message.Contains("REFERENCE"))
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
                    ExceptionLog.AddException(ex, "PageGroupRep/Delete", "");
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

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}
