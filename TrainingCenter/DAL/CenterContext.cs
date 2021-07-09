using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TrainingCenter.DAL
{
    class CenterContext : DbContext
    {
        public CenterContext() : base("CenterContext")
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStudents> CourseStudentsTable { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
