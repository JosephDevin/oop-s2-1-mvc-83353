using CommunityLibrary.Domain;
using CommunityLibrary.MVC.Data;
using CommunityLibrary.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CommunityLibrary.MVC.Controllers;

public class LoansController(ApplicationDbContext context) : Controller
{
    // GET: Loans
    public async Task<IActionResult> Index(string? statusFilter)
    {
        IQueryable<Loan> query = context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member);

        query = statusFilter switch
        {
            "active"   => query.Where(l => l.ReturnedDate == null),
            "returned" => query.Where(l => l.ReturnedDate != null),
            "overdue"  => query.Where(l => l.ReturnedDate == null && l.DueDate < DateTime.Today),
            _          => query
        };

        var today = DateTime.Today;

        var loans = await query
            .OrderByDescending(l => l.LoanDate)
            .Select(l => new LoanSummaryViewModel
            {
                Id = l.Id,
                BookTitle = l.Book!.Title,
                MemberName = l.Member!.FullName,
                LoanDate = l.LoanDate,
                DueDate = l.DueDate,
                ReturnedDate = l.ReturnedDate,
                IsOverdue = l.ReturnedDate == null && l.DueDate < today
            })
            .ToListAsync();

        return View(new LoanIndexViewModel { Loans = loans, StatusFilter = statusFilter });
    }

    // GET: Loans/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var loan = await context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (loan is null) return NotFound();

        var vm = new LoanDetailsViewModel
        {
            Id = loan.Id,
            BookTitle = loan.Book?.Title ?? "",
            BookAuthor = loan.Book?.Author ?? "",
            BookId = loan.BookId,
            MemberName = loan.Member?.FullName ?? "",
            MemberId = loan.MemberId,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnedDate = loan.ReturnedDate,
            IsOverdue = loan.IsOverdue
        };

        return View(vm);
    }

    // GET: Loans/Create
    public async Task<IActionResult> Create()
    {
        return View(await BuildCreateViewModelAsync());
    }

    // POST: Loans/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LoanCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var fresh = await BuildCreateViewModelAsync();
            vm.MemberOptions = fresh.MemberOptions;
            vm.BookOptions = fresh.BookOptions;
            return View(vm);
        }

        // Book must not already be on an active loan
        var hasActiveLoan = await context.Loans
            .AnyAsync(l => l.BookId == vm.BookId && l.ReturnedDate == null);

        if (hasActiveLoan)
        {
            ModelState.AddModelError(nameof(vm.BookId),
                "This book is already on an active loan. Please choose another.");
            var fresh = await BuildCreateViewModelAsync();
            vm.MemberOptions = fresh.MemberOptions;
            vm.BookOptions = fresh.BookOptions;
            return View(vm);
        }

        // DueDate must be after LoanDate
        if (vm.DueDate <= vm.LoanDate)
        {
            ModelState.AddModelError(nameof(vm.DueDate), "Due date must be after the loan date.");
            var fresh = await BuildCreateViewModelAsync();
            vm.MemberOptions = fresh.MemberOptions;
            vm.BookOptions = fresh.BookOptions;
            return View(vm);
        }

        // Create the loan and mark book unavailable
        context.Loans.Add(new Loan
        {
            BookId = vm.BookId,
            MemberId = vm.MemberId,
            LoanDate = vm.LoanDate,
            DueDate = vm.DueDate,
            ReturnedDate = null
        });

        var book = await context.Books.FindAsync(vm.BookId);
        if (book is not null) book.IsAvailable = false;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: Loans/MarkReturned/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkReturned(int id)
    {
        var loan = await context.Loans
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (loan is null) return NotFound();
        if (loan.ReturnedDate != null) return RedirectToAction(nameof(Details), new { id });

        loan.ReturnedDate = DateTime.Today;

        // Mark the book available again
        if (loan.Book is not null) loan.Book.IsAvailable = true;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }
    
    private async Task<LoanCreateViewModel> BuildCreateViewModelAsync()
    {
        var members = await context.Members
            .OrderBy(m => m.FullName)
            .Select(m => new SelectListItem(m.FullName, m.Id.ToString()))
            .ToListAsync();

        var books = await context.Books
            .Where(b => b.IsAvailable)
            .OrderBy(b => b.Title)
            .Select(b => new SelectListItem($"{b.Title} — {b.Author}", b.Id.ToString()))
            .ToListAsync();

        return new LoanCreateViewModel
        {
            MemberOptions = members,
            BookOptions = books
        };
    }
}
