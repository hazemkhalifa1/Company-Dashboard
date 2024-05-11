using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
	{
		private readonly IUnitOfWork _unitofwork;
		private readonly IMapper _mapper;
		private readonly IDepartmentRepository _repository;

		public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper, IDepartmentRepository repository)
		{
			_unitofwork = unitOfWork;
			_mapper = mapper;
			_repository = repository;
		}

		public async Task<IActionResult> Index() => View(_mapper.Map<IEnumerable<DepartmentVM>>(await _unitofwork.DepartmentRepository.GetAllAsync()));

		public IActionResult Create() => View(new DepartmentVM());

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(DepartmentVM departmentVM)
		{
			if (ModelState.IsValid)
			{
				try
				{
					await _unitofwork.DepartmentRepository.AddAsync(_mapper.Map<Department>(departmentVM));
					await _unitofwork.CompleteAsync();
					return RedirectToAction(nameof(Index));

				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}

			return View(departmentVM);
		}

		public async Task<IActionResult> Details(int? id) => await GetDepartment(id, nameof(Details));

		public async Task<IActionResult> Update(int? id) => await GetDepartment(id, nameof(Update));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([FromRoute] int id, DepartmentVM departmentVM)
		{
			if (id != departmentVM.Id)
				return BadRequest();
			try
			{
				_unitofwork.DepartmentRepository.Update(_mapper.Map<Department>(departmentVM));
				await _unitofwork.CompleteAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(departmentVM);
		}

		public async Task<IActionResult> Delete(int? id) => await GetDepartment(id, nameof(Delete));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete([FromRoute] int id, DepartmentVM departmentVM)
		{
			if (id != departmentVM.Id)
				return BadRequest();
			try
			{
				_unitofwork.DepartmentRepository.Delete(_mapper.Map<Department>(departmentVM));
				await _unitofwork.CompleteAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(departmentVM);
		}

		public async Task<IActionResult> GetDepartment(int? id, string ViewName)
		{
			if (id == null)
				return BadRequest();

			var departmentVM = _mapper.Map<DepartmentVM>(await _unitofwork.DepartmentRepository.GetByIdAsync(id));
			if (departmentVM == null)
				return NotFound();
			return View(ViewName, departmentVM);
		}
	}
}
