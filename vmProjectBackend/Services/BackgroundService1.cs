using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using System.Net.Http;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Data.Entity;

namespace vmProjectBackend.Services
{
    public class BackgroundService1 : BackgroundService
    {
        public IServiceProvider Services;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BackgroundService1> _logger;
        private readonly IConfiguration _Configuration;

        // private readonly VmContext _context;
        // public List<CourseCreate> coursedata = new List<CourseCreate>();


        public BackgroundService1(ILogger<BackgroundService1> logger, IServiceProvider service, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            Services = service;
            _httpClientFactory = httpClientFactory;
            _Configuration = configuration;
        }

        // View the database
        public async Task ReadAndUpdateDB()
        {
            // var enrollment = _context.Courses.FindAsync();
            using (var scope = Services.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<VmContext>();
                Guid g = new Guid("cbaef875-6ea0-4c6c-0d49-08d9adf8a71c");
                // var _listOfCourse = _context.Courses.ToList();
                Console.WriteLine("Enrollment");
                List<Enrollment> listOfenroll = _context.Enrollments.Where(e => e.teacherId == e.UserId).ToList();
                Console.WriteLine(listOfenroll);
                if (listOfenroll != null)
                {
                    // Figure out a better way to just filter only enrollment with Teachers
                    // for now we are looping through all the enrollment and finding the ones that are for teachers.
                    foreach (var enroll in listOfenroll)
                    {
                        // check if it is a Teacher enrollment
                        if (enroll.UserId == enroll.teacherId)
                        {
                            Console.WriteLine("here is teacher Id");
                            Console.WriteLine(enroll.teacherId);
                            // grab the id, canvas_token, section_num for every course
                            // var _course_id = enroll.CourseID;
                            long _course_id = 117072;
                            var _course_sectionnum = enroll.section_num;
                            var _course_canvas_token = enroll.canvas_token;
                            Console.WriteLine($"Course Id: {_course_id},Section Num: {_course_sectionnum}, CanvasToken: {_course_canvas_token}");

                            // This varible is changeable, it will chnage depending of the environment that the 
                            // project uses. We are using tutors to test this function and in Production we will use actual students
                            var user_role_id = _Configuration["Canvas:StudentRoleId"];

                            // call the Api for that course with the canvas token
                            // create an httpclient instance
                            var httpClient = _httpClientFactory.CreateClient();
                            // Console.WriteLine("here 2");
                            // Authorization Token for the Canvas url that we are hitting, we need this for every courese
                            // and we will grab it 
                            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, "Bearer 10706~dMj9lrtPek3cvjbK5ZpnpMHGpY5T6oN9mlDsNqgKnbdZ3UZnA5REJSajwQTXMXo2");
                            // contains our base Url where individula course_id is added
                            // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                            var response = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{_course_id}/enrollments?per_page=1000&role_id={user_role_id}");

                            if (response.IsSuccessStatusCode)
                            {
                                // Console.WriteLine("passed here 3 ");
                                string responseString = await response.Content.ReadAsStringAsync();
                                // Console.WriteLine(responseString);
                                // turn the Object into json, will convert this to be a type to work with soon
                                // We are grabing the all the Student enrolled for that class

                                // var responseObject = JsonConvert.DeserializeObject(responseString);
                                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                                var student_id = responseObject[0]["user_id"];

                                // Take the student_id and call the other Api to get just user Information
                                var studentInfoResponse = await httpClient.GetAsync($"https://byui.test.instructure.com/api/v1/courses/{_course_id}/users?search_term={student_id}");
                                if (studentInfoResponse.IsSuccessStatusCode)
                                {
                                    string studentResponseString = await studentInfoResponse.Content.ReadAsStringAsync();
                                    // turn the Object into json, will convert this to be a type to work with soon
                                    // We are grabing the Student info for that class
                                    dynamic studentObject = JsonConvert.DeserializeObject<dynamic>(studentResponseString);
                                    // grab the student Id and the email and name to create the student if they don't exits
                                    // and enroll them in that class
                                    Console.WriteLine("here in the student object");
                                    Console.WriteLine(studentObject[0]["id"]);
                                    Console.WriteLine(studentObject[0]["email"]);

                                    var current_student_id = studentObject[0]["id"];
                                    string current_student_email = studentObject[0]["email"];

                                    // check if the student is already created, if not then create and enroll in that class
                                    var _studentDetails = _context.Users.Where(u => u.email == current_student_email).FirstOrDefault();
                                    string studentnames = studentObject[0]["name"];
                                    string[] names = studentnames.Split(' ');
                                    int lastIndex = names.GetUpperBound(0);
                                    string current_student_firstName = names[0];
                                    string current_student_lastName = names[lastIndex];

                                    Console.WriteLine("here is the student details");
                                    if (_studentDetails != null)
                                    {
                                        Console.WriteLine("student already exits");
                                        // Check if they are enrolled in that class also, because they can be in the Database, but not in
                                        // the class that is same as canvas and this application.

                                        // check if they are enrolled in the same course as canvas and the database

                                    }
                                    else
                                    {
                                        //  We now know that the student is not in the Database so we create a new user
                                        // create the course
                                        Console.WriteLine("Student does not exit in Db");

                                        // User student_user = new User();
                                        // Console.WriteLine("Student mode 1");
                                        // student_user.firstName = current_student_firstName;
                                        // Console.WriteLine("Student mode 2");
                                        // student_user.lastName = current_student_lastName;
                                        // Console.WriteLine("Student mode 3");
                                        // student_user.email = current_student_email;
                                        // Console.WriteLine("Student mode 4");
                                        // student_user.userType = "Student";
                                        // Console.WriteLine("Student mode 5");
                                        // _context.Users.Add(student_user);
                                        // Console.WriteLine("Student mode 6");
                                        // await _context.SaveChangesAsync();
                                        // Console.WriteLine("Student mode 7");

                                        // // // Enroll that Student to that course                                                            
                                        // Enrollment enrollment = new Enrollment();
                                        // Console.WriteLine("Student mode 1");

                                        // enrollment.CourseID = _course_id;
                                        // Console.WriteLine("Student mode 8");
                                        // enrollment.UserId = student_user.UserID;
                                        // Console.WriteLine("Student mode 9");
                                        // enrollment.teacherId = enroll.teacherId;
                                        // Console.WriteLine("Student mode 10");
                                        // enrollment.VmTableID = enroll.VmTableID;
                                        // Console.WriteLine("Student mode 11");
                                        // enrollment.Status = "InActive";
                                        // Console.WriteLine("Student mode 12");
                                        // enrollment.section_num = _course_sectionnum;
                                        // Console.WriteLine("Student mode 13");
                                        // enrollment.semester = enroll.semester;
                                        // Console.WriteLine("Student mode 14");
                                        // _context.Enrollments.Add(enrollment);
                                        // Console.WriteLine("Student mode 15");

                                        // Console.WriteLine("Student mode 16");
                                    }


                                    // Console.WriteLine("this is student_id");
                                    // Console.WriteLine(student_id);
                                    // Console.WriteLine("this is course_id");
                                    // Console.WriteLine(_course_id);
                                    // Console.WriteLine("this is section_num");
                                    // Console.WriteLine(_course_sectionnum);
                                    // Console.WriteLine("this is teacher_id");
                                    // Console.WriteLine(enroll.teacherId);
                                    // Console.WriteLine("this is vmtable_id");
                                    // Console.WriteLine(enroll.VmTableID);
                                    // Console.WriteLine("this is semester");
                                    // Console.WriteLine(enroll.semester);


                                }
                                // From that Response, find the Email, and name of student and store it.
                                // Now we have, student_id and email, we check if we have that student in the database, if not we create that user
                                // and enroll that use into that course 

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


                // Ask for a easier way to just filter the enrollment for the Teachers 



                // Loop through all the courses in the database
                // store those course name and course ID in a list and their canvas_token
                // Take an course Id and canvas_token from that List and call the canvas API to get a list of Student
                // And for every student that you get, take their ID and call the Individual Canvas API to get their Infomation 
                // which is primarily their email.Check if that User is already created, if not Use that to create a user(student user)
                // Then create a list of Users(student). Then enroll them into a class for that section.
                //  Do this for every course in the Database.



                // foreach (var )
            }

        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await ReadAndUpdateDB();
                // _logger.LogInformation("From background service");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            await Task.CompletedTask;
        }
    }
}