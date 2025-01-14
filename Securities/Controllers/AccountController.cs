using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Securities.Data;
using Securities.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Securities.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataDbContext _context;

        public AccountController(DataDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, user.Status == 0 ? "Admin" : "User")
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (user.Status == 0)
                        return RedirectToAction("Index", "Admin");
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password");
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
