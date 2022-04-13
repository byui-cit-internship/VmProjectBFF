using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Microsoft.Extensions.Configuration;


namespace vmProjectBackend.Services
{
    public class BackgroundService1 : BackgroundService
    {
        public IServiceProvider Services;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BackgroundService1> _logger;
        private readonly DatabaseContext _context;

        private readonly IConfiguration _configuration;

        private readonly int canvasStudentRoleId;

        // private readonly DatabaseContext _context;
        // public List<CourseCreate> coursedata = new List<CourseCreate>();
        ILogger Logger { get; } = AppLogger.CreateLogger<BackgroundService1>();


        public BackgroundService1(IHttpClientFactory httpClientFactory, DatabaseContext context, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _configuration = configuration;
            canvasStudentRoleId = Int32.Parse(_configuration["Canvas:StudentRoleId"]);

        }

        public async Task ReadAndUpdateDB()
        {
            List<User> canvasUsers = (from u in _context.Users
                                      where u.CanvasToken != null
                                      select u).ToList();

            Role studentRole = (from r in _context.Roles
                                where r.CanvasRoleId == canvasStudentRoleId
                                select r).FirstOrDefault();

            if (studentRole == null)
            {
                studentRole = new Role();
                studentRole.CanvasRoleId = canvasStudentRoleId;
                studentRole.RoleName = "StudentEnrollment";
                _context.Roles.Add(studentRole);
                _context.SaveChanges();
            }

            if (canvasUsers.Count > 0)
            {
                foreach (User professor in canvasUsers)
                {
                    List<Section> sections = (from u in _context.Users
                                              join usr in _context.UserSectionRoles
                                              on u.UserId equals usr.UserId
                                              join s in _context.Sections
                                              on usr.SectionId equals s.SectionId
                                              where u.UserId == professor.UserId
                                              select s).ToList();

                    foreach (Section section in sections)
                    {
                        // grab the id, canvas_token, section_num for every course
                        int sectionId = section.SectionCanvasId;
                        string profCanvasToken = professor.CanvasToken;

                        // This varible is changeable, it will chnage depending of the environment that the 
                        // project uses. We are using tutors to test this function and in Production we will use actual students

                        // call the Api for that course with the canvas token
                        // create an httpclient instance
                        HttpClient httpClient = _httpClientFactory.CreateClient();

                        // Authorization Token for the Canvas url that we are hitting, we need this for every courese
                        // and we will grab it 
                        httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {profCanvasToken}");

                        // contains our base Url where individula course_id is added
                        // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                        HttpResponseMessage response = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{sectionId}/users?per_page=1000&role_id={canvasStudentRoleId}");

                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();

                            // turn the Object into json, will convert this to be a type to work with soon
                            // We are grabing the all the Student enrolled for that class
                            dynamic students = JsonConvert.DeserializeObject<dynamic>(responseString);
                            // should get back a list of students objects

                            // looping thorough the list of students
                            foreach (var canvasStudent in students)
                            {
                                // grab the student Id and the email and name to create the student if they don't exits
                                // and enroll them in that class
                                string studentEmail = canvasStudent["email"];
                                string studentFullName = canvasStudent["name"];

                                string[] splitName = studentFullName.Split(' ');

                                string studentFirstName = splitName.First();
                                string studentLastName = splitName.Last();

                                // check if the student is already created, if not then create and enroll in that class
                                User student = (from u in _context.Users
                                                where u.Email == studentEmail
                                                select u).FirstOrDefault();

                                if (student == null)
                                {
                                    student = new User();
                                    student.Email = studentEmail;
                                    student.FirstName = studentFirstName;
                                    student.LastName = studentLastName;
                                    student.IsAdmin = false;
                                    _context.Users.Add(student);
                                    _context.SaveChanges();
                                }

                                var studentSectionEnrollments = (from u in _context.Users
                                                                 join usr in _context.UserSectionRoles
                                                                 on u.UserId equals usr.UserId
                                                                 join s in _context.Sections
                                                                 on usr.SectionId equals s.SectionId
                                                                 join c in _context.Courses
                                                                 on s.CourseId equals c.CourseId
                                                                 where u.UserId == student.UserId
                                                                 where s.SectionId == section.SectionId
                                                                 select new
                                                                 {
                                                                     course_id = s.SectionCanvasId,
                                                                     course_name = c.CourseName,
                                                                     enrollment_id = usr.UserSectionRoleId,
                                                                     student_name = $"{u.FirstName} {u.LastName}",
                                                                     template_id = c.TemplateVm
                                                                 }).ToList();

                                if (studentSectionEnrollments.Count == 0)//student enrollment hasn't been imported from canvas to database yet
                                {

                                    UserSectionRole enrollment = new UserSectionRole();
                                    enrollment.UserId = student.UserId;
                                    enrollment.RoleId = studentRole.RoleId;
                                    enrollment.SectionId = section.SectionId;
                                    _context.UserSectionRoles.Add(enrollment);
                                    _context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("You are not authorized or course does not exits");
                            Console.WriteLine(response.StatusCode);
                        }
                        Console.WriteLine("here 5");
                    }
                }
            }
            else
            {
                Console.WriteLine("There is no sections imported from canvas");
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await ReadAndUpdateDB();
                // _logger.LogInformation("From background service");
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
            await Task.CompletedTask;
        }
    }
}