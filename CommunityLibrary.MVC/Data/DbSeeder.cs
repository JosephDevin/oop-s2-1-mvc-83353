using Microsoft.AspNetCore.Identity;

namespace CommunityLibrary.MVC.Data;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // Seed Admin role
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        // Seed Admin user
        const string adminEmail = "admin@library.local";
        const string adminPassword = "Admin1234!";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(admin, adminPassword);
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");
    }
}
