using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4WebApp.Data;

namespace Task4WebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.OrderByDescending(u => u.LastLoginTime).ToListAsync();
        return View(users);
    }
}