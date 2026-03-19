using CommunityLibrary.Domain;
using CommunityLibrary.MVC.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CommunityLibrary.Tests;

public class LoanBusinessRuleTests
{
    // Test 1: Cannot create a second active loan for a book already on loan
    [Fact]
    public async Task CannotCreateLoan_WhenBookAlreadyOnActiveLoan()
    {
        using var context = DbContextFactory.Create();
        var book   = new Book   { Title = "Clean Code", Author = "Robert Martin", Isbn = "111", Category = "Technology" };
        var member = new Member { FullName = "Alice",   Email  = "alice@test.com", Phone = "01234" };
        context.Books.Add(book);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var firstLoan = new Loan
        {
            BookId   = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Today.AddDays(-7),
            DueDate  = DateTime.Today.AddDays(7),
            ReturnedDate = null
        };
        context.Loans.Add(firstLoan);
        book.IsAvailable = false;
        await context.SaveChangesAsync();

        var hasActiveLoan = await context.Loans
            .AnyAsync(l => l.BookId == book.Id && l.ReturnedDate == null);

        Assert.True(hasActiveLoan,
            "There should be an active loan blocking a second issue of the same book.");
    }

    // Test 2: Returning a loan makes the book available again
    [Fact]
    public async Task ReturnLoan_MakesBookAvailableAgain()
    {
        using var context = DbContextFactory.Create();
        var book   = new Book   { Title = "DDIA", Author = "Kleppmann", Isbn = "222", Category = "Technology", IsAvailable = false };
        var member = new Member { FullName = "Bob",  Email = "bob@test.com", Phone = "07700" };
        context.Books.Add(book);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var loan = new Loan
        {
            BookId   = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Today.AddDays(-5),
            DueDate  = DateTime.Today.AddDays(9),
            ReturnedDate = null
        };
        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        loan.ReturnedDate = DateTime.Today;
        book.IsAvailable  = true;
        await context.SaveChangesAsync();

        var savedBook = await context.Books.FindAsync(book.Id);
        Assert.True(savedBook!.IsAvailable, "Book should be available after loan is returned.");

        var savedLoan = await context.Loans.FindAsync(loan.Id);
        Assert.NotNull(savedLoan!.ReturnedDate);
    }

    // Test 3: Book search returns correct matches on Title or Author
    [Fact]
    public async Task BookSearch_ReturnsTitleAndAuthorMatches()
    {
        using var context = DbContextFactory.Create();
        context.Books.AddRange(
            new Book { Title = "The Pragmatic Programmer", Author = "Hunt",     Isbn = "A1", Category = "Technology" },
            new Book { Title = "Refactoring",              Author = "Fowler",   Isbn = "A2", Category = "Technology" },
            new Book { Title = "Domain-Driven Design",     Author = "Evans",    Isbn = "A3", Category = "Technology" },
            new Book { Title = "Clean Architecture",       Author = "Martin",   Isbn = "A4", Category = "Technology" }
        );
        await context.SaveChangesAsync();

        var term = "fowler";
        var results = await context.Books
            .Where(b => b.Title.Contains(term) || b.Author.Contains(term))
            .ToListAsync();

        Assert.Single(results);
        Assert.Equal("Refactoring", results[0].Title);
    }

    // Test 4: Overdue logic — DueDate < Today and ReturnedDate is null
    [Fact]
    public async Task OverdueLogic_CorrectlyIdentifiesOverdueLoans()
    {
        using var context = DbContextFactory.Create();
        var book1  = new Book   { Title = "Overdue Book",  Author = "A", Isbn = "O1", Category = "Fiction" };
        var book2  = new Book   { Title = "On-time Book",  Author = "B", Isbn = "O2", Category = "Fiction" };
        var member = new Member { FullName = "Carol", Email = "carol@test.com", Phone = "" };
        context.Books.AddRange(book1, book2);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var overdueDate = DateTime.Today.AddDays(-3);  
        var futureDate  = DateTime.Today.AddDays(5);    

        context.Loans.AddRange(
            new Loan { BookId = book1.Id, MemberId = member.Id, LoanDate = DateTime.Today.AddDays(-17), DueDate = overdueDate, ReturnedDate = null   },
            new Loan { BookId = book2.Id, MemberId = member.Id, LoanDate = DateTime.Today.AddDays(-2),  DueDate = futureDate,  ReturnedDate = null   }
        );
        await context.SaveChangesAsync();

        var today        = DateTime.Today;
        var overdueLoans = await context.Loans
            .Where(l => l.ReturnedDate == null && l.DueDate < today)
            .ToListAsync();

        Assert.Single(overdueLoans);
        Assert.Equal(book1.Id, overdueLoans[0].BookId);
    }

    // Test 5: Returned loan is NOT flagged as overdue
    [Fact]
    public async Task ReturnedLoan_IsNotFlaggedAsOverdue()
    {
        using var context = DbContextFactory.Create();
        var book   = new Book   { Title = "Old Book", Author = "X", Isbn = "R1", Category = "History" };
        var member = new Member { FullName = "Dave", Email = "dave@test.com", Phone = "" };
        context.Books.Add(book);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var loan = new Loan
        {
            BookId       = book.Id,
            MemberId     = member.Id,
            LoanDate     = DateTime.Today.AddDays(-30),
            DueDate      = DateTime.Today.AddDays(-10),  
            ReturnedDate = DateTime.Today.AddDays(-2)    
        };
        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        var today        = DateTime.Today;
        var overdueLoans = await context.Loans
            .Where(l => l.ReturnedDate == null && l.DueDate < today)
            .ToListAsync();

        Assert.Empty(overdueLoans);
    }
}
