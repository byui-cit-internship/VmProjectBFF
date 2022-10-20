// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
// using vmProjectBFF.DTO;
// using vmProjectBFF.Exceptions;
// using vmProjectBFF.Models;
// using vmProjectBFF.Services;

// separation
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0

// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using RPauth.Data;

// var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//                                 options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();
// builder.Services.AddRazorPages();

// builder.Services.Configure<IdentityOptions>(options =>
// {
    // Default Password settings.
//     options.Password.RequireDigit = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequireNonAlphanumeric = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequiredLength = 6;
//     options.Password.RequiredUniqueChars = 1;
// });

// var app = builder.Build();

// Remaining code removed for brevity.
// separation




















// namespace vmProjectBFF.Controllers
// {
//     [Authorize]
//     [Route("api/[controller]")]
//     [ApiController]
//     public class UserController : BffController
//     {

        // public UserController(
        //     IAuthorization authorization,
        //     IBackendRepository backend,
        //     ICanvasRepository canvas,
        //     IConfiguration configuration,
        //     IHttpClientFactory httpClientFactory,
        //     IHttpContextAccessor httpContextAccessor,
        //     ILogger<UserController> logger,
        //     IVCenterRepository vCenter)
            // : base(
        //           authorization: authorization,
        //           backend: backend,
        //           canvas: canvas,
        //           configuration: configuration,
        //           httpClientFactory: httpClientFactory,
        //           httpContextAccessor: httpContextAccessor,
        //           logger: logger,
        //           vCenter: vCenter)
        // {
        // }

//         [HttpPost]
// [ValidateAntiForgeryToken]
// public async Task<IActionResult> Register(UserRegistrationModel userModel)
// {
//     if (!ModelState.IsValid)
//     {
//         return View(userModel);
//     }

//     var user = _mapper.Map<User>(userModel);

//     var result = await _userManager.CreateAsync(user, userModel.Password);
//     if (!result.Succeeded)
//     {
//         foreach (var error in result.Errors)
//         {
//             ModelState.TryAddModelError(error.Code, error.Description);
//         }

//         return View(userModel);
//     }

//     var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//     var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

//     var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
//     await _emailSender.SendEmailAsync(message);

//     await _userManager.AddToRoleAsync(user, "Visitor");

//     return RedirectToAction(nameof(SuccessRegistration));
// }

        /***********************************************
         * This is for updating a user
         * If you are updating your self. 
         * ********************************************/

        // [HttpPut("")]
        // public async Task<ActionResult> PutUser(User user)
        // {
        //     User admin = _authorization.GetAuth("admin");
        //     if (admin != null)
        //     {
        //         _lastResponse = _backendHttpClient.Put("api/v2/User", user);
        //         User changedUser = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

        //         if (admin.UserId == changedUser.UserId)
        //         {
        //             return Ok(changedUser);
        //         }
        //         else
        //         {
        //             return Ok("User was modified successfully");
        //         }

        //     }
        //     User authUser = _authorization.GetAuth("user");
        //     if (authUser != null)
        //     {
        //         if (user.UserId == authUser.UserId && user.IsAdmin == authUser.IsAdmin)
        //         {
        //             _lastResponse = _backendHttpClient.Put("api/v2/User", user);
        //             User changedUser = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

        //             return Ok(changedUser);
        //         }
        //         else
        //         {
        //             return BadRequest("Bad Request");
        //         }
        //     }


        //     return Unauthorized("Contact your admin to get access");


        // }

        /****************************************
        Create or update a user in the database to have admin privileges
        ****************************************/
        // [HttpPost("admin/createuser")]
        // public async Task<ActionResult<User>> PostAdminUser(PostAdmin postUser)
        // {
        //     try
        //     {
        //         User admin = _authorization.GetAuth("admin");

        //         if (admin != null)
        //         {
        //             _lastResponse = _backendHttpClient.Get("api/v2/User", new { email = postUser.email });
        //             User user = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

        //             if (user == null)
        //             {
        //                 user = new User();
        //                 user.Email = postUser.email;
        //                 user.FirstName = postUser.firstName;
        //                 user.LastName = postUser.lastName;
        //                 user.IsAdmin = true;

        //                 _lastResponse = _backendHttpClient.Post("api/v2/User", user);

        //                 return Ok("created user as admin");
        //             }
        //             else
        //             {
        //                 user.IsAdmin = true;

        //                 _lastResponse = _backendHttpClient.Put("api/v2/User", user);

        //                 return Ok("modified user to be admin");
        //             }
        //         }
        //         return Unauthorized();
        //     }
        //     catch (BffHttpException be)
        //     {
        //         return StatusCode((int)be.StatusCode, be.Message);
        //     }
        // }
        // [HttpGet("professors")]
        // public async Task<ActionResult> GetProfessors()
        // {
        //     try
        //     {
        //         User professor = _authorization.GetAuth("admin");

        //         if (professor != null)
        //         {

        //             _lastResponse = _backendHttpClient.Get($"api/v2/user", new { isAdmin = true });
        //             List<User> professorList = JsonConvert.DeserializeObject<List<User>>(_lastResponse.Response);
        //             return Ok(professorList);
        //         }
        //         else
        //         {
        //             return Unauthorized("You are not a professor");
        //         }
        //     }
        //     catch (BffHttpException be)
        //     {
        //         return StatusCode((int)be.StatusCode, be.Message);
        //     }
        // }
        // [HttpGet("bySection")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status403Forbidden)]
//         public async Task<ActionResult> GetUsersByCourse(int sectionId)
//         {
//             try
//             {
//                 User professor = _authorization.GetAuth("admin");

//                 if (professor is not null)
//                 {
//                     return Ok(_backend.GetUsersBySection(sectionId));
//                 }
//                 else
//                 {
//                     return Forbid();
//                 }
//             }
//             catch (BffHttpException be)
//             {
//                 return StatusCode((int)be.StatusCode, be.Message);
//             }
//         }
//     }
// }
