using Bogus;
using CommunityLibrary.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityLibrary.MVC.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Book → Loans
        builder.Entity<Book>()
            .HasMany(b => b.Loans)
            .WithOne(l => l.Book)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // Member → Loans
        builder.Entity<Member>()
            .HasMany(m => m.Loans)
            .WithOne(l => l.Member)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        SeedData(builder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        // Deterministic seed so migrations are stable
        var bookFaker = new Faker<Book>()
            .UseSeed(1234)
            .RuleFor(b => b.Id, f => f.IndexFaker + 1)
            .RuleFor(b => b.Title, f => f.Commerce.ProductName())
            .RuleFor(b => b.Author, f => f.Name.FullName())
            .RuleFor(b => b.Isbn, f => f.Commerce.Ean13())
            .RuleFor(b => b.Category, f => f.PickRandom(
                "Fiction", "Non-Fiction", "Science", "History",
                "Biography", "Mystery", "Fantasy", "Technology"))
            .RuleFor(b => b.IsAvailable, _ => true);

        var books = bookFaker.Generate(20);

        var memberFaker = new Faker<Member>()
            .UseSeed(5678)
            .RuleFor(m => m.Id, f => f.IndexFaker + 1)
            .RuleFor(m => m.FullName, f => f.Name.FullName())
            .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.Phone, f => f.Phone.PhoneNumber("07### ######"));

        var members = memberFaker.Generate(10);

        // Build 15 loans manually for deterministic seeding
        var today = new DateTime(2025, 6, 1);
        var loans = new List<Loan>
        {
            // Returned loans
            new() { Id = 1, BookId = 1, MemberId = 1, LoanDate = today.AddDays(-30), DueDate = today.AddDays(-16), ReturnedDate = today.AddDays(-18) },
            new() { Id = 2, BookId = 2, MemberId = 2, LoanDate = today.AddDays(-25), DueDate = today.AddDays(-11), ReturnedDate = today.AddDays(-12) },
            new() { Id = 3, BookId = 3, MemberId = 3, LoanDate = today.AddDays(-20), DueDate = today.AddDays(-6),  ReturnedDate = today.AddDays(-7)  },
            new() { Id = 4, BookId = 4, MemberId = 4, LoanDate = today.AddDays(-40), DueDate = today.AddDays(-26), ReturnedDate = today.AddDays(-28) },
            new() { Id = 5, BookId = 5, MemberId = 5, LoanDate = today.AddDays(-15), DueDate = today.AddDays(-1),  ReturnedDate = today.AddDays(-2)  },

            // Active loans (not overdue)
            new() { Id = 6,  BookId = 6,  MemberId = 6,  LoanDate = today.AddDays(-5),  DueDate = today.AddDays(9),  ReturnedDate = null },
            new() { Id = 7,  BookId = 7,  MemberId = 7,  LoanDate = today.AddDays(-3),  DueDate = today.AddDays(11), ReturnedDate = null },
            new() { Id = 8,  BookId = 8,  MemberId = 8,  LoanDate = today.AddDays(-2),  DueDate = today.AddDays(12), ReturnedDate = null },
            new() { Id = 9,  BookId = 9,  MemberId = 9,  LoanDate = today.AddDays(-7),  DueDate = today.AddDays(7),  ReturnedDate = null },
            new() { Id = 10, BookId = 10, MemberId = 10, LoanDate = today.AddDays(-1),  DueDate = today.AddDays(13), ReturnedDate = null },

            // Overdue loans (active but past due date)
            new() { Id = 11, BookId = 11, MemberId = 1,  LoanDate = today.AddDays(-30), DueDate = today.AddDays(-16), ReturnedDate = null },
            new() { Id = 12, BookId = 12, MemberId = 2,  LoanDate = today.AddDays(-25), DueDate = today.AddDays(-11), ReturnedDate = null },
            new() { Id = 13, BookId = 13, MemberId = 3,  LoanDate = today.AddDays(-20), DueDate = today.AddDays(-6),  ReturnedDate = null },
            new() { Id = 14, BookId = 14, MemberId = 4,  LoanDate = today.AddDays(-21), DueDate = today.AddDays(-7),  ReturnedDate = null },
            new() { Id = 15, BookId = 15, MemberId = 5,  LoanDate = today.AddDays(-18), DueDate = today.AddDays(-4),  ReturnedDate = null },
        };

        // Mark books on active loans as unavailable
        var activeLoanBookIds = loans
            .Where(l => l.ReturnedDate == null)
            .Select(l => l.BookId)
            .ToHashSet();

        foreach (var book in books)
            book.IsAvailable = !activeLoanBookIds.Contains(book.Id);

        builder.Entity<Book>().HasData(books);
        builder.Entity<Member>().HasData(members);
        builder.Entity<Loan>().HasData(loans);
    }
}
