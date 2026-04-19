using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4WebApp.Data;

namespace Task4WebApp.Controllers;

[Authorize]
public class UsersController(ApplicationDbContext context) : Controller
{
    [HttpPost]
    public async Task<IActionResult> BulkAction(List<int>? ids, string action)
    {
        if (ids == null || ids.Count == 0)
        {
            return RedirectToAction("Index", "Home");
        }

        var users = await context.Users
            .Where(u => ids.Contains(u.Id))
            .ToListAsync();

        foreach (var user in users)
        {
            switch (action)
            {
                case "block":
                    user.Status = "Blocked";
                    break;
                case "unblock":
                    user.Status = "Active";
                    break;
                case "delete":
                    context.Users.Remove(user);
                    break;
            }
        }

        await context.SaveChangesAsync();
        
        return RedirectToAction("Index", "Home");
    }
}