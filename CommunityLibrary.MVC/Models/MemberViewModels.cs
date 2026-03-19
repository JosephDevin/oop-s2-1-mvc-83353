using System.ComponentModel.DataAnnotations;

namespace CommunityLibrary.MVC.Models;


public class MemberSummaryViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public int ActiveLoanCount { get; set; }
}

// Create or edit

public class MemberFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full name is required.")]
    [MaxLength(150)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(200)]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = "";

    [MaxLength(20)]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    public string Phone { get; set; } = "";
}

// Details of member page

public class MemberDetailsViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public List<MemberLoanRowViewModel> Loans { get; set; } = [];
}

public class MemberLoanRowViewModel
{
    public int LoanId { get; set; }
    public string BookTitle { get; set; } = "";
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public bool IsOverdue { get; set; }
}

// Delete a member

public class MemberDeleteViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public int ActiveLoanCount { get; set; }
}
