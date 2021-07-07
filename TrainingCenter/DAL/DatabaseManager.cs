using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            db.Accounts.Remove(account);
            db.SaveChanges();
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
        public Account findAccountWithEmail(string email)
        {
            Account emailAccount = db.Accounts.Single(o => o.Email == email);
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
        }//zwraca kursy dla danego uzytkownika
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
    }
}
