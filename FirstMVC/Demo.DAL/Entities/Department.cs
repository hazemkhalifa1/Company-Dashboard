﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department : BaseEntity
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
