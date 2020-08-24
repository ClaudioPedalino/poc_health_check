using poc_health_check.Data;
using poc_health_check.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc_health_check.Services
{
    public class EmployeeService
    {
        private readonly DataContext dataContext;

        public EmployeeService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public List<Employee> GetEmployees() 
            => dataContext.Employees.ToList();
    }
}
