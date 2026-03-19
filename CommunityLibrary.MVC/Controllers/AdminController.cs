using CommunityLibrary.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityLibrary.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(RoleManager<IdentityRole> roleManager) : Controller
{
    // GET: /Admin/Roles
    public async Task<IActionResult> Roles()
    {
        var roles = await roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => new RoleRowViewModel
            {
                Id = r.Id,
                Name = r.Name ?? ""
            })
            .ToListAsync();

        var vm = new RoleIndexViewModel
        {
            Roles = roles,
            NewRole = new RoleCreateViewModel()
        };

        return View(vm);
    }

    // POST: /Admin/Roles/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRole(RoleIndexViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Roles = await roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => new RoleRowViewModel { Id = r.Id, Name = r.Name ?? "" })
                .ToListAsync();
            return View("Roles", vm);
        }

        var roleName = vm.NewRole.Name.Trim();

        if (await roleManager.RoleExistsAsync(roleName))
        {
            ModelState.AddModelError("NewRole.Name", $"Role '{roleName}' already exists.");
            vm.Roles = await roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => new RoleRowViewModel { Id = r.Id, Name = r.Name ?? "" })
                .ToListAsync();
            return View("Roles", vm);
        }

        await roleManager.CreateAsync(new IdentityRole(roleName));
        TempData["Success"] = $"Role '{roleName}' created successfully.";
        return RedirectToAction(nameof(Roles));
    }

    // POST: /Admin/Roles/Delete
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is null) return NotFound();

        // Prevent deleting the Admin role
        if (role.Name == "Admin")
        {
            TempData["Error"] = "The Admin role cannot be deleted.";
            return RedirectToAction(nameof(Roles));
        }

        await roleManager.DeleteAsync(role);
        TempData["Success"] = $"Role '{role.Name}' deleted.";
        return RedirectToAction(nameof(Roles));
    }
}
