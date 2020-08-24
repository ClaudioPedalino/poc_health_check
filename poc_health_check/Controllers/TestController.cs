using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using poc_health_check.Data;
using poc_health_check.Models;
using poc_health_check.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace poc_health_check.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DataContext context;
        public IConfiguration configuration { get; }

        public TestController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        //Test DB-Connection
        [HttpGet("employees")]
        public List<Employee> GetEmployeesAsync()
        {
            var service = new EmployeeService(context);
            return service.GetEmployees();
        }

        //Test Api-Service
        [HttpGet("books")]
        public async Task<object> GetBooksAsync()
        {
            var service = new BooksApiService(configuration);
            return await service.GetBooksAsync();
        }

        //TODO:
        ///Test 3er-Service
        ///Test Postgre

    }
}
