// Entity Framework 6, Code First, CRUD for a student entity
// custom connection string, annotations on student entity

using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace EFStudentDemoCF
{
    // model class (entity)
    public class Student                               
    {
        public int ID { get; set; }                     // PK and identity
        
        [Required]                                      // not null
        public string StudentNo { get; set; }               
        [Required]
        public string Name { get; set; }                // not null
        public string Email { get; set; }               // null

        public override string ToString()
        {
            return ID + " " + StudentNo + " " + Name + " " + Email;
        }
    }

    // connection to db
    public class StudentContext : DbContext             
    {
        // explicitly specify name of connection string in app.config
        public StudentContext() : base("DefaultConnection")
        {
            Database.SetInitializer<StudentContext>(new DropCreateDatabaseAlways<StudentContext>());
            // or CreateDatabaseIfNotExists (default)
            // DropCreateDatabaseIfModelChanges
            // or DropCreateDatabaseAlways
        }

        public DbSet<Student> Students { get; set; }
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

        // update one student's e-mail address
        static void Update()
        {
            using (StudentContext db = new StudentContext())
            {
                var student = db.Students.FirstOrDefault(s => s.StudentNo == "X00001111");
                if (student != null)
                {
                    student.Email = "gclynch@ittd.ie";
                    db.SaveChanges();
                }
            }
        }

        // delete a student
        static void Delete()
        {
            using (StudentContext db = new StudentContext())
            {
                // find Jane Doe
                var student = db.Students.FirstOrDefault(s => s.Name == "Jane Doe");
                if (student != null)
                {
                    db.Students.Remove(student);                  // delete entity
                    db.SaveChanges();
                }
            }
        }

        static void Main()
        {
            Add();
            Update();
            Delete();
            QueryAll();
        }
    }

    // also:
    // for code first migrations use package manager console:
    // Enable-Migrations –EnableAutomaticMigrations 
    // Update-Database 
    // migrations keep data, and migrate the schema
}
