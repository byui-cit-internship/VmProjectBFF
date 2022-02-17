using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Services
{
    public class BackgroundService1 : BackgroundService
    {
        public IServiceProvider Services;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BackgroundService1> _logger;
        private readonly IConfiguration _Configuration;
        private readonly DatabaseContext _context;

        // private readonly DatabaseContext _context;
        // public List<CourseCreate> coursedata = new List<CourseCreate>();


        public BackgroundService1(ILogger<BackgroundService1> logger, IServiceProvider service, IHttpClientFactory httpClientFactory, IConfiguration configuration, DatabaseContext context)
        {
            _logger = logger;
            Services = service;
            _httpClientFactory = httpClientFactory;
            _Configuration = configuration;
            _context = context;
        }

        public async Task ReadAndUpdateDB()
        {
            using (var scope = Services.CreateScope())
            {
                List<User> canvasList = (from u in _context.Users
                                         where u.CanvasToken != null
                                         select u).ToList();

                

                if (canvasList.Count > 0)
                {
                    // Figure out a better way to just filter only enrollment with Teachers
                    // for now we are looping through all the enrollment and finding the ones that are for teachers.
                    Console.WriteLine("first loop");
                    foreach (var enroll in listOfenroll)
                    {
                        // check if it is a Teacher enrollment
                        if (enroll.UserId == enroll.teacherId)
                        {
                            // grab the id, canvas_token, section_num for every course
                            var _course_id = enroll.CourseID;
                            // long _course_id = 117072;
                            var _course_sectionnum = enroll.section_num;
                            var _course_canvas_token = enroll.canvas_token;

                            // This varible is changeable, it will chnage depending of the environment that the 
                            // project uses. We are using tutors to test this function and in Production we will use actual students

                            var user_role_id = 3;
                            // call the Api for that course with the canvas token
                            // create an httpclient instance
                            var httpClient = _httpClientFactory.CreateClient();

                            // Authorization Token for the Canvas url that we are hitting, we need this for every courese
                            // and we will grab it 
                            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Bearer " + _course_canvas_token);
                            // contains our base Url where individula course_id is added
                            // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                            var response = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{_course_id}/enrollments?per_page=1000&role_id={user_role_id}");

                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = await response.Content.ReadAsStringAsync();
                                // turn the Object into json, will convert this to be a type to work with soon
                                // We are grabing the all the Student enrolled for that class
                                dynamic listOfcurrent_StudentObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                                // should get back a list of students objects

                                // looping thorough the list of student
                                Console.WriteLine("second loop");

                                foreach (var student in listOfcurrent_StudentObject)
                                {
                                    int target = listOfcurrent_StudentObject.Count;
                                    if (student.Count != 0)
                                    {
                                        var student_id = student["user_id"];
                                        // Take the student_id and call the other Api to get just user Information
                                        var studentInfoResponse = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{_course_id}/users?search_term={student_id}");
                                        if (studentInfoResponse.IsSuccessStatusCode)
                                        {
                                            string studentResponseString = await studentInfoResponse.Content.ReadAsStringAsync();
                                            // turn the Object into json, will convert this to be a type to work with soon
                                            // We are grabing the Student info for that class
                                            dynamic current_studentObject = JsonConvert.DeserializeObject<dynamic>(studentResponseString);
                                            if (current_studentObject.Count != 0)
                                            {
                                                // grab the student Id and the email and name to create the student if they don't exits
                                                // and enroll them in that class
                                                var current_student_id = current_studentObject[0]["id"];
                                                string current_student_email = current_studentObject[0]["email"];

                                                string studentnames = current_studentObject[0]["name"];
                                                string[] names = studentnames.Split(' ');
                                                int lastIndex = names.GetUpperBound(0);
                                                string current_student_firstName = names[0];
                                                string current_student_lastName = names[lastIndex];

                                                // check if the student is already created, if not then create and enroll in that class
                                                var current_student_in_db = _context.Users.Where(u => u.email == current_student_email).FirstOrDefault();

                                                Console.WriteLine("here is the student details");
                                                if (current_student_in_db != null)
                                                {
                                                    Console.WriteLine("student already exits");
                                                    //  search to see if the current student is enrolled in the class
                                                    var current_student_enrollment = _context.Enrollments.Where(e => e.UserId == current_student_in_db.UserID
                                                                                                          && e.CourseID == _course_id)
                                                                                                          .FirstOrDefault();
                                                    Guid current_student_enrollid = current_student_enrollment.UserId;
                                                    if (current_student_enrollment == null)
                                                    {
                                                        //Enroll that Student to that course  
                                                        await EnrollStudent(_course_id, current_student_enrollid, enroll.teacherId, enroll.VmTableID, _course_sectionnum, enroll.semester);
                                                        Console.WriteLine("Student enrolled into the course");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Student already enrolled");
                                                    }
                                                }
                                                else
                                                {
                                                    //  We now know that the student is not in the Database so we create a new user

                                                    Console.WriteLine("Student does not exit in Db");

                                                    User student_user = new User();

                                                    student_user.firstName = current_student_firstName;
                                                    student_user.lastName = current_student_lastName;
                                                    student_user.email = current_student_email;
                                                    student_user.userType = "Student";
                                                    _context.Users.Add(student_user);
                                                    await _context.SaveChangesAsync();
                                                    Console.WriteLine("Student_user was created");

                                                    // Enroll the newly created student into that course
                                                    await EnrollStudent(_course_id, student_user.UserID, enroll.teacherId, enroll.VmTableID, _course_sectionnum, enroll.semester);
                                                    Console.WriteLine("Student now created and enrolled into the course");
                                                }
                                            }
                                        }

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
                    Console.WriteLine("There is no Enrollment");
                }
            }

        }

        async Task EnrollStudent(long _course_id, Guid userid, Guid teacherid, Guid vmtableId, string sectionnum, string semester)
        {
            Section enrollment = new Section();
            long enroll_course_id = _context.Courses.FirstOrDefault(c => c.CourseID == _course_id).CourseID;
            enrollment.CourseID = enroll_course_id;
            enrollment.UserId = userid;
            enrollment.teacherId = teacherid;
            enrollment.VmTableID = vmtableId;
            enrollment.Status = "InActive";
            enrollment.section_num = sectionnum;
            enrollment.semester = semester;
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
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