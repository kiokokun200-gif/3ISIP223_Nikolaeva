using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP223_Nikolaeva
{
    class Person
    {
        public int ID;
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public char Gender { get; set; }

        public Person(string name, DateOnly date, char gender) 
        {
            Name = name;
            DateOfBirth = date;
            Gender = gender;
        }


        public virtual void PrintInfo()
        {
            Console.WriteLine($"ID: {ID}, ФИО:  {Name}, Дата рождения: {DateOfBirth}, Пол: {Gender}, ");
        }

    }
 

    class Student : Person 
    {
        public static int NextID = 1;

        //public List<int> Marks {  get; set; }
        public List<Course> courses { get; set; }


        public Student(int ID, string name, DateOnly date, char gender ) : base( name, date, gender )  
        {
            ID = NextID;
            NextID++;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            if (courses != null)
            {
                foreach (var course in courses)
                {
                    ConsoleWriteLine(course.Name);

                }
            }
            else {
                ConsoleWriteLine("Нет курсов");
            }
        }

        public void PrintCourses()
        {
            if (courses != null)
            {
                foreach (var course in courses)
                {
                    ConsoleWriteLine(course.Name);

                }
            }
            else
            {
                ConsoleWriteLine("Нет курсов");
            }
        }



    }

    class Teacher : Person
    {
        public static int NextID = 1;
        public int Cabinet { get; set; }
        public List<Course> courses { get; set; }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.Write($"Кабинет: {Cabinet}, ");
            Console.Write("Курсы: ");
            if (courses != null)
            {
                foreach (var course in courses)
                {
                    Console.Write($"{ course.Name} ");
                }
            }

            else
            {
                Console.Write("Нет курсов");
            }

        }


    }

    class Courses
    {
        public string Name { get; set; }
        public List <Student> Students { get; set; }
        = new List<Student>();
        public int student_count;
        

    }



    class University
    {
        List<Student> students = new List<Student>();
        List<Teacher> teachers = new List<Teacher>();
        List<Courses> courses = new List<Courses>();
    }


        internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
