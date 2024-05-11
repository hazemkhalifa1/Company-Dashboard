using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                var users = await _userManager.Users.ToListAsync();
                var mappedUser = users.Select(user => new AppUserVM
                {
                    Id = user.Id,
                    LName = user.LName,
                    FName = user.FName,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result
                }).ToList();
                return View(mappedUser);
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var mappeduser = await MapToUserVMAsync(user);
                return View(new List<AppUserVM> { mappeduser });
            }
            return View(Enumerable.Empty<AppUserVM>);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            return View(viewName, await MapToUserVMAsync(user));
        }

        public async Task<IActionResult> Update(string id) => await Details(id, nameof(Update));

        [HttpPost]
        public async Task<IActionResult> Update(string id, AppUserVM model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null) return BadRequest();
            user.FName = model.FName;
            user.LName = model.LName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var item in result.Errors)
                ModelState.AddModelError("", item.Description);
            return View();
        }

        public async Task<IActionResult> Delete(string id) => await Details(id, nameof(Delete));

        [HttpPost]
        public async Task<IActionResult> Delete(AppUserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null) return BadRequest();
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }


        public async Task<AppUserVM> MapToUserVMAsync(AppUser user)
        {
            var userVM = _mapper.Map<AppUserVM>(user);
            userVM.Roles = await _userManager.GetRolesAsync(user);
            return userVM;
        }

    }
}
