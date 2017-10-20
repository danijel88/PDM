using Microsoft.AspNetCore.Identity;
using PDM.Models;
using System.Threading.Tasks;

namespace PDM.Data
{
    public class DbInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            _context.Database.EnsureCreated();
            if ((await _roleManager.FindByNameAsync("Administrator") == null))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Administrator" });
            }
            if ((await _roleManager.FindByNameAsync("Manager") == null))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Manager" });
            }
            if ((await _roleManager.FindByNameAsync("Operator") == null))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Operator" });
            }
            if ((await _roleManager.FindByNameAsync("Member") == null))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            }

            //creating the default Admin account
            string user = "danijel.boksan@fiorano.rs";
            string password = "Danijel88!";

            await _userManager.CreateAsync(new ApplicationUser { FirstName = "Danijel", LastName = "Boksan", Company = "Fiorano doo", Email = user, UserName = user }, password);
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user), "Administrator");
        }
    }
}
