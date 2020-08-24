using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc_health_check.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
    }
}
