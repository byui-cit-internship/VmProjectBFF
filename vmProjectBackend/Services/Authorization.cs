using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Services
{
    public class Authorization
    {
        private readonly DatabaseContext _context;

        ILogger Logger { get; } = AppLogger.CreateLogger<Authorization>();

        public Authorization(DatabaseContext context)
        {
            _context = context;
        }

        public User getProfessor(string emailToTest)
        {
            IQueryable<User> professors = from u in _context.Users
                                          join usr in _context.UserSectionRoles
                                          on u.UserId equals usr.UserId
                                          join r in _context.Roles
                                          on usr.RoleId equals r.RoleId
                                          where r.RoleName == "Professor"
                                          && u.Email == emailToTest
                                          select u;
            return professors.FirstOrDefault();
        }

        public User getUser(string emailToTest)
        {
            IQueryable<User> users = from u in _context.Users
                                     join usr in _context.UserSectionRoles
                                     on u.UserId equals usr.UserId
                                     join r in _context.Roles
                                     on usr.RoleId equals r.RoleId
                                     where r.RoleName == "Student"
                                     && u.Email == emailToTest
                                     select u;
            return users.FirstOrDefault();
        }

        public User getAdmin(string emailToTest)
        {
            IQueryable<User> admins = from u in _context.Users
                                      where u.IsAdmin
                                      select u;
            return admins.FirstOrDefault();
        }
    }
}
