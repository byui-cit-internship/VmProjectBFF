using Microsoft.Extensions.Configuration;
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
using vmProjectBFF.Models;
using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public class BackgroundService1 : BackgroundService
    {
        private readonly Backend _backend;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BackgroundService1> _logger;
        private readonly int canvasStudentRoleId;
        private readonly IHttpContextAccessor _contextAccessor;
        private BackendResponse _lastResponse;

        public IServiceProvider Services;

        // private readonly DatabaseContext _context;
        // public List<CourseCreate> coursedata = new List<CourseCreate>();


        public BackgroundService1(
            ILogger<BackgroundService1> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _backend = new(_contextAccessor, _logger, _configuration);
            _httpClientFactory = httpClientFactory;
            canvasStudentRoleId = Int32.Parse(_configuration["Canvas:StudentRoleId"]);
        }

        public async Task ReadAndUpdateDB()
        {
            try
            {
                _backend.Cookie = "temp";
                _lastResponse = _backend.Get("");
                _lastResponse = _backend.Post("api/v1/token", new DTO.AccessTokenDTO(_configuration.GetConnectionString("BackendConnectionPassword")));

                if (!_lastResponse.HttpResponse.IsSuccessStatusCode && !_lastResponse.HttpResponse.Headers.Contains("Set-Cookie"))
                {
                    _logger.LogCritical("Could not authenticate to backend server in service \"BackendService1\". Unsuccessful response code or no cookie set.");
                    return;
                }

                string cookieHeader = _lastResponse.HttpResponse.Headers.GetValues("Set-Cookie")?.ToArray()[0];
                if (cookieHeader == null)
                {
                    _logger.LogCritical("Could not authenticate to backend server in service \"BackendService1\". Could not parse cookie value returned from backend on successful response");
                    return;
                }
                string cookie = cookieHeader.Split(';', 2)[0];
                _backend.Cookie = cookie;


                _lastResponse = _backend.Get("api/v1/User/canvasUsers");
                List<User> canvasUsers = JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);


                _lastResponse = _backend.Get($"api/v2/Role", new { canvasRoleId = canvasStudentRoleId });
                Role studentRole = JsonConvert.DeserializeObject<List<Role>>(_lastResponse.Response).FirstOrDefault();

                if (studentRole == null)
                {
                    studentRole = new();
                    studentRole.CanvasRoleId = canvasStudentRoleId;
                    studentRole.RoleName = "StudentEnrollment";

                    _lastResponse = _backend.Post($"api/v2/Role", studentRole);
                    studentRole = JsonConvert.DeserializeObject<Role>(_lastResponse.Response);
                }

                if (canvasUsers.Count > 0)
                {
                    foreach (User professor in canvasUsers)
                    {
                        _lastResponse = _backend.Get($"api/v2/Section", new { userId = professor.UserId });
                        List<Section> sections = JsonConvert.DeserializeObject<List<Section>>(_lastResponse.Response);

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


                                    _lastResponse = _backend.Get("api/v2/User", new { email = studentEmail });
                                    User student = JsonConvert.DeserializeObject<User>(_lastResponse.Response);
                                    // check if the student is already created, if not then create and enroll in that class

                                    if (student == null)
                                    {
                                        student = new User();
                                        student.Email = studentEmail;
                                        student.FirstName = studentFirstName;
                                        student.LastName = studentLastName;
                                        student.IsAdmin = false;

                                        _lastResponse = _backend.Post("api/v2/User", student);
                                        student = JsonConvert.DeserializeObject<User>(_lastResponse.Response);
                                    }
                                    _lastResponse = _backend.Get("api/v2/UserSectioRole", new { userId = student.UserId, sectionId = section.SectionId });
                                    UserSectionRole enrollment = JsonConvert.DeserializeObject<UserSectionRole>(_lastResponse.Response);

                                    if (enrollment == null)//student enrollment hasn't been imported from canvas to database yet
                                    {
                                        enrollment.UserId = student.UserId;
                                        enrollment.RoleId = studentRole.RoleId;
                                        enrollment.SectionId = section.SectionId;

                                        _lastResponse = _backend.Post("api/v2/UserSectionRole", enrollment);
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


                BackendResponse deleteResponse = _backend.Delete("api/v1/token", null);
            } catch (BackendException be)
            {
                return;
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