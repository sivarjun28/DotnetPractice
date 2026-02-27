using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise06
{
    internal class Student
    {
        public string Name { get; set; }
        public string ID { get; set; }
        private List<double> grades = new List<double>();

        public Student(string name, string id)
        {
            Name = name;
            ID = id;
        }

        public void AddGrade(double grade)
        {
            if (grade >= 0 && grade <= 100)
                grades.Add(grade);
            else
                Console.WriteLine("Grade must be 0-100");
        }

        public double GetAverage() => grades.Count > 0 ? grades.Average() : 0;

        public string GetLetterGrade() => GetAverage() switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };

        public void PrintReport()
        {
            Console.WriteLine($"\n=== Report: {Name} ({ID}) ===");
            Console.WriteLine($"Grades: {(grades.Count > 0 ? string.Join(", ", grades) : "No grades")}");
            Console.WriteLine($"Average: {GetAverage():F2}");
            Console.WriteLine($"Letter Grade: {GetLetterGrade()}");
        }
    }

    internal class StudentGradeSystem
    {
        static void Main()
        {
            List<Student> students = new List<Student>();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nMenu: 1.Add Student 2.Add Grades 3.Print Report 4.Compare 5.Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (hoice)
                {
                    case "1":
                        Console.Write("Name: "); string name = Console.ReadLine();
                        Console.Write("ID: "); string id = Console.ReadLine();
                        students.Add(new Student(name, id));
                        break;

                    case "2":
                        Student s = SelectStudent(students);
                        if (s != null)
                        {
                            Console.Write("Enter grades separated by space: ");
                            string[] input = Console.ReadLine().Split(' ');
                            foreach (var g in input)
                                if (double.TryParse(g, out double grade)) s.AddGrade(grade);
                        }
                        break;

                    case "3":
                        Student reportStudent = SelectStudent(students);
                        reportStudent?.PrintReport();
                        break;

                    case "4":
                        if (students.Count >= 2)
                        {
                            var top = students.OrderByDescending(st => st.GetAverage()).First();
                            var low = students.OrderBy(st => st.GetAverage()).First();
                            Console.WriteLine($"Top Student: {top.Name} ({top.GetAverage():F2})");
                            Console.WriteLine($"Lowest Student: {low.Name} ({low.GetAverage():F2})");
                        }
                        else Console.WriteLine("Add at least 2 students for comparison.");
                        break;

                    case "5":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }

        static Student SelectStudent(List<Student> students)
        {
            if (students.Count == 0) { Console.WriteLine("No students."); return null; }

            for (int i = 0; i < students.Count; i++)
                Console.WriteLine($"{i + 1}. {students[i].Name} ({students[i].ID})");

            Console.Write("Select student: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= students.Count)
                return students[choice - 1];

            Console.WriteLine("Invalid selection.");
            return null;
        }
    }
}