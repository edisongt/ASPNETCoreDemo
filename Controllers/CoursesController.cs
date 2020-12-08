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
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course.Where(x => x.IsDeleted == false).ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        [HttpGet("credits/{credit}")]
        public ActionResult<IEnumerable<Course>> GetCourseByCredit(int credit)
        {
            return _context.Course.Where(x => x.Credits == credit).ToList();
        }


        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            course.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("abc/{id}")]
        public ActionResult<IEnumerable<Course>> GetDepartmentCourses(int id)
        {
            //return _context.Department.Include(p => p.Course)
            //    .First(p => p.DepartmentId == id).Course.ToList();

            return _context.Department.Where(p => p.DepartmentId == id).Include(p => p.Course)
             .SelectMany(x => x.Course).ToList();
        }

        [HttpGet("vwCourseStudentCount")]
        public ActionResult<IEnumerable<VwCourseStudentCount>> GetCourseStudentCount()
        {
            return _context.VwCourseStudentCount;
        }



        [HttpGet("vwCourseStudents")]
        public ActionResult<IEnumerable<VwCourseStudents>> GetVwCourseStudents()
        {
            return _context.VwCourseStudents;
        }

        [HttpGet("vwDepartmentCourseCount")]
        public ActionResult<IEnumerable<VwDepartmentCourseCount>> GetvwDepartmentCourseCount()
        {
            try
            {
                var result = _context.VwDepartmentCourseCount.FromSqlInterpolated($"select * from vwDepartmentCourseCount").ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
