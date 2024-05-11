using AutoMapper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var rolesVM = _mapper.Map<List<RoleVM>>(roles);
                return View(rolesVM);
            }
            var role = await _roleManager.FindByNameAsync(name);
            if (role is not null)
            {
                var roleVM = _mapper.Map<RoleVM>(role);
                return View(new List<RoleVM> { roleVM });
            }
            return View(Enumerable.Empty<AppUserVM>);
        }

        public IActionResult Create() => View(_mapper.Map<RoleVM>(new IdentityRole()));

        [HttpPost]
        public async Task<IActionResult> Create(RoleVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _roleManager.CreateAsync(_mapper.Map<IdentityRole>(model));
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var item in result.Errors)
                ModelState.AddModelError("", item.Description);
            return View();
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            return View(viewName, _mapper.Map<RoleVM>(role));
        }

        public async Task<IActionResult> Update(string id) => await Details(id, nameof(Update));

        [HttpPost]
        public async Task<IActionResult> Update(string id, RoleVM model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role is null) return BadRequest();
            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var item in result.Errors)
                ModelState.AddModelError("", item.Description);
            return View();
        }

        public async Task<IActionResult> Delete(string id) => await Details(id, nameof(Delete));

        [HttpPost]
        public async Task<IActionResult> Delete(RoleVM model)
        {
            var user = await _roleManager.FindByIdAsync(model.Id);
            if (user is null) return BadRequest();
            await _roleManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

    }
}
