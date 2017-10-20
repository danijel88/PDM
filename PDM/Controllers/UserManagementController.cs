using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDM.Data;
using PDM.Models;
using PDM.Models.AccountViewModels;
using PDM.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PDM.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserManagementController : Controller
    {
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IEmailSender _emailSender;

        public UserManagementController(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var vm = new UserManagementIndexViewModel
            {
                Users = _dbContext.Users.OrderBy(u => u.Email).ToList()
            };

            return View(vm);
        }

        public async Task<IActionResult> AddRole(string id)
        {
            var user = await GetUserById(id);
            var vm = new UserManagementAddRoleViewModel
            {
                Roles = GetAllRoles(),
                UserId = id,
                Email = user.Email
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromForm] UserManagementAddRoleViewModel userManagementAddRoleViewModel)
        {
            var user = await GetUserById(userManagementAddRoleViewModel.UserId);
            if (ModelState.IsValid)
            {

                var result = await _userManager.AddToRoleAsync(user, userManagementAddRoleViewModel.NewRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            userManagementAddRoleViewModel.Email = user.Email;
            userManagementAddRoleViewModel.Roles = GetAllRoles();
            return View(userManagementAddRoleViewModel);
        }

        public IActionResult Create()
        {
            UserManagementCreateViewModel rvm = new UserManagementCreateViewModel();
            rvm.Roles = GetAllRoles();
            return PartialView("_Create", rvm);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserManagementCreateViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { FirstName = rvm.FirstName, LastName = rvm.LastName, Email = rvm.Email, UserName = rvm.Email };
                var result = await _userManager.CreateAsync(user, rvm.Password);
                await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(rvm.Email), rvm.NewRole);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callBackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(rvm.Email, callBackUrl);
                }
            }
            rvm.Roles = GetAllRoles();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRole(string id)
        {
            var user = _dbContext.Users.Where(w => w.Email == id).SingleOrDefault();
            var roles = await _userManager.GetRolesAsync(user);
            var vm = new UserManagementRemoveRoleViewModel
            {
                Roles = roles,
                Email = id
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(UserManagementRemoveRoleViewModel rvm)
        {
            var user = _dbContext.Users.Where(w => w.Email == rvm.Email).SingleOrDefault();
            var role = await _roleManager.GetRoleNameAsync(new IdentityRole { Id = rvm.RoleId });
            await _userManager.RemoveFromRoleAsync(user, role);
            return RedirectToAction("Index");
        }

        private async Task<ApplicationUser> GetUserById(string id) =>
            await _userManager.FindByIdAsync(id);

        private SelectList GetAllRoles()
            => new SelectList(_roleManager.Roles.OrderBy(r => r.Name));
    }
}
