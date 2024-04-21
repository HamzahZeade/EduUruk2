using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduUruk.DAL.Repositories
{
    public class PageRepo : DBProvider
    {
        private readonly ApplicationDbContext DbContext;

        public PageRepo(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public PageRepo(bool isValid, ApplicationDbContext context)
        {
            IsModelStateValid = isValid;
            DbContext = context;
        }

        public List<Page> GetAll()
        {
            try
            {
                return DbContext.Pages.ToList();
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "PageRep/GetAll", "");
                return new List<Page>();
            }
        }

        public List<Page> GetCustom(SearchFilters filter, out int totalRecords, out int recordsFiltered)
        {
            totalRecords = recordsFiltered = 0;
            var search = (from a in DbContext.Pages.Include("Group") select a);
            totalRecords = recordsFiltered = search.Count();
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                search = search.Where(c => c.ArabicPageName.Contains(filter.SearchString) || c.PageID.ToString().Contains(filter.SearchString)
                                            || c.Group.ArabicGroupName.ToString().Contains(filter.SearchString));

                recordsFiltered = search.Count();
            }
            if (!string.IsNullOrEmpty(filter.SortColumn) && !string.IsNullOrEmpty(filter.SortDirection))
            {
                //search = search.OrderBy(filter.SortColumn + " " + filter.SortDirection);
            }
            return search.Skip(filter.StartIndex).Take(filter.PageLength).ToList();
        }

        public Page GetByID(int id)
        {
            return DbContext.Pages.FirstOrDefault(p => p.PageID == id);
        }

        public List<Page> GetBreadcrumb(string currentPage)
        {
            try
            {
                return DbContext.Pages.Where(x => x.PageURL.Contains(currentPage)).Include(x => x.Group).ToList();
            }
            catch (Exception ex)
            {
                ExceptionLog.AddException(ex, "PageRep/GetAll", "");
                return new List<Page>();
            }
        }

        public PageForm GetPageCreateFormAsync()
        {
            PageForm form = new PageForm();
            form.PageID = 0;

            using (PageGroupRepo groupRepository = new PageGroupRepo(DbContext))
            {
                form.Groups.AddRange(groupRepository.GetForInput(true));
            }

            return form;
        }

        public PageForm GetPageEditFormAsync(int id)
        {
            Page page = GetByID(id);
            PageForm form = new PageForm()
            {
                PageID = page.PageID,
                ArabicPageName = page.ArabicPageName,
                EnglishPageName = page.EnglishPageName,
                PageURL = page.PageURL,
                GroupID = page.GroupID,
                PageIndex = page.PageIndex,
                Ico = page.Ico,
            };

            using (PageGroupRepo groupRepository = new PageGroupRepo(DbContext))
            {
                form.Groups.AddRange(groupRepository.GetForInput(true));
            }

            return form;
        }

        public ResponseResult Add(PageForm form)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    Page page = new Page()
                    {
                        ArabicPageName = form.ArabicPageName,
                        EnglishPageName = form.EnglishPageName,
                        PageURL = form.PageURL,
                        GroupID = form.GroupID,
                        PageIndex = form.PageIndex.Value,
                        Ico = form.Ico

                    };
                    DbContext.Pages.Add(page);
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
                ExceptionLog.AddException(ex, "PageRep/Add", "");
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

        public ResponseResult Edit(PageForm form)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (IsModelStateValid)
                {
                    Page page = DbContext.Pages.FirstOrDefault(p => p.PageID == form.PageID);
                    page.ArabicPageName = form.ArabicPageName;
                    page.EnglishPageName = form.EnglishPageName;
                    page.PageURL = form.PageURL;
                    page.GroupID = form.GroupID;
                    page.PageIndex = form.PageIndex.Value;
                    page.Ico = form.Ico;
                    DbContext.Entry(page).State = EntityState.Modified;

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
                ExceptionLog.AddException(ex, "PageRep/Edit", "");
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
                    Page page = DbContext.Pages.Find(id);
                    DbContext.Pages.Remove(page);
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
                    ExceptionLog.AddException(ex, "PageRep/Delete", "");
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
