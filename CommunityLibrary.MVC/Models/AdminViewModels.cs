using System.ComponentModel.DataAnnotations;

namespace CommunityLibrary.MVC.Models;

public class RoleIndexViewModel
{
    public List<RoleRowViewModel> Roles { get; set; } = [];
    public RoleCreateViewModel NewRole { get; set; } = new();
}

public class RoleRowViewModel
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}

public class RoleCreateViewModel
{
    [Required(ErrorMessage = "Role name is required.")]
    [MaxLength(50)]
    [Display(Name = "Role Name")]
    public string Name { get; set; } = "";
}
