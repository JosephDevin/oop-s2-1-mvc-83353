using CommunityLibrary.Domain;
using CommunityLibrary.MVC.Data;
using CommunityLibrary.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CommunityLibrary.MVC.Controllers;

public class BooksController(ApplicationDbContext context) : Controller
{
    // GET: Books
    public async Task<IActionResult> Index(string? searchTerm, string? categoryFilter, string? availabilityFilter)
    {
        IQueryable<Book> query = context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(b =>
                b.Title.Contains(searchTerm) ||
                b.Author.Contains(searchTerm));

        if (!string.IsNullOrWhiteSpace(categoryFilter))
            query = query.Where(b => b.Category == categoryFilter);

        if (availabilityFilter == "available")
            query = query.Where(b => b.IsAvailable);
        else if (availabilityFilter == "onloan")
            query = query.Where(b => !b.IsAvailable);

        query = query.OrderBy(b => b.Title);

        var books = await query
            .Select(b => new BookSummaryViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Category = b.Category,
                Isbn = b.Isbn,
                IsAvailable = b.IsAvailable
            })
            .ToListAsync();

        var categories = await context.Books
            .Select(b => b.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        var vm = new BookIndexViewModel
        {
            Books = books,
            SearchTerm = searchTerm,
            CategoryFilter = categoryFilter,
            AvailabilityFilter = availabilityFilter,
            CategoryOptions = categories.Select(c => new SelectListItem(c, c))
        };

        return View(vm);
    }

    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var book = await context.Books
            .Include(b => b.Loans)
                .ThenInclude(l => l.Member)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return NotFound();

        var vm = new BookDetailsViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Isbn = book.Isbn,
            Category = book.Category,
            IsAvailable = book.IsAvailable,
            RecentLoans = book.Loans
                .OrderByDescending(l => l.LoanDate)
                .Select(l => new BookLoanRowViewModel
                {
                    LoanId = l.Id,
                    MemberName = l.Member?.FullName ?? "",
                    LoanDate = l.LoanDate,
                    DueDate = l.DueDate,
                    ReturnedDate = l.ReturnedDate,
                    IsOverdue = l.IsOverdue
                })
                .ToList()
        };

        return View(vm);
    }

    // GET: Books/Create
    public IActionResult Create() => View(new BookFormViewModel());

    // POST: Books/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        context.Books.Add(new Book
        {
            Title = vm.Title,
            Author = vm.Author,
            Isbn = vm.Isbn,
            Category = vm.Category,
            IsAvailable = true
        });
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var book = await context.Books.FindAsync(id);
        if (book is null) return NotFound();

        return View(new BookFormViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Isbn = book.Isbn,
            Category = book.Category
        });
    }

    // POST: Books/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookFormViewModel vm)
    {
        if (id != vm.Id) return NotFound();
        if (!ModelState.IsValid) return View(vm);

        var book = await context.Books.FindAsync(id);
        if (book is null) return NotFound();

        book.Title = vm.Title;
        book.Author = vm.Author;
        book.Isbn = vm.Isbn;
        book.Category = vm.Category;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var book = await context.Books
            .Include(b => b.Loans)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return NotFound();

        return View(new BookDeleteViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            HasActiveLoans = book.Loans.Any(l => l.ReturnedDate == null)
        });
    }

    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hasActiveLoans = await context.Loans
            .AnyAsync(l => l.BookId == id && l.ReturnedDate == null);

        if (hasActiveLoans)
        {
            var book = await context.Books.FindAsync(id);
            ModelState.AddModelError(string.Empty,
                "This book is currently on loan and cannot be deleted.");
            return View(new BookDeleteViewModel
            {
                Id = id,
                Title = book?.Title ?? "",
                Author = book?.Author ?? "",
                HasActiveLoans = true
            });
        }

        var entity = await context.Books.FindAsync(id);
        if (entity is not null) context.Books.Remove(entity);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
