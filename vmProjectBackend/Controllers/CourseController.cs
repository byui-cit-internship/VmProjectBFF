using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly DatabaseContext _context;

        ILogger Logger { get; } = AppLogger.CreateLogger<CourseController>();
        public IHttpClientFactory _httpClientFactory { get; }

        public CourseController(DatabaseContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public User getProfessor(string emailToTest)
        {
            IQueryable<User> professors = (from u in _context.Users
                                           join usr in _context.UserSectionRoles
                                           on u.UserId equals usr.UserId
                                           join r in _context.Roles
                                           on usr.RoleId equals r.RoleId
                                           where r.RoleName == "Professor"
                                           && u.Email == emailToTest
                                           select u);
            return professors.DefaultIfEmpty(null).FirstOrDefault();
        }

        /****************************************
        Teachers should see a list of their classes 
        that they have for that semester and that section

        **************************************/

        // GET: api/Course/1/winter
        [HttpGet("professor/allcourses/{year}/{term}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> Get_Courses_section_specific(int year, string term)
        {
            // grabbing the user that signed in
            string userEmail = HttpContext.User.Identity.Name;
            // check if it is a professor

            User professor = getProfessor(userEmail);
            Logger.LogInformation("User Email: " + professor);

            if (professor != null)
            {
                var sectionList = (from sem in _context.Semesters
                                   join sec in _context.Sections
                                   on sem.SemesterId equals sec.SemesterId
                                   join c in _context.Courses
                                   on sec.CourseId equals c.CourseId
                                   join usr in _context.UserSectionRoles
                                   on sec.SectionId equals usr.SectionId
                                   join u in _context.Users
                                   on usr.UserId equals u.UserId
                                   where u.UserId == professor.UserId
                                   && sem.SemesterYear == year
                                   && sem.SemesterTerm == term
                                   select new
                                   {
                                       course_name = c.CourseCode,
                                       course_id = sec.SectionCanvasId,
                                       course_semester = $"{sem.SemesterTerm} {sem.SemesterYear}",
                                       course_section = sec.SectionNumber,
                                       course_professor = $"{professor.FirstName} {professor.LastName}"
                                   }).ToList();

                return Ok(sectionList);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }

        /*******************************
        teacher should see a list of their classes in all section for a specific semester

        ****************************************/
        // GET: api/Course/fall
        [HttpGet("professor/semester/{course_semester}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> GetCourses_semester(string course_semester)
        {
            // grabbing the user that signed in
            string useremail = HttpContext.User.Identity.Name;

            // check if it is a professor
            User professor = getProfessor(useremail);

            // Console.WriteLine("this is the user email" + useremail);
            if (professor != null)
            {
                var listOfCourse = (from c in _context.Courses
                                    join sec in _context.Sections
                                    on c.CourseId equals sec.CourseId
                                    join sem in _context.Semesters
                                    on sec.SemesterId equals sem.SemesterId
                                    join usr in _context.UserSectionRoles
                                    on sec.SectionId equals usr.SectionId
                                    join u in _context.Users
                                    on usr.UserId equals u.UserId
                                    where u.Email == professor.Email
                                    && sem.SemesterTerm == course_semester
                                    select new
                                    {
                                        course_name = c.CourseName,
                                        course_id = sec.SectionId,
                                        course_semester = sem.SemesterTerm,
                                        course_section = sec.SectionNumber,
                                        course_professor = $"{u.FirstName} {u.LastName}"
                                    }).ToList();

                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }

        /***********************************
        Teacher searching for a specifc course and check the VM for that class

        ********************************/
        // GET: api/Course/5/3/fall
        /*[HttpGet("professor/course/{course_Id}/{semester}/{sectionnum}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult<Course>> GetCourse(long course_Id, string semester, int sectionnum)
        {
            string userEmail = HttpContext.User.Identity.Name;
            // check if it is a professor
            User professor = getProfessor(userEmail);

            if (professor != null)
            {
                var courses = (from u in _context.Users
                              join usr in _context.UserSectionRoles
                              on u.UserId equals usr.UserId
                              join sec in _context.Sections
                              on usr.SectionId equals sec.SectionId
                              join c in _context.Courses
                              on sec.CourseId equals c.CourseId
                              join sem in _context.Semesters
                              on sec.SemesterId equals sem.SemesterId
                              where u.Email == professor.Email
                              && sec.SectionCanvasId == course_Id
                              && sem.SemesterTerm == semester
                              && sec.SectionNumber == sectionnum
                              select new
                              {
                                  course_name = c.CourseName,
                                  section_num = sec.SectionNumber,
                                  semester = $"{sem.SemesterTerm} {sem.SemesterYear}"
                              }).DefaultIfEmpty(null).FirstOrDefault();
                int courseTag = (from t in _context.Tags
                                where t.TagName == courses.course_name
                                select t.TagId).FirstOrDefault();
                int secTag = (from t in _context.Tags
                              where t.TagName == $"{courses.section_num}"
                              select t.TagId).FirstOrDefault();
                int semTag = (from t in _context.Tags
                              where t.TagName == courses.semester
                              select t.TagId).FirstOrDefault();
                var vms = from vmi in _context.VmInstances
                          join tvm in _context.VmInstanceTags
                          on vmi.VmUserInstanceId equals tvm.VmInstanceId
                          where tvm.TagId ==
                var singleCourse = await _context.Enrollment
                                .Include(c => c.Course)
                                .Include(vm => vm.VmTable)
                                .Where(c => c.CourseID == course_Id
                                        && c.section_num == sectionnum
                                        && c.semester == semester
                                        && c.UserId == user_prof.UserID)
                                .Select(c => new
                                {
                                    course_name = c.Course.CourseName,
                                    course_id = c.CourseID,
                                    courses_semester = c.semester,
                                    course_section = c.section_num,
                                    course_vm = c.VmTable
                                })
                                .ToListAsync();
                return Ok(singleCourse);
            }
            else
            {
                return NotFound("No such Course found");
            }
        }*/

        /********************************************
        Teacher wanting to know who is registered for their
        class
        **************************/
        [HttpGet("professor/students/{course_Id}/{course_semester}/{sectionnum}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> Get_Students_section_specific(long course_Id, string course_semester, string sectionnum)
        {
            // grabbing the user that signed in
            string userEmail = HttpContext.User.Identity.Name;

            // check if it is a professor
            User professor = getProfessor(userEmail);

            // Console.WriteLine("this is the user email" + useremail);
            if (professor != null)
            {
                var listOfCourse = await _context.Enrollments
                                .Include(c => c.User)
                                .Include(c => c.Course)
                                .Include(vm => vm.VmTable)
                                .Where(u => u.teacherId == user_prof.UserID
                                        && u.section_num == sectionnum
                                        && u.semester == course_semester
                                        && u.Course.CourseID == course_Id
                                        && u.User.userType == "Student"
                                        )
                                .Select(s => new
                                {
                                    course_name = s.Course.CourseName,
                                    course_id = s.CourseID,
                                    course_semester = s.semester,
                                    course_sectionnum = s.section_num,
                                    student_name = $"{s.User.firstName} {s.User.lastName}",
                                    student_vm_status = s.Status,
                                    course_vm = s.VmTable
                                })
                                .ToListAsync();
                return Ok(listOfCourse);
            }

            else
            {
                return NotFound("You are not Authorized and not a Professor");
            }
        }

        /************************************************
        Teacher changing the status of a vm for a student
        ***********************/
        /*[HttpPatch("professor/changeVmStatus/{studentId}/{courseId}/{sectionNum}/{coursesemester}")]
        // The Url param should match the varibales that you will pass into function below
        public async Task<ActionResult> ChangeVmStatus(Guid studentId, long courseId, string sectionNum, string coursesemester, JsonPatchDocument<Section> patchDoc)
        {
            // verify it is a teacher
            string useremail = HttpContext.User.Identity.Name;
            var user_prof = _context.Users
                            .Where(p => p.email == useremail
                                    && p.userType == "Professor")
                            .FirstOrDefault();
            if (user_prof != null)
            {
                // need the StudentId and the student Enrollment, check if the enrollment
                //  has a teacher that matches with the teacher Id. Then do the patch else, Professor is not 
                // Authorized
                var student_enrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(c => c.UserId == studentId
                                       && c.teacherId == user_prof.UserID
                                       && c.CourseID == courseId
                                       && c.section_num == sectionNum
                                       && c.semester == coursesemester);

                if (student_enrollment == null)
                {
                    return NotFound();
                }
                patchDoc.ApplyTo(student_enrollment, ModelState);
                await _context.SaveChangesAsync();

                return Ok(student_enrollment);
            }
            return Unauthorized("You are not authorized");

        }*/

        /***********************************************
        Teachers change their course
        ************************/

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{course_id}")]
        public async Task<IActionResult> PutCourse(long course_id, Course course)
        {
            if (course_id != course.CourseID)
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
                if (!CourseExists(course_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        /**************************

        Teacher change course
        ****************/

        // POST: api/Course
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            // Do I need to grab the section num, semester in the url and another body data with the Vm data
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                _context.Courses.Add(course);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (CourseExists(course.CourseID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
                return CreatedAtAction("GetCourse", new { id = course.CourseID }, course);
            }
            return Unauthorized("You are not a professor");

        }*/


        /**************************************************
        Teacher delete course

        ********************/

        // DELETE: api/Course/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(long id)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();

            if (user_prof != null)
            {
                var singleCourse = _context.Enrollments
                                    .Include(c => c.Course)
                                    .Where(c => c.CourseID == id && c.UserId == user_prof.UserID)
                                    .FirstOrDefault();
                var course = await _context.Courses.FindAsync(id);
                if (course == null)
                {
                    return NotFound();
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            return Unauthorized("You are not a professor");




        }

        private bool CourseExists(long id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }*/

        /************************************************

        Teacher patch their course

        ***********************/
        /*[HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdate(long id, JsonPatchDocument<Course> patchDoc)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();
            // not complete as yet
            if (user_prof != null)
            {
                var orginalCourse = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == id);

                if (orginalCourse == null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(orginalCourse, ModelState);
                await _context.SaveChangesAsync();

                return Ok(orginalCourse);


            }
            return Unauthorized("You are not Authorized and is not a Professor");

        }*/

        /************************************************
        Endpoint that will check the validity of the courseId and canvas token
        ***********************/
        /*[HttpPost("professor/checkCanvasToken")]
        public async Task<ActionResult> CallCanvas([FromBody] CanvasCredentials canvasCredentials)
        {
            string useremail = HttpContext.User.Identity.Name;
            // check if it is a professor
            var user_prof = _context.Users
                            .Where(p => p.email == useremail && p.userType == "Professor")
                            .FirstOrDefault();
            // not complete as yet
            if (user_prof != null)
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Bearer " + canvasCredentials.canvas_token);
                // contains our base Url where individula course_id is added
                // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                var response = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{canvasCredentials.canvas_course_id}/enrollments?per_page=1000");

                if (response.IsSuccessStatusCode)
                {
                    return Ok(canvasCredentials);
                }
                return Unauthorized("Invalid token");
                // return Ok(canvasCredentials);
            }
            return Unauthorized("You are not Authorized and is not a Professor");

        }*/
    }
}
