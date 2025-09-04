using HRMSystem.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly HRMDbContext _context;

        public EmployeesController(HRMDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await (
                from e in _context.Employees
                join r in _context.Roles on e.RoleId equals r.RoleId into rg
                from role in rg.DefaultIfEmpty()   // LEFT JOIN: employees without role still included
                select new Employee
                {
                    Id = e.Id,
                    Name = e.Name,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.DepartmentName : null!,
                    Email = e.Email,
                    DateOfJoining = e.DateOfJoining,
                    RoleId = e.RoleId,
                    RoleName = role != null ? role.RoleName : null
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees
        .Include(e => e.Department)
        .Where(e => e.Id == id)
        .Select(e => new
        {
            e.Id,
            e.Name,
            e.DepartmentId,
            DepartmentName = e.Department!.DepartmentName, // 👈 Flattened property
            e.Email,
            e.DateOfJoining
        })
        .FirstOrDefaultAsync();

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("WithRoles")]
        public IActionResult getemployeewithrole()
        {
            var employee = from e in _context.Employees join r in _context.Roles on e.RoleId
                           equals r.RoleId into er from role in er.DefaultIfEmpty() //leftjoin
                           select new
                           {
                               e.Id,
                               e.Name,
                               rolename = role != null ? role.RoleName : null,
                           };
            return Ok(employee.ToList());
        }

        [HttpPut("{id}/assign-role")]
        public async Task<IActionResult> assignrole(int id, int roleId)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee == null)
                return NotFound();
            employee.RoleId = roleId; // assign new role
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
