using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CommunityLibrary.MVC.Models;

// Search
public class BookIndexViewModel
{
    public List<BookSummaryViewModel> Books { get; set; } = [];
    public string? SearchTerm { get; set; }
    public string? CategoryFilter { get; set; }
    public string? AvailabilityFilter { get; set; }
    public IEnumerable<SelectListItem> CategoryOptions { get; set; } = [];
}

public class BookSummaryViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Category { get; set; } = "";
    public string Isbn { get; set; } = "";
    public bool IsAvailable { get; set; }
}

// Create and edit

public class BookFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200)]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Author is required.")]
    [MaxLength(150)]
    public string Author { get; set; } = "";

    [Required(ErrorMessage = "ISBN is required.")]
    [MaxLength(20)]
    [Display(Name = "ISBN")]
    public string Isbn { get; set; } = "";

    [Required(ErrorMessage = "Category is required.")]
    [MaxLength(100)]
    public string Category { get; set; } = "";
}

// Book details

public class BookDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Isbn { get; set; } = "";
    public string Category { get; set; } = "";
    public bool IsAvailable { get; set; }
    public List<BookLoanRowViewModel> RecentLoans { get; set; } = [];
}

public class BookLoanRowViewModel
{
    public int LoanId { get; set; }
    public string MemberName { get; set; } = "";
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public bool IsOverdue { get; set; }
}

// Delete book

public class BookDeleteViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public bool HasActiveLoans { get; set; }
}
