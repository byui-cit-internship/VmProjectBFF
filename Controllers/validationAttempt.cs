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
//                                        options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();
// builder.Services.AddRazorPages();

// builder.Services.Configure<IdentityOptions>(options =>
// {
//     Default Lockout settings.
//     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//     options.Lockout.MaxFailedAccessAttempts = 5;
//     options.Lockout.AllowedForNewUsers = true;
//     options.Password.RequireDigit = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequireNonAlphanumeric = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequiredLength = 6;
//     options.Password.RequiredUniqueChars = 1;
// });

// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseMigrationsEndPoint();
// }
// else
// {
//     app.UseExceptionHandler("/Error");
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapRazorPages();

// app.Run();


// builder.Services.Configure<IdentityOptions>(options =>
// {
//     Default SignIn settings.
//     options.SignIn.RequireConfirmedEmail = false;
//     options.SignIn.RequireConfirmedPhoneNumber = false;
//     options.User.AllowedUserNameCharacters =
//             "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//     options.User.RequireUniqueEmail = false;
// });

// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.AccessDeniedPath = "/Identity/Account/AccessDenied";
//     options.Cookie.Name = "YourAppCookieName";
//     options.Cookie.HttpOnly = true;
//     options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
//     options.LoginPath = "/Identity/Account/Login";
//     ReturnUrlParameter requires 
//     using Microsoft.AspNetCore.Authentication.Cookies;
//     options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
//     options.SlidingExpiration = true;
// });

// using Microsoft.AspNetCore.Identity;

// builder.Services.Configure<PasswordHasherOptions>(option =>
// {
//     option.IterationCount = 12000;
// });

// namespace vmProjectBFF.Controllers
// {
//     [Authorize]
//     [Route("api/[controller]")]
//     [ApiController]
//     public class UserController : BffController
//     {
//                [HttpPost]
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