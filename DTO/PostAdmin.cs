﻿// this model is not link to the Database, it is just used to recieve data from post methods [HttpPost("professor/checkCanvasToken")]
namespace vmProjectBFF.Models
{
    public class PostAdmin
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string userType { get; set; }
        public bool userAccess { get; set; }
    }
}