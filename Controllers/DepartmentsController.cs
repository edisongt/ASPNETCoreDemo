using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCore5Demo.Models;

namespace ASPNETCore5Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;
        private readonly ContosoUniversityContextProcedures sp;

        public DepartmentsController(ContosoUniversityContext context, ContosoUniversityContextProcedures sp)
        {
            this.sp = sp;
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            return await _context.Department.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="id"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }



            //_context.Entry(department).State = EntityState.Modified;

            try
            {
                var test = _context.Department.Find(id);
                await sp.Department_Update(id, department.Name, department.Budget, test.StartDate, test.InstructorId, test.RowVersion, null);
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {

            await sp.Department_Insert(department.Name, department.Budget, department.StartDate, department.InstructorId, null);
            //_context.Department.Add(department);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);
        }

        // DELETE: api/Departments/5
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            await sp.Department_Delete(id, department.RowVersion, null);
            //_context.Department.Remove(department);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id);
        }
    }
}
