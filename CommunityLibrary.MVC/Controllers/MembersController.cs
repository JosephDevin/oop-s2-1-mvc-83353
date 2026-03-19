using CommunityLibrary.Domain;
using CommunityLibrary.MVC.Data;
using CommunityLibrary.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityLibrary.MVC.Controllers;

public class MembersController(ApplicationDbContext context) : Controller
{
    // GET: Members
    public async Task<IActionResult> Index()
    {
        var members = await context.Members
            .OrderBy(m => m.FullName)
            .Select(m => new MemberSummaryViewModel
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                ActiveLoanCount = m.Loans.Count(l => l.ReturnedDate == null)
            })
            .ToListAsync();

        return View(members);
    }

    // GET: Members/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var member = await context.Members
            .Include(m => m.Loans)
                .ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (member is null) return NotFound();

        var vm = new MemberDetailsViewModel
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            Phone = member.Phone,
            Loans = member.Loans
                .OrderByDescending(l => l.LoanDate)
                .Select(l => new MemberLoanRowViewModel
                {
                    LoanId = l.Id,
                    BookTitle = l.Book?.Title ?? "",
                    LoanDate = l.LoanDate,
                    DueDate = l.DueDate,
                    ReturnedDate = l.ReturnedDate,
                    IsOverdue = l.IsOverdue
                })
                .ToList()
        };

        return View(vm);
    }

    // GET: Members/Create
    public IActionResult Create() => View(new MemberFormViewModel());

    // POST: Members/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        context.Members.Add(new Member
        {
            FullName = vm.FullName,
            Email = vm.Email,
            Phone = vm.Phone
        });
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Members/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var member = await context.Members.FindAsync(id);
        if (member is null) return NotFound();

        return View(new MemberFormViewModel
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            Phone = member.Phone
        });
    }

    // POST: Members/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MemberFormViewModel vm)
    {
        if (id != vm.Id) return NotFound();
        if (!ModelState.IsValid) return View(vm);

        var member = await context.Members.FindAsync(id);
        if (member is null) return NotFound();

        member.FullName = vm.FullName;
        member.Email = vm.Email;
        member.Phone = vm.Phone;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    // GET: Members/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var member = await context.Members
            .Include(m => m.Loans)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (member is null) return NotFound();

        return View(new MemberDeleteViewModel
        {
            Id = member.Id,
            FullName = member.FullName,
            ActiveLoanCount = member.Loans.Count(l => l.ReturnedDate == null)
        });
    }

    // POST: Members/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var activeLoans = await context.Loans
            .CountAsync(l => l.MemberId == id && l.ReturnedDate == null);

        if (activeLoans > 0)
        {
            var member = await context.Members.FindAsync(id);
            ModelState.AddModelError(string.Empty,
                "This member has active loans and cannot be deleted.");
            return View(new MemberDeleteViewModel
            {
                Id = id,
                FullName = member?.FullName ?? "",
                ActiveLoanCount = activeLoans
            });
        }

        var entity = await context.Members.FindAsync(id);
        if (entity is not null) context.Members.Remove(entity);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
