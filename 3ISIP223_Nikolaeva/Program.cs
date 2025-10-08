using System;
using System.Collections.Generic;

namespace _3ISIP223_Nikolaeva
{
    class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public char Gender { get; set; }

        public Person(int id, string name, DateOnly date, char gender)
        {
            ID = id;
            Name = name;
            DateOfBirth = date;
            Gender = gender;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"ID: {ID}, ФИО: {Name}, Дата рождения: {DateOfBirth}, Пол: {Gender}");
        }
    }

    class Student : Person
    {
        public static int NextID = 1;
        public List<Course> Courses { get; set; }

        public Student(string name, DateOnly date, char gender)
            : base(NextID, name, date, gender)
        {
            NextID++;
            Courses = new List<Course>();
        }

        public void AddCourse(Course course)
        {
            if (!Courses.Contains(course))
            {
                Courses.Add(course);
                course.AddStudent(this);
                Console.WriteLine($"Студент {Name} записан на курс {course.Name}");
            }
        }

        public void PrintCourses()
        {
            if (Courses.Count == 0)
            {
                Console.WriteLine($"{Name} не записан на курсы");
                return;
            }

            Console.WriteLine($"Курсы студента {Name}:");
            for (int i = 0; i < Courses.Count; i++)
            {
                Console.WriteLine($"- {Courses[i].Name}");
            }
        }
    }

    class Teacher : Person
    {
        public static int NextID = 1;
        public int Cabinet { get; set; }
        public List<Course> Courses { get; set; }

        public Teacher(string name, DateOnly date, char gender, int cabinet)
            : base(NextID, name, date, gender)
        {
            NextID++;
            Cabinet = cabinet;
            Courses = new List<Course>();
        }

        public void AddCourse(Course course)
        {
            if (!Courses.Contains(course))
            {
                Courses.Add(course);
                course.Teacher = this;
                Console.WriteLine($"Преподаватель {Name} назначен на курс {course.Name}");
            }
        }

        public void PrintCourses()
        {
            if (Courses.Count == 0)
            {
                Console.WriteLine($"{Name} не ведет курсы");
                return;
            }

            Console.WriteLine($"Курсы преподавателя {Name}:");
            for (int i = 0; i < Courses.Count; i++)
            {
                Console.WriteLine($"- {Courses[i].Name}");
            }
        }
    }

    class Course
    {
        public string Name { get; set; }
        public List<Student> Students { get; set; }
        public Teacher Teacher { get; set; }

        public Course(string name)
        {
            Name = name;
            Students = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                Students.Add(student);
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Курс: {Name}");
            if (Teacher != null)
            {
                Console.WriteLine($"Преподаватель: {Teacher.Name}");
            }
            else
            {
                Console.WriteLine($"Преподаватель: Не назначен");
            }
            Console.WriteLine($"Студентов: {Students.Count}");
        }

        public void PrintStudents()
        {
            if (Students.Count == 0)
            {
                Console.WriteLine($"На курсе {Name} нет студентов");
                return;
            }

            Console.WriteLine($"Студенты курса {Name}:");
            for (int i = 0; i < Students.Count; i++)
            {
                Console.WriteLine($"- {Students[i].Name}");
            }
        }
    }

    class University
    {
        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }

        public University()
        {
            Students = new List<Student>();
            Teachers = new List<Teacher>();
            Courses = new List<Course>();
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        public void AddTeacher(Teacher teacher)
        {
            Teachers.Add(teacher);
        }

        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }

        public Student FindStudentById(int id)
        {
            for (int i = 0; i < Students.Count; i++)
            {
                if (Students[i].ID == id)
                {
                    return Students[i];
                }
            }
            return null;
        }

        public Teacher FindTeacherById(int id)
        {
            for (int i = 0; i < Teachers.Count; i++)
            {
                if (Teachers[i].ID == id)
                {
                    return Teachers[i];
                }
            }
            return null;
        }

        public Course FindCourseByName(string name)
        {
            for (int i = 0; i < Courses.Count; i++)
            {
                if (Courses[i].Name == name)
                {
                    return Courses[i];
                }
            }
            return null;
        }

        public void PrintAllStudents()
        {
            Console.WriteLine("СПИСОК СТУДЕНТОВ:");
            if (Students.Count == 0)
            {
                Console.WriteLine("Нет студентов");
                return;
            }

            for (int i = 0; i < Students.Count; i++)
            {
                Students[i].PrintInfo();
            }
        }

        public void PrintAllTeachers()
        {
            Console.WriteLine("СПИСОК ПРЕПОДАВАТЕЛЕЙ:");
            if (Teachers.Count == 0)
            {
                Console.WriteLine("Нет преподавателей");
                return;
            }

            for (int i = 0; i < Teachers.Count; i++)
            {
                Teachers[i].PrintInfo();
                Console.WriteLine($"Кабинет: {Teachers[i].Cabinet}");
            }
        }

        public void PrintAllCourses()
        {
            Console.WriteLine("СПИСОК КУРСОВ:");
            if (Courses.Count == 0)
            {
                Console.WriteLine("Нет курсов");
                return;
            }

            for (int i = 0; i < Courses.Count; i++)
            {
                Courses[i].PrintInfo();
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static University university = new University();

        static void Main(string[] args)
        {
            AddSampleData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ ===");
                Console.WriteLine("1. Управление студентами");
                Console.WriteLine("2. Управление преподавателями");
                Console.WriteLine("3. Управление курсами");
                Console.WriteLine("4. Показать всю информацию");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StudentMenu();
                        break;
                    case "2":
                        TeacherMenu();
                        break;
                    case "3":
                        CourseMenu();
                        break;
                    case "4":
                        ShowAllInfo();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddSampleData()
        {
            Teacher teacher1 = new Teacher("Иван Петров", new DateOnly(1980, 5, 15), 'М', 101);
            Teacher teacher2 = new Teacher("Мария Сидорова", new DateOnly(1975, 8, 22), 'Ж', 102);

            Student student1 = new Student("Алексей Иванов", new DateOnly(2000, 3, 10), 'М');
            Student student2 = new Student("Елена Козлова", new DateOnly(2001, 7, 5), 'Ж');

            Course course1 = new Course("Программирование на C#");
            Course course2 = new Course("Базы данных");

            university.AddTeacher(teacher1);
            university.AddTeacher(teacher2);
            university.AddStudent(student1);
            university.AddStudent(student2);
            university.AddCourse(course1);
            university.AddCourse(course2);

            teacher1.AddCourse(course1);
            teacher2.AddCourse(course2);
            student1.AddCourse(course1);
            student2.AddCourse(course2);
        }

        static void StudentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ СТУДЕНТАМИ ===");
                Console.WriteLine("1. Показать всех студентов");
                Console.WriteLine("2. Добавить студента");
                Console.WriteLine("3. Записать студента на курс");
                Console.WriteLine("4. Показать курсы студента");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        university.PrintAllStudents();
                        WaitForKey();
                        break;
                    case "2":
                        AddStudent();
                        break;
                    case "3":
                        EnrollStudent();
                        break;
                    case "4":
                        ShowStudentCourses();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        WaitForKey();
                        break;
                }
            }
        }

        static void TeacherMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ ПРЕПОДАВАТЕЛЯМИ ===");
                Console.WriteLine("1. Показать всех преподавателей");
                Console.WriteLine("2. Добавить преподавателя");
                Console.WriteLine("3. Назначить преподавателя на курс");
                Console.WriteLine("4. Показать курсы преподавателя");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        university.PrintAllTeachers();
                        WaitForKey();
                        break;
                    case "2":
                        AddTeacher();
                        break;
                    case "3":
                        AssignTeacher();
                        break;
                    case "4":
                        ShowTeacherCourses();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        WaitForKey();
                        break;
                }
            }
        }

        static void CourseMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ КУРСАМИ ===");
                Console.WriteLine("1. Показать все курсы");
                Console.WriteLine("2. Добавить курс");
                Console.WriteLine("3. Показать студентов курса");
                Console.WriteLine("4. Назад");
                Console.Write("Выберите пункт: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        university.PrintAllCourses();
                        WaitForKey();
                        break;
                    case "2":
                        AddCourse();
                        break;
                    case "3":
                        ShowCourseStudents();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        WaitForKey();
                        break;
                }
            }
        }

        static void ShowAllInfo()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЯ ИНФОРМАЦИЯ ===");
            university.PrintAllStudents();
            Console.WriteLine();
            university.PrintAllTeachers();
            Console.WriteLine();
            university.PrintAllCourses();
            WaitForKey();
        }

        static void AddStudent()
        {
            Console.Clear();
            Console.WriteLine("ДОБАВЛЕНИЕ СТУДЕНТА");
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Введите месяц рождения: ");
            int month = int.Parse(Console.ReadLine());
            Console.Write("Введите день рождения: ");
            int day = int.Parse(Console.ReadLine());
            Console.Write("Введите пол (М/Ж): ");
            char gender = Console.ReadLine()[0];

            Student student = new Student(name, new DateOnly(year, month, day), gender);
            university.AddStudent(student);
            Console.WriteLine($"Студент {name} добавлен! ID: {student.ID}");
            WaitForKey();
        }

        static void AddTeacher()
        {
            Console.Clear();
            Console.WriteLine("ДОБАВЛЕНИЕ ПРЕПОДАВАТЕЛЯ");
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Введите месяц рождения: ");
            int month = int.Parse(Console.ReadLine());
            Console.Write("Введите день рождения: ");
            int day = int.Parse(Console.ReadLine());
            Console.Write("Введите пол (М/Ж): ");
            char gender = Console.ReadLine()[0];
            Console.Write("Введите номер кабинета: ");
            int cabinet = int.Parse(Console.ReadLine());

            Teacher teacher = new Teacher(name, new DateOnly(year, month, day), gender, cabinet);
            university.AddTeacher(teacher);
            Console.WriteLine($"Преподаватель {name} добавлен! ID: {teacher.ID}");
            WaitForKey();
        }

        static void AddCourse()
        {
            Console.Clear();
            Console.WriteLine("ДОБАВЛЕНИЕ КУРСА");
            Console.Write("Введите название курса: ");
            string name = Console.ReadLine();

            Course course = new Course(name);
            university.AddCourse(course);
            Console.WriteLine($"Курс {name} добавлен!");
            WaitForKey();
        }

        static void EnrollStudent()
        {
            Console.Clear();
            Console.WriteLine("ЗАПИСЬ СТУДЕНТА НА КУРС");
            university.PrintAllStudents();
            Console.Write("Введите ID студента: ");
            int studentId = int.Parse(Console.ReadLine());

            university.PrintAllCourses();
            Console.Write("Введите название курса: ");
            string courseName = Console.ReadLine();

            Student student = university.FindStudentById(studentId);
            Course course = university.FindCourseByName(courseName);

            if (student != null && course != null)
            {
                student.AddCourse(course);
            }
            else
            {
                Console.WriteLine("Студент или курс не найден!");
            }
            WaitForKey();
        }

        static void AssignTeacher()
        {
            Console.Clear();
            Console.WriteLine("НАЗНАЧЕНИЕ ПРЕПОДАВАТЕЛЯ НА КУРС");
            university.PrintAllTeachers();
            Console.Write("Введите ID преподавателя: ");
            int teacherId = int.Parse(Console.ReadLine());

            university.PrintAllCourses();
            Console.Write("Введите название курса: ");
            string courseName = Console.ReadLine();

            Teacher teacher = university.FindTeacherById(teacherId);
            Course course = university.FindCourseByName(courseName);

            if (teacher != null && course != null)
            {
                teacher.AddCourse(course);
            }
            else
            {
                Console.WriteLine("Преподаватель или курс не найден!");
            }
            WaitForKey();
        }

        static void ShowStudentCourses()
        {
            Console.Clear();
            Console.WriteLine("КУРСЫ СТУДЕНТА");
            university.PrintAllStudents();
            Console.Write("Введите ID студента: ");
            int studentId = int.Parse(Console.ReadLine());

            Student student = university.FindStudentById(studentId);
            if (student != null)
            {
                student.PrintCourses();
            }
            else
            {
                Console.WriteLine("Студент не найден!");
            }
            WaitForKey();
        }

        static void ShowTeacherCourses()
        {
            Console.Clear();
            Console.WriteLine("КУРСЫ ПРЕПОДАВАТЕЛЯ");
            university.PrintAllTeachers();
            Console.Write("Введите ID преподавателя: ");
            int teacherId = int.Parse(Console.ReadLine());

            Teacher teacher = university.FindTeacherById(teacherId);
            if (teacher != null)
            {
                teacher.PrintCourses();
            }
            else
            {
                Console.WriteLine("Преподаватель не найден!");
            }
            WaitForKey();
        }

        static void ShowCourseStudents()
        {
            Console.Clear();
            Console.WriteLine("СТУДЕНТЫ КУРСА");
            university.PrintAllCourses();
            Console.Write("Введите название курса: ");
            string courseName = Console.ReadLine();

            Course course = university.FindCourseByName(courseName);
            if (course != null)
            {
                course.PrintStudents();
            }
            else
            {
                Console.WriteLine("Курс не найден!");
            }
            WaitForKey();
        }

        static void WaitForKey()
        {
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}