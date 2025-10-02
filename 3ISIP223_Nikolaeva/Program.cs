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


    }

    class Student : Person 
    {
        public static int NextID = 1;

        public List<int> Marks {  get; set; }
        public List<Course> courses { get; set; }


        public Student(int ID, string name, DateOnly date, char gender, )   
        {
            
        }

        public void PrintInfo()
        {

        }

        public void PrintCourses()
        {

        }



    }

    class Teacher : Person
    {
        public static int NextID = 1;
        public int Cabinet { get; set; }
        public List<Course> courses { get; set; }

        public void PrintInfo()
        {

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
