using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CommunityLibrary.MVC.Models;


public class LoanIndexViewModel
{
    public List<LoanSummaryViewModel> Loans { get; set; } = [];
    public string? StatusFilter { get; set; }
}

public class LoanSummaryViewModel
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = "";
    public string MemberName { get; set; } = "";
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public bool IsOverdue { get; set; }
}

// Create a loan

public class LoanCreateViewModel
{
    [Required(ErrorMessage = "Please select a member.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a member.")]
    [Display(Name = "Member")]
    public int MemberId { get; set; }

    [Required(ErrorMessage = "Please select a book.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a book.")]
    [Display(Name = "Book")]
    public int BookId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Loan Date")]
    public DateTime LoanDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Due Date")]
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);

    // Populated by controller
    public IEnumerable<SelectListItem> MemberOptions { get; set; } = [];
    public IEnumerable<SelectListItem> BookOptions { get; set; } = [];
}

// Details about the loan

public class LoanDetailsViewModel
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = "";
    public string BookAuthor { get; set; } = "";
    public int BookId { get; set; }
    public string MemberName { get; set; } = "";
    public int MemberId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public bool IsOverdue { get; set; }
}
