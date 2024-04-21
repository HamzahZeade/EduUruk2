using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace EduUruk.DAL.Repositories
{
	public class UserRepo : DBProvider
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		private readonly ApplicationDbContext DbContext;
		public UserRepo(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			DbContext = context;
			_userManager = userManager;
			_roleManager = roleManager;

		}
		public UserRepo(ApplicationDbContext context)
		{
			DbContext = context;
		}
		public List<User> GetCustom2(SearchFilters filter, out int TotalRecords, out int RecordsFiltered)
		{
			TotalRecords = RecordsFiltered = 0;
			List<User> Users = new List<User>();

			var search = (from a in DbContext.Users select a);
			TotalRecords = RecordsFiltered = search.Count();
			var pageLength = filter.PageLength;
			if (pageLength > TotalRecords)
				pageLength = TotalRecords;
			if (!string.IsNullOrEmpty(filter.SearchString))
			{
				search = DbContext.Users.Where(c => c.UserName.Contains(filter.SearchString) ||
										   c.PhoneNumber.Contains(filter.SearchString) || c.Email.Contains(filter.SearchString));
				RecordsFiltered = search.Count();
			}

			if (!string.IsNullOrEmpty(filter.SortColumn) && filter.SortColumn == "name")
			{
				if (!string.IsNullOrEmpty(filter.SortDirection) && filter.SortDirection == "desc")
					search = search.OrderByDescending(x => x.UserName);
				else
					search = search.OrderBy(x => x.UserName);
			}
			else if (!string.IsNullOrEmpty(filter.SortColumn) && filter.SortColumn == "username")
			{
				if (!string.IsNullOrEmpty(filter.SortDirection) && filter.SortDirection == "desc")
					search = search.OrderByDescending(x => x.UserName);
				else
					search = search.OrderBy(x => x.UserName);
			}
			else if (!string.IsNullOrEmpty(filter.SortColumn) && filter.SortColumn == "email")
			{
				if (!string.IsNullOrEmpty(filter.SortDirection) && filter.SortDirection == "desc")
					search = search.OrderByDescending(x => x.Email);
				else
					search = search.OrderBy(x => x.Email);
			}
			else if (!string.IsNullOrEmpty(filter.SortColumn) && filter.SortColumn == "mobile")
			{
				if (!string.IsNullOrEmpty(filter.SortDirection) && filter.SortDirection == "desc")
					search = search.OrderByDescending(x => x.PhoneNumber);
				else
					search = search.OrderBy(x => x.PhoneNumber);
			}
			else if (!string.IsNullOrEmpty(filter.SortColumn) && filter.SortColumn == "createdDate")
			{
				if (!string.IsNullOrEmpty(filter.SortDirection) && filter.SortDirection == "desc")
					search = search.OrderByDescending(x => x.CreatedTime);
				else
					search = search.OrderBy(x => x.CreatedTime);
			}

			if (!string.IsNullOrEmpty(filter.SortColumn) && !string.IsNullOrEmpty(filter.SortDirection))
			{
				// search = search.OrderBy(filter.SortColumn + " " + filter.SortDirection);
			}
			return search.Skip(filter.StartIndex).Take(pageLength).ToList();
		}
		public List<User> GetCustom(SearchFilters filter, out int TotalRecords, out int RecordsFiltered)
		{
			TotalRecords = RecordsFiltered = 0;
			List<User> Users = new List<User>();

			var search = (from a in DbContext.Users select a);
			TotalRecords = RecordsFiltered = search.Count();
			var pageLength = filter.PageLength;
			if (pageLength > TotalRecords)
				pageLength = TotalRecords;
			if (!string.IsNullOrEmpty(filter.SearchString))
			{
				search = search.Where(c => c.UserName.Contains(filter.SearchString) ||
										   c.PhoneNumber.Contains(filter.SearchString) || c.Email.Contains(filter.SearchString) ||
										   c.UserName.Contains(filter.SearchString)
										   );
				RecordsFiltered = search.Count();
			}
			if (!string.IsNullOrEmpty(filter.SortColumn) && !string.IsNullOrEmpty(filter.SortDirection))
			{
				//search = search.OrderBy(filter.SortColumn + " " + filter.SortDirection);
			}
			try
			{
				search = search.OrderBy(x => x.UserName).ThenByDescending(x => x.CreatedTime);
				var list = search.Skip(filter.StartIndex).Take(pageLength).ToList();
				list.ForEach(x => x.Roles = _userManager.GetRolesAsync(x).Result.ToList());
				return list;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		public User GetByID(string id)
		{

			User emp = DbContext.Users.FirstOrDefault(r => r.Id == id);

			return emp;
		}
		public List<User> GetAll()
		{
			return DbContext.Users.ToList();

		}



		public UserForm GetCreateUserForm()
		{
			UserForm form = new UserForm { Createdby = "0", UpdatedBy = "0" };


			using (RoleRepo roleRepository = new RoleRepo(DbContext))
			{
				form.Roles.AddRange(roleRepository.GetForInput(false));
			}


			return form;
		}
		public UserForm GetEditUserForm(string id)
		{
			UserForm form = new UserForm();
			CultureInfo info = new CultureInfo("ar-sa");

			User emp = DbContext.Users.FirstOrDefault(r => r.Id == id);
			form.Id = id;
			form.UserId = id;
			form.Email = emp.Email;
			//  form.UserId = emp.UserId;
			//  form.ManagerID = emp.ManagerID;
			form.UserName = emp.UserName;
			var roleNames = (_userManager.GetRolesAsync(emp).Result).ToArray();
			var roleIds = _roleManager.Roles.Where(r => roleNames.AsEnumerable().Contains(r.Name)).Select(r => r.Id).ToArray();
			form.RoleIds = roleIds;//= (_userManager.GetRolesAsync(emp).Result).ToList().Select(r => r.id).ToArray();
			using (RoleRepo roleRepository = new RoleRepo(DbContext))
			{
				form.Roles.AddRange(roleRepository.GetForInput(false));
			}

			return form;
		}


		public ResponseResult UpdateStatus(string id, bool status)
		{
			ResponseResult result = new ResponseResult();
			try
			{
				User emp = DbContext.Users.FirstOrDefault(r => r.Id == id);
				DbContext.Users.Update(emp);
				DbContext.SaveChanges();
				result = new ResponseResult()
				{
					Status = "success",
					BtnClass = "btn btn-success",
					Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
					Message = "تم تعديل  بنجاح",
					Close = true
				};
			}
			catch (Exception ex)
			{
				ExceptionLog.AddException(ex, "Users/UpdateStatus", "");
				result = new ResponseResult()
				{
					Status = "error",
					BtnClass = "btn btn-danger",
					Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
					Message = "عذراً حدث خطأ اثناء تغيير حالة الموعد",
					Close = false
				};
			}
			return result;
		}
		public UserForm GetById(string Id)
		{
			try
			{
				UserForm form = new UserForm();
				User user = DbContext.Users.FirstOrDefault(r => r.Id == Id);

				CultureInfo info = new CultureInfo("ar-sa");

				form.Id = user.Id;
				form.UserId = user.Id;
				form.Email = user.Email;
				form.UserName = user.UserName;

				var roleNames = (_userManager.GetRolesAsync(user).Result).ToArray();
				var roleIds = _roleManager.Roles.Where(r => roleNames.AsEnumerable().Contains(r.Name)).Select(r => r.Id).ToArray();
				form.RoleIds = roleIds;//= (_userManager.GetRolesAsync(emp).Result).ToList().Select(r => r.id).ToArray();
				using (RoleRepo roleRepository = new RoleRepo(DbContext))
				{
					form.Roles.AddRange(roleRepository.GetForInput(false));
				}



				return form;
			}
			catch (Exception ex)
			{
				ExceptionLog.AddException(ex, "UserRepo/GetById", "");
				return new UserForm();
			}
		}

		public string GetFullName(string userName)
		{
			try
			{
				User user = DbContext.Users.FirstOrDefault(r => r.UserName == userName);

				return user.UserName;
			}
			catch (Exception ex)
			{
				ExceptionLog.AddException(ex, "UserRepo/GetFullName", "");
				return "";
			}
		}

		public async Task<ResponseResult> Add(UserForm form)
		{
			ResponseResult result = new ResponseResult();
			try
			{


				var existingUser = await _userManager.FindByEmailAsync(form.Email);
				if (existingUser != null)
				{
					result = new ResponseResult()
					{
						Status = "error",
						BtnClass = "btn btn-danger",
						Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
						Message = "Email already exists. Please choose a different email.",
						Close = false
					};
					return result;
				}

				if (string.IsNullOrEmpty(form.Password))
				{
					var pass = "@Aa171408";//model.Password;//AKmrUJ1CK5rz9gEGNnfFdvlWvhbJwmofZIkE1mVWJWsNsFAeO0yFIjLW1H1E2vdVEA==
					form.Password = pass;
				}

				if (IsModelValid(form))
				{

					CultureInfo info = new CultureInfo("ar-sa");
					var user = new User()
					{
						UserName = form.UserName,
						Email = form.Email,
						EmailConfirmed = true,
						Createdby = form.Createdby,
						UpdatedBy = form.UpdatedBy,
						CreatedTime = DateTime.Now
					};
					var res = await _userManager.CreateAsync(user, form.Password);
					//var customClaim_arr = new List<Claim>();
					//  customClaim_arr.Add(new Claim("FullName", user.UserFullName));
					await _userManager.AddClaimAsync(user, new Claim("UserName", user.UserName));
					if (res.Succeeded)
					{
						User User = DbContext.Users.FirstOrDefault(r => r.Id == user.Id);
						if (form.RoleIds != null)
						{
							if (form.RoleIds.Count() != 0)
							{
								foreach (string role in form.RoleIds)
								{
									var R = _roleManager.FindByIdAsync(role).Result;
									await _userManager.AddToRoleAsync(user, R.Name);

									//IdentityUserRole identityUserRole = new IdentityUserRole()
									//{
									//    RoleId = role,
									//    UserId = User.Id
									//};
									//User.Roles.Add(identityUserRole);
								}
								//DbContext.Entry(user).State = EntityState.Modified;
								//   DbContext.SaveChanges();
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
									Status = "success",
									BtnClass = "btn btn-success",
									Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
									Message = EduUruk.Models.Resources.SysModelRes.AddingSuccessfully,
									Close = true
								};
							}
						}
						else
						{
							result = new ResponseResult()
							{
								Status = "success",
								BtnClass = "btn btn-success",
								Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
								Message = EduUruk.Models.Resources.SysModelRes.AddingSuccessfully,
								Close = true
							};
						}


					}
					else
					{
						result = new ResponseResult()
						{
							Status = "error",
							BtnClass = "btn btn-danger",
							Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
							Message = "",// res.Errors.FirstOrDefault(),
							Close = false
						};
					}
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
				ExceptionLog.AddException(ex, "UserRep/Add", "");
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





		public List<Role> GetByRoles(string term)
		{
			try
			{
				return _roleManager.Roles.ToList();
				//return DbContext.Departments.Where(x => x.DepartmentName.Contains(term)).ToList();
			}
			catch (Exception ex)
			{
				ExceptionLog.AddException(ex, "OrgnaizationRepo/GetBySearch", "");
				return new List<Role>();
			}
		}

		public async Task<ResponseResult> Edit(UserForm form)
		{
			ResponseResult result = new ResponseResult();
			try
			{
				var existingUserWithEmail = await _userManager.FindByEmailAsync(form.Email);
				if (existingUserWithEmail != null && existingUserWithEmail.Id != form.Id)
				{
					// Email is already in use by another user
					result = new ResponseResult()
					{
						Status = "error",
						BtnClass = "btn btn-danger",
						Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
						Message = "Email already exists. Please choose a different email.",
						Close = false
					};
					return result;
				}

				if (IsModelValid(form))
				{
					User user = DbContext.Users.FirstOrDefault(r => r.Id == form.Id);
					if (user != null)
					{
						// Update user details
						user.UserName = form.UserName;
						user.Email = form.Email;
						user.UpdatedBy = form.UpdatedBy;
						user.UpdatdTime = DateTime.Now;

						await _userManager.UpdateAsync(user);

						// Change password
						if (!string.IsNullOrEmpty(form.Password))
						{
							var token = await _userManager.GeneratePasswordResetTokenAsync(user);
							var resetPassResult = await _userManager.ResetPasswordAsync(user, token, form.Password);
						}


						if (form.RoleIds.Length != 0)
						{
							// Remove existing roles
							var userRoles = await _userManager.GetRolesAsync(user);
							if (userRoles != null && userRoles.Any())
							{
								await _userManager.RemoveFromRolesAsync(user, userRoles);
							}

							// Add new role
							foreach (var item in form.RoleIds)
							{
								var role = await _roleManager.FindByIdAsync(item);
								if (role != null)
								{
									await _userManager.AddToRoleAsync(user, role.Name);
								}
							}
						}

						else
						{
							// Remove all roles if role ID is null or empty
							var userRoles = await _userManager.GetRolesAsync(user);
							if (userRoles != null && userRoles.Any())
							{
								await _userManager.RemoveFromRolesAsync(user, userRoles);
							}
						}

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
						// User not found
						result = new ResponseResult()
						{
							Status = "error",
							BtnClass = "btn btn-danger",
							Title = EduUruk.Models.Resources.GeneralRes.ErrorTitle,
							Message = "User not found.",
							Close = false
						};
					}
				}
				else
				{
					// Invalid model
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
				// Exception handling
				ExceptionLog.AddException(ex, "UserRep/Edit", "");
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





		public ResponseResult Delete(string id)
		{
			ResponseResult result = new ResponseResult();
			try
			{
				if (true)
				{
					User User = DbContext.Users.Find(id);
					DbContext.Users.Remove(User);
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
					ExceptionLog.AddException(ex, "UserRep/Delete", "");
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


		public ResponseResult Remove(string id)
		{
			ResponseResult result = new ResponseResult();
			try
			{
				var _exists = DbContext.Users.Find(id);
				if (_exists is null)
				{
					result = new ResponseResult()
					{
						Status = "exists",
						BtnClass = "btn btn-success",
						Title = EduUruk.Models.Resources.GeneralRes.SuccessTitle,
						Message = EduUruk.Models.Resources.SysModelRes.AddingSuccessfully,
						Close = true
					};
				}
				else
				{
					var user = DbContext.Users.Find(id);
					DbContext.Users.Remove(user);
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
			}
			catch (Exception ex)
			{
				ExceptionLog.AddException(ex, "GuestRepo/Remove", "");
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


		public LoggedUser GetUserProfileFormByID(string id)
		{
			LoggedUser form = new LoggedUser();
			User user = DbContext.Users.FirstOrDefault(r => r.Id == id);

			form.UserID = user.Id;
			form.Email = user.Email;
			form.UserName = user.UserName;
			form.Mobile = user.PhoneNumber;
			form.FullName = user.UserName;
			return form;
		}
		public List<ControlMenu> GetMenu(string Id)
		{
			List<ControlMenu> menuList = new List<ControlMenu>();
			//User User = GetByID(Id);
			//if (User == null)
			//{
			//	return menuList;
			//}
			//List<string> roleIds = _userManager.GetRolesAsync(User).Result.ToList();
			List<Page> pagess = DbContext.RolePermissions
				.Include(x => x.Page)
				.ThenInclude(Y => Y.Group)
				//.Where(p => roleIds.Contains(p.UserRole.Name) && p.CanSelect == true)
				.Select(x => x.Page)
				.ToList();

			foreach (Page page in pagess)
			{
				ControlMenu link = new ControlMenu()
				{
					MenuID = page.PageID,
					MenuLink = page.PageURL,
					MenuTitle = page.PageName,
					GroupOrder = page.PageIndex,
					MenuIcon = page.Ico,
				};
				if (page.GroupID.HasValue)
				{
					var parent = menuList.FirstOrDefault(m => m.GroupID == page.GroupID);
					if (parent != null)
					{
						parent.ChildLinks.Add(link);
					}
					else
					{
						ControlMenu parentMenu = new ControlMenu()
						{
							MenuID = page.Group.GroupID,
							MenuLink = page.PageURL,
							MenuTitle = page.Group.GroupName,
							GroupID = page.Group.GroupID,
							MenuIcon = page.Group.GroupIco,
							GroupOrder = page.Group.PageGroupIndex,  //.GroupOrder
						};
						parentMenu.ChildLinks.Add(link);
						menuList.Add(parentMenu);
					}
				}
				else
				{
					menuList.Add(link);
				}
			}
			menuList = menuList.OrderBy(m => m.GroupOrder).ToList();
			return menuList;
		}




	}
}
