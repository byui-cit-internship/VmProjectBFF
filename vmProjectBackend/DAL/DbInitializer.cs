using vmProjectBackend.Models;
using System;
using System.Linq;
namespace vmProjectBackend.DAL
{
    public class DbInitializer
    {
        public static void Initialize(VmContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User { firstName = "Carson",   lastName = "Alexander",
                    email="carson@gmail.com", userType="Student", userAccess= false},
                new User { firstName = "Tammy",   lastName = "Nolasco",
                    email="tnolasco54@gmail.com", userType="Student", userAccess= false},
                new User { firstName = "Meredith",   lastName = "Alonso",
                    email="Alonso@gmail.com", userType="Student", userAccess= false},
                new User { firstName = "Arturo",   lastName = "Anand",
                    email="carson@gmail.com", userType="Professor", userAccess= false},
                new User { firstName = "Gytis",   lastName = "Barzdukas",
                    email="Barzdukas@gmail.com", userType="Professor", userAccess= false},
                new User { firstName = "Peggy",   lastName = "Justice",
                    email="Justice@gmail.com", userType="Student", userAccess= false},

            };

            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
            //    creating data fro the courses
            var courses = new Course[]
            {
                new Course {CourseName="Cit 123" , canvas_token= "11345fxb", description= "this is demo"},
                new Course {CourseName="Cit 456" , canvas_token= "12345fxb", description= "this is demo"},
                new Course {CourseName="Cit 798" , canvas_token= "1345fxb", description= "this is demo" },
                new Course {CourseName="Cit 101" , canvas_token= "14345fxb", description= "this is demo" },
               
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            // create the enrollment based on 

            var enrollments = new Enrollment[]
            {
                new Enrollment {
                    UserId = users.Single(s => s.lastName == "Nolasco").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456").CourseID,
                    Status= "Active", section_num= "2", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798").CourseID,
                    Status= "Active", section_num= "2", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alonso").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456").CourseID,
                    Status= "Active", section_num= "2", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Justice").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798").CourseID,
                    Status= "Active", section_num= "2", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101").CourseID,
                    Status= "Active", section_num= "1", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Anand").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101").CourseID,
                    Status= "Active", section_num= "3", semester="Fall"
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Nolasco").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456").CourseID,
                    Status= "Active", section_num= "1", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798").CourseID,
                    Status= "Active", section_num= "3", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alonso").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456").CourseID,
                    Status= "Active", section_num= "2", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Justice").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798").CourseID,
                    Status= "Active", section_num= "5", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101").CourseID,
                    Status= "Active", section_num= "5", semester="Fall"
                    },

                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Nolasco").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101").CourseID,
                    Status= "Active", section_num= "4", semester="Fall"
                    }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                            s.User.UserID == e.UserId &&
                            s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();

            // create vm tables with data 
            var vmtables = new VmTable[]
            {
                new VmTable{vm_image="windows 10 gb"},
                new VmTable{vm_image="Linux 10 gb"},
                new VmTable{vm_image="windows 5gb"},

            };

            foreach (VmTable v in vmtables)
            {
                context.VmTables.Add(v);
            }
            context.SaveChanges();

            // create the link on the vm and cousres table

            // var vmTableCourses = new VmTableCourse[]
            // {

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 123".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="windows 10 gb").VmTableID},


            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 456".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="Linux 10 gb").VmTableID},

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 101".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID},

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 798".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="Linux 10 gb").VmTableID},

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 123".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="windows 10 gb").VmTableID},

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 123".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID},

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 456".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID},

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 456".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="Linux 10 gb").VmTableID},

            //     new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 101".CourseID,
            //     VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID}


            // };
            // foreach (VmTableCourse t in vmTableCourses)
            // {
            //     var vmTableCoursesInDataBase = context.VmTableCourse.Where(
            //         s =>
            //                 s.VmTable.VmTableID == t.VmTableID &&
            //                 s.Course.CourseID == t.CourseID).SingleOrDefault();
            //     if (vmTableCoursesInDataBase == null)
            //     {
            //         context.VmTableCourse.Add(t);
            //     }
            // }
            // context.SaveChanges();

        }
    }
}