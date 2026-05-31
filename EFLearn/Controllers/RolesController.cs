using EFLearn.Models;
using EFLearn.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Text;

namespace EFLearn.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole>? _rolemanager;
        private readonly UserManager<IdentityUser>? _userManager;

        public RolesController(RoleManager<IdentityRole> RoleManagr, UserManager<IdentityUser> UserManager)
        {
            _rolemanager = RoleManagr;
            _userManager = UserManager;
        }
        public IActionResult Index()
        {
            var RoleUsers = new RoleUser
            {
             Roles = _rolemanager?.Roles.ToList(),
             Users = _userManager?.Users.OrderBy(n => n.Email).ToList()
            };
            return View(RoleUsers);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(string Role)
        {
            if (!string.IsNullOrEmpty(Role))
            {
                if (!await _rolemanager.RoleExistsAsync(Role))
                {
                    await _rolemanager.CreateAsync(new IdentityRole(Role));
                }
            }
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var role = await _rolemanager.FindByIdAsync(id);
                if (role != null)
                {
                    await _rolemanager.DeleteAsync(role);
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser()
        {
            var newuser = new UserModel
            {
                RoleItem = _rolemanager?.Roles
                .Select(p =>
                new SelectListItem
                {
                    Value = p.Name,
                    Text = p.Name
                }
                ).ToList()
            };
            return View(newuser);   
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel AddUser)
        {
            var UserCheck = await _userManager.FindByEmailAsync(AddUser.Email);
            if (UserCheck == null)
            {
                UserCheck = new IdentityUser
                {
                UserName = AddUser.Email,
                Email = AddUser.Email,
                EmailConfirmed = true
                };
            await _userManager.CreateAsync(UserCheck, AddUser.Password);
            await _userManager.AddToRoleAsync(UserCheck, AddUser.RoleSelect);
            }
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string UserEmail)
        {
            if (!string.IsNullOrEmpty(UserEmail))
            {
                var User = await _userManager.FindByEmailAsync(UserEmail);
                if (User != null)
                {
                    await _userManager.DeleteAsync(User);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ManageRoles(string UserEmail)
        {
            var RoleInfo = new RoleManage();
            RoleInfo.UserEmail = UserEmail;
             //ViewBag.Roles = _rolemanager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            var user =await _userManager.FindByEmailAsync(UserEmail);
            if (user != null)
            {
                RoleInfo.RolesList = _rolemanager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
                var role =await _userManager.GetRolesAsync(user);
                if (role != null)
                {
                    RoleInfo.UserRoles = role.ToList();
                }
                return View(RoleInfo);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddUserRole(string UserEmail, string RoleName)
        {
            var user = await _userManager.FindByEmailAsync(UserEmail);
            if (!await _userManager.IsInRoleAsync(user, RoleName))
            {
                await _userManager.AddToRoleAsync(user, RoleName);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserRole(string UserEmail, string RoleName)
        {
            var user = await _userManager.FindByEmailAsync(UserEmail);
            if (await _userManager.IsInRoleAsync(user, RoleName))
            {
                await _userManager.RemoveFromRoleAsync(user, RoleName);
            }
            return RedirectToAction("Index");
        }
    }
}