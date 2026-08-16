namespace CustomerOrderManagement.Infrastructure.Migrations
{
    using CustomerOrderManagement.Domain.Entities;
    using CustomerOrderManagement.Infrastructure.Data.Contexts;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    internal sealed class Configuration : DbMigrationsConfiguration<CustomerOrderManagement.Infrastructure.Data.Contexts.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CustomerOrderManagement.Infrastructure.Data.Contexts.ApplicationDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
        }
        private void SeedRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("user"))
            {
                roleManager.Create(new IdentityRole("user"));
            }

            if (!roleManager.RoleExists("admin"))
            {
                roleManager.Create(new IdentityRole("admin"));
            }
        }
        private void SeedUsers(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            CreateUser(
                context,
                userManager,
                "Ahmed@gmail.com",
                "Ahmed@2002",
                "user");

            CreateUser(
                context,
                userManager,
                "Mustafa@gmail.com",
                "Mustafa@2000",
                "admin");
        }

        private void CreateUser(ApplicationDbContext context,UserManager<ApplicationUser> userManager,string email,string password,string role)
        {
            var existingUser = userManager.FindByEmail(email);

            if (existingUser != null)
            {
                return;
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Seed"
            };

            var result = userManager.Create(user, password);

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Failed to create seed user {email}: " +
                    string.Join(", ", result.Errors));
            }

            userManager.AddToRole(user.Id, role);
        }
    }
}
