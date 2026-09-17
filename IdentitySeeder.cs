using Microsoft.AspNetCore.Identity;

namespace ShashaStyle.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminAsync(
            IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<
                    UserManager<IdentityUser>>();

            var adminEmail =
                "admin@shashastyle.co.za";

            var adminPassword =
                "ShashaAdmin@2026";

            var existingAdmin =
                await userManager.FindByEmailAsync(
                    adminEmail);

            if (existingAdmin == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result =
                    await userManager.CreateAsync(
                        admin,
                        adminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Unable to create ShashaStyle administrator: "
                        + string.Join(
                            ", ",
                            result.Errors.Select(e =>
                                e.Description)));
                }
            }
        }
    }
}