using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.Exceptions;

namespace VmProjectBFF.Services
{
    public class BackgroundService1 : BackgroundService
    {
        protected readonly IBackendHttpClient _backendHttpClient;
        protected readonly IConfiguration _configuration;
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger<BackgroundService1> _logger;
        protected readonly int canvasStudentRoleId;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected BffResponse _lastResponse;
        public Timer _timer;

        public IServiceProvider Services;

        // private readonly DatabaseContext _context;
        // public List<CourseCreate> coursedata = new List<CourseCreate>();


        public BackgroundService1(
            IBackendHttpClient backendHttpClient,
            ILogger<BackgroundService1> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _backendHttpClient = backendHttpClient;
            _httpClientFactory = httpClientFactory;
            canvasStudentRoleId = int.Parse(_configuration["Canvas:StudentRoleId"]);
        }

        public async Task ReadAndUpdateDB(int sectionIdCreated=0, User newCourseProfessor = null)//this is our own identifier
        {
            _lastResponse = _backendHttpClient.Get($"api/v2/Role", new() { { "canvasRoleId", canvasStudentRoleId } });
            Role studentRole = JsonConvert.DeserializeObject<List<Role>>(_lastResponse.Response).FirstOrDefault();
            if(sectionIdCreated==0) { //This means it is running unattended
            try
            {
                //_lastResponse = _backendHttpClient.Post("api/v1/token", new DTO.AccessTokenDTO(_configuration.GetConnectionString("BackendConnectionPassword")));

                // if (!_lastResponse.HttpResponse.IsSuccessStatusCode)
                // {
                //     _logger.LogCritical("Could not authenticate to backend server in service \"BackendService1\". Unsuccessful response code or no cookie set.");
                //     return;
                // }

                _lastResponse = _backendHttpClient.Get("api/v1/User/canvasUsers");
                List<User> canvasUsers = JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);

                if (studentRole == null)
                {
                    studentRole = new();
                    studentRole.CanvasRoleId = canvasStudentRoleId;
                    studentRole.RoleName = "StudentEnrollment";

                    _lastResponse = _backendHttpClient.Post($"api/v2/Role", studentRole);
                    studentRole = JsonConvert.DeserializeObject<Role>(_lastResponse.Response);
                }

                if (canvasUsers.Count > 0)
                {
                    foreach (User professor in canvasUsers)
                    {
                        //call the new function instead of the code below, then we can call it outside the loop as well
                        _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "userId", professor.UserId } });
                        List<Section> sections = JsonConvert.DeserializeObject<List<Section>>(_lastResponse.Response);

                        foreach (Section section in sections)
                        {
                            await CreateNewSection(section, professor, studentRole);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("There is no sections imported from canvas");
                }


                BffResponse deleteResponse = _backendHttpClient.Delete("api/v1/token", null);
            }
            catch (BffHttpException be)
            {
                _logger.LogError("Error synchronizing with Canvas " + be.Message);
                return;
            }
            }
            else { //this is when a teacher just created a class

                _lastResponse = _backendHttpClient.Get($"api/v2/Section", new() { { "sectionId", sectionIdCreated } });
                Section section = JsonConvert.DeserializeObject<Section>(_lastResponse.Response);
                //we assume we will get back one section for the id provided
                await CreateNewSection(section, newCourseProfessor, studentRole);   //TO-DO:pass parameters 
            }
        }

        protected async Task CreateNewSection(Section section, User professor, Role studentRole)    
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
                            HttpResponseMessage response = await httpClient.GetAsync($"{_configuration.GetConnectionString("CanvasRootUri")}/api/v1/courses/{sectionId}/users?per_page=1000&role_id={canvasStudentRoleId}");

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

                                    if (studentEmail != null)
                                    {
                                        _lastResponse = _backendHttpClient.Get("api/v2/User", new() { { "email", studentEmail } });
                                        User student = JsonConvert.DeserializeObject<User>(_lastResponse.Response);
                                        // check if the student is already created, if not then create and enroll in that class

                                        if (student == null)
                                        {
                                            student = new User();
                                            student.Email = studentEmail;
                                            student.FirstName = studentFirstName;
                                            student.LastName = studentLastName;
                                            student.IsAdmin = false;

                                            _lastResponse = _backendHttpClient.Post("api/v2/User", student);
                                            student = JsonConvert.DeserializeObject<User>(_lastResponse.Response);
                                        }
                                        _lastResponse = _backendHttpClient.Get("api/v2/UserSectionRole", new() { { "userId", student.UserId }, { "sectionId", section.SectionId }, { "RoleId", studentRole.RoleId } });
                                        UserSectionRole enrollment = JsonConvert.DeserializeObject<UserSectionRole>(_lastResponse.Response);

                                        if (enrollment is null)//student enrollment hasn't been imported from canvas to database yet
                                        {
                                            enrollment = new UserSectionRole();

                                            enrollment.UserId = student.UserId;
                                            enrollment.RoleId = studentRole.RoleId;
                                            enrollment.SectionId = section.SectionId;

                                            _lastResponse = _backendHttpClient.Post("api/v2/UserSectionRole", enrollment);
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


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                await ReadAndUpdateDB();//0 means all sections
                // _logger.LogInformation("From background service");
            }
            await Task.CompletedTask;
        }
    }
}