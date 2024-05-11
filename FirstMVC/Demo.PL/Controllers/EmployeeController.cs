using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Demo.PL.Utitly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork _unitofwork;
		private readonly IMapper _mapper;

		public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitofwork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index(string SearchValue = "")
		{
			IEnumerable<Employee> employees;

			if (string.IsNullOrEmpty(SearchValue))
				employees = await _unitofwork.EmployeeRepository.GetAllAsync();
			else
				employees = await _unitofwork.EmployeeRepository.SearchAsync(e => e.Name == SearchValue);

			return View(_mapper.Map<IEnumerable<EmployeeVM>>(employees));
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.Departments = await _unitofwork.DepartmentRepository.GetAllAsync();
			return View(new EmployeeVM());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EmployeeVM employeeVM)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (employeeVM.Image is not null)
						employeeVM.ImageName = DocumentSetting.UploadFile(employeeVM.Image, "Images");
					await _unitofwork.EmployeeRepository.AddAsync(_mapper.Map<Employee>(employeeVM));
					await _unitofwork.CompleteAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}

			ViewBag.Departments = await _unitofwork.DepartmentRepository.GetAllAsync();
			return View(employeeVM);

		}

		public async Task<IActionResult> Details(int? id) => await GetEmlpoyee(id, nameof(Details));

		public async Task<IActionResult> Update(int? id)
		{
			ViewBag.Departments = await _unitofwork.DepartmentRepository.GetAllAsync();
			return await GetEmlpoyee(id, nameof(Update));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([FromRoute] int id, EmployeeVM employeeVM)
		{
			if (id != employeeVM.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					if (employeeVM.Image is not null)
					{
						if (!string.IsNullOrEmpty(employeeVM.ImageName))
							DocumentSetting.DeleteFile(employeeVM.ImageName, "Images");
						employeeVM.ImageName = DocumentSetting.UploadFile(employeeVM.Image, "Images");
					}

					_unitofwork.EmployeeRepository.Update(_mapper.Map<Employee>(employeeVM));
					await _unitofwork.CompleteAsync();
					return RedirectToAction(nameof(Index));

				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
				}
			}

			ViewBag.Departments = await _unitofwork.DepartmentRepository.GetAllAsync();
			return View(employeeVM);
		}

		public async Task<IActionResult> Delete(int? id) => await GetEmlpoyee(id, nameof(Delete));

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete([FromRoute] int id, EmployeeVM employeeVM)
		{
			if (id != employeeVM.Id)
				return BadRequest();
			try
			{
				_unitofwork.EmployeeRepository.Delete(_mapper.Map<Employee>(employeeVM));
				if (await _unitofwork.CompleteAsync() > 0 && employeeVM.ImageName is not null)
					DocumentSetting.DeleteFile(employeeVM.ImageName, "Images");
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(employeeVM);
		}

		public async Task<IActionResult> GetEmlpoyee(int? id, string viewName)
		{
			if (id == null)
				return BadRequest();
			var emp = await _unitofwork.EmployeeRepository.GetByIdAsync(id);
			var employeeVM = _mapper.Map<EmployeeVM>(emp);
			if (employeeVM == null)
				return NotFound();
			return View(viewName, employeeVM);
		}
	}
}
