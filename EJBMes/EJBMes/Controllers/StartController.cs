using Microsoft.AspNetCore.Mvc;
using EJBMes.Models;
using EJBMes.Resources;
using EJBMes.Services.Contract;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace EJBMes.Controllers
{
    public class StartController : Controller
    {
        private readonly IUserService _userService;

        public StartController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserMes model)
        {
            if (model != null)
            {
                model.Password = Utilities.EncriptKey(model.Password);

                UserMes userCreated = await _userService.SaveUser(model);

                if (userCreated.UserName != "")
                {
                    return RedirectToAction("StartSession", "Start");
                }
            }
            ViewData["message"] = "User can not created.";
            return View();
        }

        public IActionResult StartSession()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartSession(string userID, string Pwd)
        {
            UserMes userFound = await _userService.GetUser(userID, Utilities.EncriptKey(Pwd));

            if (userFound == null)
            {
                ViewData["message"] = "User not found.";
                return View();
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userFound.UserName),
                new Claim(ClaimTypes.UserData, userFound.EmployeeId, "EmployeeId"),
                new Claim(ClaimTypes.UserData, userFound.Site, "Site"),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );
            return RedirectToAction("Index", "Home");
        }
    }
}
