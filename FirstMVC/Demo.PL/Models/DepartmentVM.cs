using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(10, 100)]
        public int Code { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
