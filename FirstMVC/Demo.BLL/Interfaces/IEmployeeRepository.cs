using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Task<IEnumerable<Employee>> SearchAsync(Expression<Func<Employee, bool>> ex);
    }
}
