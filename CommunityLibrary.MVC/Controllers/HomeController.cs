using CommunityLibrary.MVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityLibrary.MVC.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;

        ViewBag.TotalBooks    = await context.Books.CountAsync();
        ViewBag.TotalMembers  = await context.Members.CountAsync();
        ViewBag.ActiveLoans   = await context.Loans.CountAsync(l => l.ReturnedDate == null);
        ViewBag.OverdueLoans  = await context.Loans.CountAsync(l => l.ReturnedDate == null && l.DueDate < today);

        return View();
    }
}
