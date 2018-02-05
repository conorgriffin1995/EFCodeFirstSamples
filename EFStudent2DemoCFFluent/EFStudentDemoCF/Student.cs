// Entity Framework 6, Code First, CRUD for a student entity
// custom connection string
// Fluent to customise entity mappings
// also uses code first migrations

using System;
using System.Data.Entity;
using System.Linq;

namespace EFStudentDemoCF
{
    // model class (entity)
    public class Student                               
    {
        public int ID { get; set; }                     // PK and identity
        
        public string StudentNo { get; set; }               
        public string Name { get; set; }               
        public string Email { get; set; }    
        
        // add after migration:
        public String MobileNo { get; set; }           

        public override string ToString()
        {
            return ID + " " + StudentNo + " " + Name + " " + Email;
        }
    }

    // connection to db
    public class StudentContext : DbContext             
    {
        public DbSet<Student> Students { get; set; }

        // explicitly specify name of connection string in app.config
        public StudentContext() : base("DefaultConnection")
        {
        }

        // customise mappings here using Fluent API
        // DbModelBuilder is used to map CLR objects to database schema
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // both StudentNo and Name must be not null in schema
            modelBuilder.Entity<Student>().Property(s => s.StudentNo).IsRequired();
            modelBuilder.Entity<Student>().Property(s => s.Name).IsRequired();
        }

        // can customise more things than by using annoations e.g. inheritance (TPT, TPH), 
        // navigation properties

    }

    class Program
    {
        static void Add()
        {
            using (StudentContext db = new StudentContext())
            {
                // add 2 studies, omit identity column values
                Student s1 = new Student() { StudentNo = "X00001111", Name = "Gary Clynch", Email = "garyclynch@ittd.ie" };
                Student s2 = new Student() { StudentNo = "X00002222", Name = "Jane Doe", Email = "janedoe@ittd.ie" };
                db.Students.Add(s1);
                db.Students.Add(s2);
                db.SaveChanges();     
            }
        }
     
        static void QueryAll()
        {
            using (StudentContext db = new StudentContext())
            {
                // LINQ to entities
                var query = db.Students.OrderBy(student => student.StudentNo);
                foreach (var student in query)
                {
                    Console.WriteLine(student);
                }
            }
        }

        static void Main()
        {
            Add();
            QueryAll();
        }
    }


    // code first migrations allows us to keep data and migrate schema
    // 1. Enable-Migrations
    // in package manager console, this creates migrations folder
    // 2. add MobileNo property to Student entity
    // 3. Add-Migration MobileNoAdded
    // 4. Update-Database 
    // ...
    
}
