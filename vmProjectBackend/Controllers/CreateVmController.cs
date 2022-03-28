<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
>>>>>>> auth-ebe
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using vmProjectBackend.DTO;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
<<<<<<< HEAD
=======
using vmProjectBackend.Services;
>>>>>>> auth-ebe
using Microsoft.Extensions.Configuration;

namespace vmProjectBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CreateVmController : ControllerBase
    {
<<<<<<< HEAD
        private readonly VmContext _context;
        public IConfiguration Configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;
        public CreateVmController(VmContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;

=======
        private readonly DatabaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Authorization _auth;
        public IConfiguration Configuration { get; }
        public CreateVmController(DatabaseContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _auth = new Authorization(_context);
>>>>>>> auth-ebe
            Configuration = configuration;
        }

        [HttpGet("libraries")]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            //Open uri communication
            var httpClient = _httpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
            if (tokenResponse.IsSuccessStatusCode)
            {
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();

                //Scape characters functions to filter the new header results
<<<<<<< HEAD
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
=======
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
>>>>>>> auth-ebe
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                //Make a list of Library objects
                List<Library> libraries = new List<Library>();
                //Call library uri in vsphere
                var responseLibraryIds = await httpClient.GetAsync("https://vctr-dev.citwdd.net/api/content/local-library");
                //Turn these objects responses into a readable string
                string responseStringLibraries = await responseLibraryIds.Content.ReadAsStringAsync();
                //Make a list of library id's   
                List<String> libraryIds = JsonConvert.DeserializeObject<List<String>>(responseStringLibraries);
                //Create a list using our Dto          
                foreach (string libraryId in libraryIds)
                {
                    // return Ok(libraryIds);
                    var libraryresponse = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/local-library/" + libraryId);
                    string response2String = await libraryresponse.Content.ReadAsStringAsync();
                    Library library = JsonConvert.DeserializeObject<Library>(response2String);
<<<<<<< HEAD
                    libraries.Add(library);                   
=======
                    libraries.Add(library);
>>>>>>> auth-ebe
                }
                return Ok(libraries);
                // if (libraries != null)
                // {
<<<<<<< HEAD
                    
=======

>>>>>>> auth-ebe
                //     httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                //     httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

                //     var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
                //     return Ok("Session was deleted here");
                // }
            }
            return Unauthorized("here");
<<<<<<< HEAD
        } 

         [HttpGet("folders")]
         public async Task<ActionResult<IEnumerable<Folder>>> GetFolders()
         {
=======
        }

        [HttpGet("folders")]
        public async Task<ActionResult<IEnumerable<Folder>>> GetFolders()
        {
>>>>>>> auth-ebe
            //Open uri communication
            var httpClient = _httpClientFactory.CreateClient();
            // Basic authentication in base64
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
            if (tokenResponse.IsSuccessStatusCode)
            {
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();

                //Scape characters functions to filter the new header results
<<<<<<< HEAD
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
=======
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
>>>>>>> auth-ebe
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                //Make a list of Library object
                //Call library uri in vsphere
                var responseFolders = await httpClient.GetAsync("https://vctr-dev.citwdd.net/rest/vcenter/folder?filter.type=VIRTUAL_MACHINE");
                //Turn these objects responses into a readable string
<<<<<<< HEAD
            
                string folderResponseString = await responseFolders.Content.ReadAsStringAsync();
                                              
                FolderResponse folderResponse = JsonConvert.DeserializeObject<FolderResponse>(folderResponseString);
                // Create an empty list to save the results we need
                List<Folder> folders = new List<Folder>();
                 //declare variable from configuration (appsettings.json)
                string ignoreFolder = Configuration["IgnoreFolder"];
                foreach ( Folder folder in folderResponse.value) {
                    if (  folder.name != ignoreFolder)
                   // filtering all folders except DO-NOT-USE 
                    folders.Add(folder);
                }
                // list result
                return Ok(folders);
            }
            return Unauthorized("here");
         }

         [HttpGet("templates/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetTemplates(string Id) 
        {
            //Create link to this call
            string useremail = HttpContext.User.Identity.Name;
            //check if it is a professor
            var user_prof = _context.Users
                              .Where(p => p.email == useremail && p.userType == "Professor")
                              .FirstOrDefault();
            //this happens if the user is a professor
            //  return Ok(user_prof);
             if (user_prof != null)
             {
                 // Creating the client request and setting headers to the request
            var httpClient = _httpClientFactory.CreateClient(); 
            string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
            //Adding headers
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);          
            string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "" );
                tokenstring = tokenstring.Replace("{", "" );
                tokenstring = tokenstring.Replace("value:", "" );
                tokenstring = tokenstring.Replace("}", "" );
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                    List<Template> templates = new List<Template>();
                // 
                    var response = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item?library_id={Id}");
                    string responseString = await response.Content.ReadAsStringAsync();
                    // return Ok(responseString);
                    List<String> templateIds = templateIds = JsonConvert.DeserializeObject<List<String>>(responseString);
                    foreach (string templateId in templateIds)
                    {
                        var response2 = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item/" + templateId);
                        Console.WriteLine($"Second response {response2}");                       
                        string response2String = await response2.Content.ReadAsStringAsync();
                        Template template = JsonConvert.DeserializeObject<Template>(response2String);       
                        templates.Add(template);
                    }
                    return Ok(templates);
        }  
        return Unauthorized();    
            //[HttpDelete]     

        // public async Task<ActionResult<IEnumerable<Library>>> DeleteSession()
        // {
        // var httpClient = _httpClientFactory.CreateClient();

        // string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
        // // Adding headers to this call
        // httpClient.DefaultRequestHeaders.Add("Authorization", base64);
        // httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");

        // var deleteResponse = await httpClient.DeleteAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session");
        // return Ok("Session was deleted here");
        // }
        }
    }
}
=======
                //
                string folderResponseString = await responseFolders.Content.ReadAsStringAsync();


                FolderResponse folderResponse = JsonConvert.DeserializeObject<FolderResponse>(folderResponseString);
                List<Folder> folders = new List<Folder>();

                //declare variable from configuration (appsettings.json)
                string ignoreFolder = Configuration["IgnoreFolder"];


                foreach (Folder folder in folderResponse.value)
                {
                    if (folder.name != ignoreFolder)
                        // string response2String = await response2.Content.ReadAsStringAsync();
                        // Folder folder = JsonConvert.DeserializeObject<Folder>(response2String);
                        folders.Add(folder);
                }

                return Ok(folders);
            }
            return Unauthorized("here");
        }

        [HttpGet("templates/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetTemplates(string Id)
        {
            string userEmail = HttpContext.User.Identity.Name;
            //check if it is a professor
            User professorUser = _auth.getAdmin(userEmail);

            if (professorUser != null)
            {
                // Creating the client request and setting headers to the request
                var httpClient = _httpClientFactory.CreateClient();
                string base64 = "Basic YXBpLXRlc3RAdnNwaGVyZS5sb2NhbDp3bkQ8RHpbSFpXQDI1e11x";
                //Adding headers
                httpClient.DefaultRequestHeaders.Add("Authorization", base64);
                var tokenResponse = await httpClient.PostAsync("https://vctr-dev.citwdd.net/rest/com/vmware/cis/session", null);
                string tokenstring = " ";
                //Turn this object into a readable string
                tokenstring = await tokenResponse.Content.ReadAsStringAsync();
                //Scape characters functions to filter the new header results
                tokenstring = tokenstring.Replace("\"", "");
                tokenstring = tokenstring.Replace("{", "");
                tokenstring = tokenstring.Replace("value:", "");
                tokenstring = tokenstring.Replace("}", "");
                httpClient.DefaultRequestHeaders.Add("Cookie", $"vmware-api-session-id={tokenstring}");
                List<Template> templates = new List<Template>();
                // 
                var response = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item?library_id={Id}");
                string responseString = await response.Content.ReadAsStringAsync();
                // return Ok(responseString);
                List<String> templateIds = templateIds = JsonConvert.DeserializeObject<List<String>>(responseString);
                foreach (string templateId in templateIds)
                {
                    var response2 = await httpClient.GetAsync($"https://vctr-dev.citwdd.net/api/content/library/item/" + templateId);
                    Console.WriteLine($"Second response {response2}");
                    string response2String = await response2.Content.ReadAsStringAsync();
                    Template template = JsonConvert.DeserializeObject<Template>(response2String);
                    templates.Add(template);
                }
                return Ok(templates);
            }
            return Unauthorized();
        }
    }
}
>>>>>>> auth-ebe
