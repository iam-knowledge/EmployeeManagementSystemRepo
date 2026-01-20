using Employee_Management_System.Models;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Employee_Management_System.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Employee_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger)
        {
            _dbHelper = new DatabaseHelper(configuration);
            _logger = logger;
        }

        // GET: Account/Login - This is the first page presented
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_dbHelper.ValidateUser(model.Username, model.Password))
                    {
                        _dbHelper.UpdateLastLogin(model.Username);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(ClaimTypes.NameIdentifier, model.Username)
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        _logger.LogInformation("User {Username} logged in.", model.Username);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Login failed for user {Username}", model.Username);
                    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                }
            }
            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_dbHelper.UserExists(model.Username))
                    {
                        ModelState.AddModelError("Username", "Username already exists.");
                        return View(model);
                    }

                    if (_dbHelper.RegisterUser(model.Username, model.Email, model.Password))
                    {
                        // Auto-login after registration
                        var loginModel = new LoginModel
                        {
                            Username = model.Username,
                            Password = model.Password
                        };

                        return await Login(loginModel);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Registration failed.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Registration failed for user {Username}", model.Username);
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}