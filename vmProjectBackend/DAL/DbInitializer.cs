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
                    email="carson@gmail.com", userType="Student", userAccess= false, status= "Active"},

                new User { firstName = "Meredith",   lastName = "Alonso",
                    email="Alonso@gmail.com", userType="Student", userAccess= false, status= "Active"},
                new User { firstName = "Arturo",   lastName = "Anand",
                    email="carson@gmail.com", userType="Professor", userAccess= false, status= "Active"},
                new User { firstName = "Gytis",   lastName = "Barzdukas",
                    email="Barzdukas@gmail.com", userType="Professor", userAccess= false, status= "Active"},
                new User { firstName = "Peggy",   lastName = "Justice",
                    email="Justice@gmail.com", userType="Student", userAccess= false, status= "Active"},



            };

            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();



            //    creating data fro the courses
            var courses = new Course[]
            {
                new Course {CourseName="Cit 123" ,section_num="2", canvas_token= "12345fxb", description= "this is demo", semester="Fall" },
                new Course {CourseName="Cit 456" ,section_num="2", canvas_token= "12345fxb", description= "this is demo", semester="Fall" },
                new Course {CourseName="Cit 798" ,section_num="2", canvas_token= "12345fxb", description= "this is demo", semester="Winter" },
                new Course {CourseName="Cit 101" ,section_num="2", canvas_token= "12345fxb", description= "this is demo", semester="Spring" },
                new Course {CourseName="Cit 123" ,section_num="1", canvas_token= "12345fxb", description= "this is demo", semester="Fall" },
                new Course {CourseName="Cit 456" ,section_num="1", canvas_token= "12345fxb", description= "this is demo", semester="Winter" },
                new Course {CourseName="Cit 798" ,section_num="1", canvas_token= "12345fxb", description= "this is demo", semester="Fall" },
                new Course {CourseName="Cit 101" ,section_num="1", canvas_token= "12345fxb", description= "this is demo", semester="Winter" },
                new Course {CourseName="Cit 123" ,section_num="3", canvas_token= "12345fxb", description= "this is demo", semester="Spring" },
                new Course {CourseName="Cit 456" ,section_num="3", canvas_token= "12345fxb", description= "this is demo", semester="Winter" },
                new Course {CourseName="Cit 798" ,section_num="3", canvas_token= "12345fxb", description= "this is demo", semester="Fall" },
                new Course {CourseName="Cit 101" ,section_num="3", canvas_token= "12345fxb", description= "this is demo", semester="Spring" },
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
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456" && c.section_num=="2" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798" && c.section_num=="2" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alonso").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456" && c.section_num=="2" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Justice").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798" && c.section_num=="2" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101" && c.section_num=="2" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Anand").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101" && c.section_num=="2" ).CourseID,
                    },
                        new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456" && c.section_num=="3" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798" && c.section_num=="1" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alonso").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 456" && c.section_num=="1" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Justice").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 798" && c.section_num=="3" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Alexander").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101" && c.section_num=="1" ).CourseID,
                    },
                    new Enrollment {
                    UserId = users.Single(s => s.lastName == "Anand").UserID,
                    CourseID = courses.Single(c => c.CourseName == "Cit 101" && c.section_num=="1" ).CourseID,
                    },
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

            var vmTableCourses = new VmTableCourse[]
            {

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 123" && c.section_num=="2").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="windows 10 gb").VmTableID},


                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 456" && c.section_num=="2").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="Linux 10 gb").VmTableID},

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 101" && c.section_num=="2").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID},

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 798" && c.section_num=="3").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="Linux 10 gb").VmTableID},

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 123" && c.section_num=="1").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="windows 10 gb").VmTableID},

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 123" && c.section_num=="3").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID},

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 456" && c.section_num=="2").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID},

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 456" && c.section_num=="3").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="Linux 10 gb").VmTableID},

                new VmTableCourse{CourseID= courses.Single(c => c.CourseName== "Cit 101" && c.section_num=="3").CourseID,
                VmTableID= vmtables.Single(v => v.vm_image =="windows 5gb").VmTableID}


            };
            foreach (VmTableCourse t in vmTableCourses)
            {
                var vmTableCoursesInDataBase = context.VmTableCourse.Where(
                    s =>
                            s.VmTable.VmTableID == t.VmTableID &&
                            s.Course.CourseID == t.CourseID).SingleOrDefault();
                if (vmTableCoursesInDataBase == null)
                {
                    context.VmTableCourse.Add(t);
                }
            }
            context.SaveChanges();

        }
    }
}