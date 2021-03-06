using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrainingCenter.Model;

namespace TrainingCenter.DAL
{
    class DatabaseManager
    {
        CenterContext db;

        public DatabaseManager()
        {
            db = new CenterContext();
        }

        public List<Account> getAccountList()
        {
            var accounts = (from p in db.Accounts select p).ToList();
            return accounts;
        }
        public void addObjToDB(Account account)
        {
            db.Accounts.Add(account);
            db.SaveChanges();
        }
        public void removeObjFromDB(Account account)
        {
            try
            {
                db.Accounts.Remove(account);
                db.SaveChanges();
            }
            catch 
            {
                MessageBox.Show("Nie można usunąć użytkownika używanego w kursach", "Błąd");
            }
        }
        public void editObjInDB(Account account)
        {
            db.Accounts.AddOrUpdate(account);
            db.SaveChanges();
        }
        public bool isEmailAvailable(string email)
        {
            if (db.Accounts.Any(o => o.Email == email))
            { return false; }
            return true;
        }
        public bool isAccountInDatabase(string email)
        {
            return db.Accounts.Any(o => o.Email == email);
        }
        public Account getAccountWithEmail(string email)
        {
            Account emailAccount = db.Accounts.First(o => o.Email == email);
            return emailAccount;
        }
        public Account getAccountWithId (int accId)
        {
            Account emailAccount = db.Accounts.Single(o => o.AccountId == accId);
            return emailAccount;
        }
        public List<Course> getCourseList()
        {
            var courses = (from p in db.Courses select p).ToList();
            return courses;
        }//zwraca wszystkie kursy
        public List<Course> getCourseList(string email)
        {
            var courses = (from p in db.Courses where p.LeadingTeacher.Email == email select p).ToList();
            return courses;
        }//zwraca kursy ktore dany user prowadzi
        public List<CourseStudents> getMyCourses (string email)
        {
            var courses = (from p in db.CourseStudentsTable where p.Student.Email == email select p).ToList();
            return courses;
        }
        public List<CourseStudents> getMyCourses(int accId)
        {
            var courses = (from p in db.CourseStudentsTable where 
                           p.Student.AccountId == accId select p).ToList();
            return courses;
        }
        public List<CourseStudents> getMyCoursesImLeading(string email)
        {
            var courses = (from p in db.CourseStudentsTable where p.Course.LeadingTeacher.Email == email select p).ToList();
            return courses;
        }
        public List<CourseStudents> getMyCoursesImLeading(int accId)
        {
            var courses = (from p in db.CourseStudentsTable where p.Course.LeadingTeacher.AccountId == accId select p).ToList();
            return courses;
        }
        public void addObjToDB(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
        }
        public void removeObjFromDB(Course course)
        {
            db.Courses.Remove(course);
            db.SaveChanges();
        }
        public void editObjInDB(Course course)
        {
            db.Courses.AddOrUpdate(course);
            db.SaveChanges();
        }
        public Course getCourseWithId(int courseID)
        {
            Course course;
            course = db.Courses.First(a => a.CourseId == courseID);
            //course = (Course)(from p in db.Courses where p.CourseId == courseID select p);
            return course;
        }
        public List<CourseStudents> getCourseStudentsList(int courseID)
        {
            var coursestudents = (from p in db.CourseStudentsTable where p.Course.CourseId==courseID select p).ToList();
            return coursestudents;
        }
        public void addObjToDB(CourseStudents cs)
        {
            db.CourseStudentsTable.Add(cs);
            db.SaveChanges();
        }
        public void removeObjFromDB(CourseStudents cs)
        {
            db.CourseStudentsTable.Remove(cs);
            db.SaveChanges();
        }
        public void editObjInDB(CourseStudents cs)
        {
            db.CourseStudentsTable.AddOrUpdate(cs);
            db.SaveChanges();
        }
        public void removeSignUpIfExists(Course course,Account acc)
        {
            CourseStudents cs = db.CourseStudentsTable.FirstOrDefault
                (a => a.Course.CourseId == course.CourseId &&
                a.Student.AccountId == acc.AccountId);
            if (cs != null)
            {
                db.CourseStudentsTable.Remove(cs);
                db.SaveChanges();
            }
        }
        public bool isUserSignedUpAlready(int courseID, int accountID)
        {
            bool b = db.CourseStudentsTable.Any(a => a.Course.CourseId == courseID &&
            a.Student.AccountId == accountID && (a.Status == "signed_up" || a.Status == "approved"));
            return b;
        }
        public List<Lesson> getLessonsForCourse(int courseID)
        {
            var lessons = (from p in db.Lessons where p.CourseLesson.CourseId == courseID select p).ToList();
            return lessons;
        }
        public void addObjToDB(Lesson lesson)
        {
            db.Lessons.Add(lesson);
            db.SaveChanges();
        }
        public void removeObjFromDB(Lesson lesson)
        {
            db.Lessons.Remove(lesson);
            db.SaveChanges();
        }
        public void editObjInDB(Lesson lesson)
        {
            db.Lessons.AddOrUpdate(lesson);
            db.SaveChanges();
        }
        public List <Lesson> getMyLessons(int AccountId)
        {
            //sprawdzic na jakis kursach jestem
            //sciagnac lessony dla kazdego kursu
            //uporzadkowac po dacie
            var courses = getMyCourses(AccountId);
            List<Lesson> listaLekcji = new List<Lesson>();
            foreach (CourseStudents a in courses)
            {
                if (a.Status == "approved")
                {
                    var lekcje = getLessonsForCourse(a.Course.CourseId);
                    listaLekcji.AddRange(lekcje);
                }
            }
            listaLekcji.OrderBy(a => a.LessonStart);
            return listaLekcji;
        }
        public List<Lesson> getMyLessonsAsTeacher(int AccountId)
        {
            //sprawdzic na jakis kursach jestem
            //sciagnac lessony dla kazdego kursu
            //uporzadkowac po dacie
            var courses = getMyCoursesImLeading(AccountId);
            List<Lesson> listaLekcji = new List<Lesson>();
            foreach (CourseStudents a in courses)
            {
                var lekcje = getLessonsForCourse(a.Course.CourseId);
                listaLekcji.AddRange(lekcje);
            }
            listaLekcji.OrderBy(a => a.LessonStart);
            return listaLekcji;
        }
    }
}
