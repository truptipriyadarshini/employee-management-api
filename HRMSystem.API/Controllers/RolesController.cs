using HRMSystem.API.Migrations;
using HRMSystem.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly HRMDbContext _context;

        public RolesController(HRMDbContext context) {
           _context = context;        
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> getroles()
        {
            return await _context.Roles.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> getroles(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                NotFound();
            }
            return Ok(role);
        }
        [HttpPost]
        public async Task<ActionResult> postrole(Role roles)
        {
            await _context.Roles.AddAsync(roles);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(getroles), new { id = roles.RoleId }, roles);
        }
        [HttpPut]
        public async Task<ActionResult> putrole(int id,Role role)
        {
            if(id != role.RoleId)
            {
                NotFound();
            }
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(role);
        }
        [HttpDelete]
        public async Task<ActionResult> deleterole(int id)
        {
            var roles = await _context.Roles.FindAsync(id);
            if (roles == null)
                return NotFound();
            _context.Roles.Remove(roles);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
