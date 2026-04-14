using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4WebApp.Data;
using Task4WebApp.Models;

namespace Task4WebApp.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string name, string email, string password)
    {
        // Check if email already exists
        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            ViewBag.Error = "This email is already registered!";
            return View();
        }

        var user = new User 
        { 
            Name = name, 
            Email = email, 
            Password = password,
            Status = "Active",
            RegistrationTime = DateTime.UtcNow
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        
        if (user == null)
        {
            ViewBag.Error = "Invalid email or password.";
            return View();
        }
        
        if (user.Status == "Blocked")
        {
            ViewBag.Error = "Your account is blocked!";
            return View();
        }
        
        user.LastLoginTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.Id.ToString())
        };
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}