using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4WebApp.Data;

namespace Task4WebApp.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    public UsersController(ApplicationDbContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> BulkAction(List<int> ids, string action)
    {
        var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();

        foreach (var user in users)
        {
            if (action == "block") user.Status = "Blocked";
            else if (action == "unblock") user.Status = "Active";
            else if (action == "delete") _context.Users.Remove(user);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}