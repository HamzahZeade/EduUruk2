using EduUruk.DAL.EnitityDAL;
using EduUruk.DAL.Repositories;
using EduUruk.Models.Auth_Tables;
using EduUruk.Models.Resources;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduUruk.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly ApplicationDbContext _context;
        private readonly UserRepo _userRepository;
        private IConfiguration _config;

        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            UserRepo userRepository,
            IConfiguration config, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _userRepository = userRepository;
            _config = config;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }




        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginForm { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginForm model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.UserName.Split('@')[0];
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        // Handle unconfirmed email accounts
                        // Redirect the user to a page to resend the confirmation email or provide instructions
                        return RedirectToAction("UnconfirmedEmail", "Account");
                    }

                    // Trim leading and trailing whitespaces from the entered password
                    var trimmedPassword = model.Password.Trim();

                    // Check if the trimmed password matches the hashed password
                    var result = await _signInManager.CheckPasswordSignInAsync(user, trimmedPassword, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Assuming user.Id is the unique identifier for the user
                                    new Claim(ClaimTypes.Name, user.UserName), // Assuming user.UserName is the username
                                    // Add other claims as needed, such as roles, custom claims, etc.
                                    // Example: new Claim(ClaimTypes.Role, "Administrator")
                                };


                        // Get roles of the user
                        var userRoles = await _userManager.GetRolesAsync(user);
                        foreach (var role in userRoles)
                        {
                            // Add a claim for each role
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        // Create the ClaimsIdentity using the specified authentication scheme
                        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Check if userIdentity is not null before proceeding with signing in the user
                        if (userIdentity != null)
                        {
                            // Sign in the user using the created ClaimsIdentity
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));

                            // Redirect the user to a protected resource or dashboard
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // Handle the case where userIdentity is null, log an error, or throw an exception
                            // Example: _logger.LogError("userIdentity is null");
                            throw new Exception("userIdentity is null");
                        }


                    }
                    else
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        // Handle invalid password
                        ModelState.AddModelError(string.Empty, "Invalid password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }

            return View(model);
        }


        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //}
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterForm register)
        {
            if (!ModelState.IsValid) return View();
            User newUser = new User
            {
                Email = register.Email,
                UserName = register.UserName,
                Createdby = register.UserName,
                UpdatedBy = register.UserName,
                UserType = "studant"
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login22(LoginForm Input)
        {
            Input.ReturnUrl ??= Url.Content("~/");
            var return_result = new ResponseResult();


            try
            {
                if (ModelState.IsValid)
                {
                    Input.UserName = Input.UserName.Split('@')[0];
                    var user = await _userManager.FindByNameAsync(Input.UserName);

                    if (user == null)
                    {
                        return_result = new ResponseResult()
                        {
                            Status = "error",
                            BtnClass = "btn btn-danger",
                            Title = GeneralRes.ErrorTitle,
                            Message = SysModelRes.InvalidLoginData,
                            Close = false
                        };

                    }
                    else
                    {

                        var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: false);
                        // في حالة تسجيل الدخول من قاعدة بيانات انلظام غير صحيح :
                        if (!result.Succeeded)
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            //var res = await _userManager.ResetPasswordAsync(user, token, Input.Password);
                        }

                        else
                        {
                            var customClaim_arr = new List<Claim>();
                            // customClaim_arr.Add(new Claim("FullName", user.UserFullName));

                            #region default page
                            List<string> roleIds = _userManager.GetRolesAsync(user).Result.ToList();
                            if (roleIds.Count > 0)
                            {
                                var def_page = _roleManager.FindByNameAsync(roleIds[0]).Result;
                            }
                            else
                            {
                                return_result = new ResponseResult()
                                {
                                    Status = "error",
                                    BtnClass = "btn btn-danger",
                                    Title = GeneralRes.ErrorTitle,
                                    Message = "عذراً، لا يوجد لديك صلاحية للدخول للنظام",
                                    //  url = Input.ReturnUrl,
                                    Close = false
                                };
                                return Json(return_result);
                            }
                            #endregion
                            await _signInManager.SignInWithClaimsAsync(user, Input.RememberMe, customClaim_arr);

                            return_result = new ResponseResult()
                            {
                                Status = "success",
                                BtnClass = "btn btn-success",
                                Title = GeneralRes.SuccessTitle,
                                Message = "تم تسجيل الدخول بنجاح",
                                URL = Input.ReturnUrl,
                                Close = false
                            };
                        }

                    }
                }
                else
                {
                    return_result = new ResponseResult()
                    {
                        Status = "error",
                        BtnClass = "btn btn-danger",
                        Title = GeneralRes.ErrorTitle,
                        Message = SysModelRes.InvalidLoginData,
                        Close = false
                    };
                }

            }
            catch (Exception ex)
            {
                return_result = new ResponseResult()
                {
                    Status = "error",
                    BtnClass = "btn btn-danger",
                    Title = GeneralRes.ErrorTitle,
                    Message = ex.Message,
                    Close = false
                };
            }
            // If we got this far, something failed, redisplay form
            return Json(return_result);
        }



        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return View("login");
        }
        //public async Task<IActionResult> logout()
        //{
        //    await HttpContext.SignOutAsync();
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //    //return View("login");
        //}
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();

            // Clear any other authentication cookies as needed

            return RedirectToAction("Index", "Home");
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var cultureInfo = CultureInfo.GetCultureInfo("ar");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }







        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {

            var response = DoAuthenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }


        public AuthenticateResponse DoAuthenticate(AuthenticateRequest model)
        {
            var user = _userManager.FindByNameAsync(model.Username).Result;
            if (user == null)
            {
                return null;
            }
            var result = _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false).Result;



            // return null if user not found
            if (user == null || result.Succeeded == false)
            {

                return null;
            }
            else
            {
                List<string> roleIds = _userManager.GetRolesAsync(user).Result.ToList();
                if (roleIds.Count > 0)
                {
                    if (!roleIds.Contains("ApiRole"))
                    {
                        return null;
                    }
                    else
                    {
                        var token = generateJwtToken(user);

                        return new AuthenticateResponse(user, token);
                    }
                }

            }

            return null;

        }





        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
