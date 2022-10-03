﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vmProjectBFF.Models;
using Microsoft.AspNetCore.Authorization;
using vmProjectBFF.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using vmProjectBFF.DTO;
using Newtonsoft.Json;
using vmProjectBFF.Exceptions;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : BffController
    {

        public StudentCourseController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<StudentCourseController> logger)
            : base(
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger)
        {
        }

        //Student get to see all their classes that they are enrolled in
        // GET: api/StudentCourse
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            try
            {
                User student = _auth.getAuth("user");
                if (student != null)
                {
                    _lastResponse = _backend.Get($"api/v1/StudentCourse", new { queryUserId = student.UserId });
                    List<CourseListByUserDTO> courseList = JsonConvert.DeserializeObject<List<CourseListByUserDTO>>(_lastResponse.Response);

                    return Ok(courseList);
                }
                return Unauthorized("You are not an Authorized User");
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}