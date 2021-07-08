using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TrainingCenter.Model;
using TrainingCenter.AdditionalClasses;

namespace TrainingCenter.DAL
{
    class SchoolInitializer : DropCreateDatabaseIfModelChanges<CenterContext>
    {
        protected override void Seed(CenterContext context)
        {
            var users = new List<Account>
            {
                new Account{Email="admin",Password=PasswordHasher.Hash("admin"),FirstName="admin",LastName="admin",
                    SignUpDate=new DateTime(2000,5,1,8,30,52), AdressStreet="adminowa",AdressNumber="123"
                    ,Country="Poland",City="Adminowo", AccountType= "Admin"},
                new Account{Email="a@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Marek",LastName="Wojecki",
                    SignUpDate=new DateTime(2001,2,4,8,12,24), AdressStreet="Mickiewicza",AdressNumber="12"
                    ,Country="Poland",City="Rzeszów", AccountType= "Teacher"},
                new Account{Email="b@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Wojciech",LastName="Torecki",
                    SignUpDate=new DateTime(2008,3,15,20,44,30), AdressStreet="Sikorskiego",AdressNumber="133/14"
                    ,Country="Poland",City="Rzeszów", AccountType= "Teacher"},
                new Account{Email="c@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Krzysztof",LastName="Perol",
                    SignUpDate=new DateTime(2014,1,12,20,20,15), AdressStreet="Akacjowa",AdressNumber="93"
                    ,Country="Poland",City="Rzeszów", AccountType= "Teacher"},
                new Account{Email="d@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="John",LastName="Smith",
                    SignUpDate=new DateTime(2012,10,22,10,1,30), AdressStreet="15th",AdressNumber="16"
                    ,Country="England",City="London", AccountType= "Student"},
                new Account{Email="e@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Vladimir",LastName="Putin",
                    SignUpDate=new DateTime(2016,1,22,10,1,30), AdressStreet="Tverskaya",AdressNumber="1"
                    ,Country="Russia",City="Moscow", AccountType= "Student"},
                new Account{Email="f@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Paweł",LastName="Kowalski",
                    SignUpDate=new DateTime(2006,6,12,20,44,30), AdressStreet="Brzoskwiniowa",AdressNumber="4/12"
                    ,Country="Poland",City="Kraków", AccountType= "Student"},
                new Account{Email="g@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Zbigniew",LastName="Adamski",
                    SignUpDate=new DateTime(2020,3,1,10,2,25), AdressStreet="Górksa",AdressNumber="99"
                    ,Country="Poland",City="Warszawa", AccountType= "Student"},
            };
            users.ForEach(s => context.Accounts.Add(s));

            var courses = new List<Course>
            {
                new Course{Title = "Kurs autoCAD", LeadingTeacher=users[1],Status="In progress",
                    Description="Kurs AutoCAD stopień I przeznaczony dla początkujących użytkowników programu AutoCAD. Na szkoleniu " +
                "użytkownik poznaje podstawowe obiekty wektorowe – " +
                "zdobywa umiejętności ich tworzenia i modyfikacji. Ponad to użytkownik " +
                "potrafi zarządzać cechami rysunku i obiektów oraz wydrukować rysunek."},
                new Course{Title = "Kurs Excel", LeadingTeacher=users[2],Status="In progress",
                    Description="Kurs przeznaczony dla początkujących użytkowników arkuszy " +
                    "kalkulacyjnych Microsoft Excel. Celem szkolenia jest nabycie " +
                    "umiejętności dobrego codziennego wykorzystywania programu MS Excel. " +
                    "Gwarantujemy dużo przykładów i ćwiczeń praktycznych."},
                new Course{Title = "Prowadzenie własnej działalności gospodarczej", LeadingTeacher=users[3],Status="Finished",
                    Description="Samodzielne prowadzenie księgi przychodów i rozchodów, karty podatkowej lub" +
                    " ryczałtu oraz rozliczeń z Urzędem Skarbowym i ZUS-em, umiejętność generowania jak" +
                    " największych zysków w swojej działalności gospodarczej, komplet aktualnych przepisów prawnych."},
                new Course{Title = "ADOBE INDESIGN", LeadingTeacher=users[2],Status="New",
                    Description="Kurs jest połączeniem wykładów z intensywnymi ćwiczeniami praktycznymi. Ćwiczenia prowadzone" +
                    " są na bazie przykładowych projektów lub też własnych projektów uczestników."},
            };
            courses.ForEach(s => context.Courses.Add(s));

            var cs = new List<CourseStudents>
            {
                new CourseStudents{Course=courses[0],Student=users[4],Status="signed_up"},
                new CourseStudents{Course=courses[0],Student=users[5],Status="approved"},
                new CourseStudents{Course=courses[0],Student=users[6],Status="approved"},
                new CourseStudents{Course=courses[0],Student=users[7],Status="declined"},
                new CourseStudents{Course=courses[1],Student=users[4],Status="approved"},
                new CourseStudents{Course=courses[2],Student=users[4],Status="approved"},
                new CourseStudents{Course=courses[2],Student=users[5],Status="approved"},
                new CourseStudents{Course=courses[2],Student=users[6],Status="approved"},
                new CourseStudents{Course=courses[2],Student=users[7],Status="approved"},
                new CourseStudents{Course=courses[3],Student=users[5],Status="signed_up"},
                new CourseStudents{Course=courses[3],Student=users[6],Status="signed_up"},
            };
            cs.ForEach(s => context.CourseStudentsTable.Add(s));

            var lessons = new List<Lesson>
            {
                new Lesson{CourseLesson=courses[0],RoomNr="100",
                    LessonStart=new DateTime(2021,7,10,8,30,00),
                    LessonEnd=new DateTime(2021,7,10,10,30,00)},
                new Lesson{CourseLesson=courses[0],RoomNr="100",
                    LessonStart=new DateTime(2021,7,12,8,30,00),
                    LessonEnd=new DateTime(2021,7,12,10,30,00)},
                new Lesson{CourseLesson=courses[0],RoomNr="101",
                    LessonStart=new DateTime(2021,7,14,8,30,00),
                    LessonEnd=new DateTime(2021,7,14,10,30,00)},
                new Lesson{CourseLesson=courses[0],RoomNr="101",
                    LessonStart=new DateTime(2021,7,16,8,30,00),
                    LessonEnd=new DateTime(2021,7,10,16,30,00)},
                new Lesson{CourseLesson=courses[0],RoomNr="101",
                    LessonStart=new DateTime(2021,7,18,8,30,00),
                    LessonEnd=new DateTime(2021,7,18,10,30,00)},
            };
            lessons.ForEach(s => context.Lessons.Add(s));
            context.SaveChanges();
        }
    }
}
