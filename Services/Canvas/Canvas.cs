using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;

namespace vmProjectBFF.Services
{
    public class Canvas
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Canvas(ILogger logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


        /****************************************
        Given an email returns either a professor user or null if the email doesn't belong to a professor
        ****************************************/
        public async Task<dynamic>  GetCourses(string canvasToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            string base64 = $"Bearer {canvasToken}";
            httpClient.DefaultRequestHeaders.Add("Authorization", base64);
            var tokenResponse = await httpClient.GetAsync("https://byui.test.instructure.com/api/v1/courses");

            if (tokenResponse.IsSuccessStatusCode) {
                dynamic courses = JsonConvert.DeserializeObject<dynamic>(await tokenResponse.Content.ReadAsStringAsync());
                return courses;
            } else {
                throw new System.Exception("Something went wrong with canvas");
            }

        }
    }
}
