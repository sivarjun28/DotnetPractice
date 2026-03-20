using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Exercise03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load schools from JSON
            List<School> schools = School.GenerateSchools();


            Console.WriteLine("1️⃣ All departments across all schools:");
            var allDepartments = schools.SelectMany(s => s.Departments);

            foreach (var dept in allDepartments)
            {
                // Get course names as a comma-separated string
                var courseNames = string.Join(", ", dept.Courses.Select(c => c.Name));
                Console.WriteLine($"- {dept.Name} : {courseNames}");
            }
            Console.WriteLine();

            Console.WriteLine("2️⃣ All courses across all departments and schools:");
            var allCourses = allDepartments.SelectMany(d => d.Courses);
            foreach (var course in allCourses)
                Console.WriteLine($"- {course.Name}");
            Console.WriteLine();

            Console.WriteLine("3️⃣ All students enrolled in any course:");
            var allStudents = allCourses.SelectMany(c => c.Students).DistinctBy(s => s.Name);
            foreach (var student in allStudents)
                Console.WriteLine($"- {student.Name}");
            Console.WriteLine();

            Console.WriteLine("4️⃣ Courses with department and school information:");
            var coursesWithInfo = schools.SelectMany(s => s.Departments,
                (s, d) => new { School = s, Dept = d })
                .SelectMany(sd => sd.Dept.Courses,
                    (sd, c) => new { sd.School, sd.Dept, Course = c });
            foreach (var item in coursesWithInfo)
                Console.WriteLine($"- {item.Course.Name} (Dept: {item.Dept.Name}, School: {item.School.Name})");
            Console.WriteLine();

            Console.WriteLine("5️⃣ Students with their course and department info:");
            var studentsWithInfo = coursesWithInfo.SelectMany(ci => ci.Course.Students,
                (ci, s) => new { Student = s, Course = ci.Course, Department = ci.Dept, School = ci.School });
            foreach (var item in studentsWithInfo)
                Console.WriteLine($"- {item.Student.Name} (Course: {item.Course.Name}, Dept: {item.Department.Name}, School: {item.School.Name})");
            Console.WriteLine();

            Console.WriteLine("6️⃣ Total credits per school:");
            foreach (var school in schools)
            {
                int totalCredits = school.Departments.SelectMany(d => d.Courses).Sum(c => c.Credits);
                Console.WriteLine($"- {school.Name}: {totalCredits} credits");
            }
            Console.WriteLine();

            Console.WriteLine("7️⃣ Flattened list of all courses with enrollment count:");
            foreach (var course in allCourses)
            {
                Console.WriteLine($"- {course.Name}: {course.Students.Count} students");
            }
            Console.WriteLine();

            Console.WriteLine("8️⃣ Cross-product of departments and courses:");
            foreach (var dept in allDepartments)
            {
                foreach (var course in allCourses)
                {
                    Console.WriteLine($"- Dept: {dept.Name}, Course: {course.Name}");
                }
            }
            Console.WriteLine();


        }
    }

    public class School
    {
        public string Name { get; set; } = string.Empty;
        public List<Department> Departments { get; set; } = new();

        public static List<School> GenerateSchools()
        {
            return new List<School>
    {
        new School
        {
            Name = "School A",
            Departments = new List<Department>
            {
                new Department
                {
                    Name = "Computer Science",
                    Courses = new List<Course>
                    {
                        new Course
                        {
                            Name = "Algorithms",
                            Credits = 4,
                            Students = new List<Student>
                            {
                                new Student { Name = "Alice" },
                                new Student { Name = "Bob" }
                            }
                        },
                        new Course
                        {
                            Name = "Data Structures",
                            Credits = 3,
                            Students = new List<Student>
                            {
                                new Student { Name = "Charlie" }
                            }
                        }
                    }
                }
            }
        },
        new School
        {
            Name = "School B",
            Departments = new List<Department>
            {
                new Department
                {
                    Name = "Physics",
                    Courses = new List<Course>
                    {
                        new Course
                        {
                            Name = "Mechanics",
                            Credits = 3,
                            Students = new List<Student>
                            {
                                new Student { Name = "Eve" },
                                new Student { Name = "Frank" }
                            }
                        }
                    }
                }
            }
        }
    };
        }

    }

    public class Department
    {
        public string Name { get; set; } = string.Empty;
        public List<Course> Courses { get; set; } = new();
    }

    public class Course
    {
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public List<Student> Students { get; set; } = new();
    }

    public class Student
    {
        public string Name { get; set; } = string.Empty;
    }
}