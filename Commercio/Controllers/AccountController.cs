using Commercio.Data;
using Commercio.Models;
using Commercio.ServiceModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Commercio.Enums;

namespace Commercio.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Login()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var user = await _context.Users.Where(c => c.Email == request.Email && c.UserStatusId == (int)UserStatusEnum.Active).FirstOrDefaultAsync();
            if (user is null)
            {
                ModelState.AddModelError(" ", "Email or password is not correct");
                return View(request);

            }
            var result = user.CheckPassword(request.Password);
            if (!result)
            {
                ModelState.AddModelError(" ", "Email or password is not correct");
                return View(request);

            }
            var claims = new List<Claim>
    {
        new Claim("Name", user.Name),
        new Claim("Surname", user.Surname),
        new Claim("Email", user.Email),
        new Claim("Id", user.Id.ToString()),
         new Claim("RoleId", user.UserRoleId.ToString()),
    };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
		{

			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel request)
        {

            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (!ModelState.IsValid)
                {
                    return View(request);
                }
                var user = await _context.Users.Where(c => c.Email == request.Email).FirstOrDefaultAsync();
                if(user is not null) 
                {
                    ModelState.AddModelError("", "There is already user like this");
                    return View(request);
                }


					user=new User(request.Name,
                                    request.Surname,
                                    request.Email);

                user.IP = ip ?? "::00";
                user.AddPassword(request.Password);
                user.AddUserRole();

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Internal", "Error");
            }           
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();

        }

        [HttpPost]
		public async Task<IActionResult> ForgotPassword([FromBody]string email)
		{
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email is not correct");

                return View(email); 
			}

            var user= await _context.Users.Where(c => c.Email == email && c.UserStatusId==(int)UserStatusEnum.Active).FirstOrDefaultAsync();
            if (user is null)
            {
                ModelState.AddModelError("", "There is not any user like that");
             return View(email);

            }

            var otp=GenerateOTP();
            HttpContext.Session.SetString("otp", otp);

            return View();
			

		}

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();
        }

    }
}
