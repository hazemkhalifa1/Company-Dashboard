using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.PL.Models
{
	public class EmployeeVM
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		[MinLength(3)]
		public string Name { get; set; }
		[Range(22, 60)]
		public int Age { get; set; }
		public string Address { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public string? ImageName { get; set; }
		public IFormFile? Image { get; set; }
		[DataType(DataType.Currency)]
		public double Salrey { get; set; }
		public bool IsActive { get; set; }
		[Phone]
		public string Phone { get; set; }
		public DateTime HireDate { get; set; }
		public Department? Department { get; set; }
		public int DepartmentId { get; set; }
	}
}
