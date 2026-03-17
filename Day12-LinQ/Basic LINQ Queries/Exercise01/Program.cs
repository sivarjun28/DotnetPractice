using System;
namespace Exercise01
{


    /*
Write LINQ queries for:
1. All students with GPA > 3.5
2. All Computer Science students
3. Names of all students (string list)
4. Students ordered by GPA descending
5. Average GPA of all students
6. Count of students in each major
7. Oldest student
8. Top 3 students by GPA
9. Students aged 21 or 22
10. All unique majors

Implement each query with BOTH query syntax and method syntax.
*/
    internal class Program
    {

        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; } = " ";
            public int Age { get; set; }
            public string Major { get; set; } = string.Empty;
            public double GPA { get; set; }

        }
        static void Main(string[] args)
        {
            List<Student> students = new()
{
    new Student { Id = 1, Name = "Alice", Age = 20, Major = "Computer Science", GPA = 3.8 },
    new Student { Id = 2, Name = "Bob", Age = 22, Major = "Mathematics", GPA = 3.5 },
    new Student { Id = 3, Name = "Charlie", Age = 21, Major = "Computer Science", GPA = 3.9 },
    new Student { Id = 4, Name = "David", Age = 23, Major = "Physics", GPA = 3.2 },
    new Student { Id = 5, Name = "Eve", Age = 20, Major = "Mathematics", GPA = 3.7 },
    new Student { Id = 6, Name = "Frank", Age = 22, Major = "Computer Science", GPA = 3.4 },
    new Student { Id = 7, Name = "Grace", Age = 21, Major = "Physics", GPA = 3.6 }
};
            //Query 1 - GPA > 3.5
            //method syntax
            var highGPA = students.Where(s => s.GPA > 3.5);

            //QuerySyntax
            var highGPA1 = from s in students
                           where s.GPA > 3.5
                           select s;
            System.Console.WriteLine("Students with GPA > 3.5");
            foreach (var student in highGPA)
            {
                System.Console.WriteLine($"{student.Name} : {student.GPA}");
            }
            // Query 2 - Computer Science students
            var csStudents = students.Where(s => s.Major == "Computer Science");
            var csStudents1 = from s in students
                              where s.Major == "Computer Science"
                              select s;
            System.Console.WriteLine("All computer science students");
            foreach (var student in csStudents)
            {
                System.Console.WriteLine($"{student.Name} : {student.Major}");
            }
            // Query 3 - Names only
            var names = students.Select(s => s.Name);
            var names1 = from s in students
                         select s.Name;
            System.Console.WriteLine("Names of students");
            foreach (var name in names1)
            {
                System.Console.WriteLine($"{name}");
            }

            // Query 4 - Ordered by GPA
            var orderBy = students.OrderBy(s => s.GPA);
            var orderBy1 = from s in students
                           orderby s.GPA
                           select s;
            System.Console.WriteLine("Order by CGPA");
            foreach (var order in orderBy1)
            {
                System.Console.WriteLine($"{order.Name} : {order.GPA}");
            }

            // TODO: Query 5 - Average GPA
            double avgGPA = students.Average(s => s.GPA);
            System.Console.WriteLine($"Average GPA is {avgGPA:F2}");

            double avgGPA1 = (from s in students
                              select s.GPA).Average();
            System.Console.WriteLine($"Average GPA is {avgGPA1:F2}");

            // TODO: Query 6 - Count per major
            var counyByMajor = students.
                                GroupBy(s => s.Major)
                                .Select(g => new { Major = g.Key, Count = g.Count() });
            var counyByMajor1 = from s in students
                                group s by s.Major into g
                                select new
                                {
                                    Major = g.Key,
                                    Count = g.Count()
                                };
            System.Console.WriteLine("Count per major ");
            foreach (var item in counyByMajor1)
            {
                System.Console.WriteLine($"{item.Major} : {item.Count}");
            }

            //Query 7 - Oldest student
            var oldestStudent = students.
                                OrderByDescending(s => s.Age)
                                .FirstOrDefault();
            var oldestStudent1 = (from s in students
                                  orderby s.Age descending
                                  select s).FirstOrDefault();
            if (oldestStudent1 != null)
            {
                System.Console.WriteLine($"Oldest: {oldestStudent1.Name}");
            }

            // TODO: Query 8 - Top 3 by GPA
            var top3ByGPA = students.
                            OrderByDescending(s => s.GPA)
                            .Take(3);
            var top3ByGPA1 = (from s in students
                              orderby s.GPA descending
                              select s).Take(3);
            System.Console.WriteLine("Top 3 CGPA");
            foreach (var item in top3ByGPA1)
            {
                System.Console.WriteLine($"{item.Name}:{item.GPA:F2}");
            }
            // TODO: Query 9 - Age 21 or 22
            var age = students.
                        Where(s => s.Age == 21 || s.Age == 22);
            var age1 = from s in students
                       where s.Age == 21 || s.Age == 22
                       select s;
            System.Console.WriteLine("Age contains 21 and 22");
            foreach (var item in age1)
            {
                System.Console.WriteLine($"{item.Name} : {item.Age}");
            }
            // TODO: Query 10 - Unique majors
            var unique = students.
                            Select(s => s.Major)
                            .Distinct();
            var unique1 = (
                from s in students
                select s.Major).Distinct();
            System.Console.WriteLine("Distinct(unique) majors");
            foreach (var item in unique1)
            {
                System.Console.WriteLine(item);
            }




        }

    }
}
