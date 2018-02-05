// Entity Framework 6, Code First
// demo of storing a Student entity in a database table

using System;
using System.Data.Entity;
using System.Linq;

// use Nuget to install Entity Framework 6.1.3 (which updates app.config)

namespace EFStudentDemoCF
{
    // a Student entity
    public class Student                                // POCO
    {
        public int ID { get; set; }                     // PK and identity also
        public string StudentNo { get; set; }           // X number, null
        public string Name { get; set; }                // null
        public string Email { get; set; }               // null
    }
    // Students table with column names corresponding to property names
    
    // context of connection to database 
    public class StudentContext : DbContext       // System.Data.Entity.DbContext        
    {
        public DbSet<Student> Students { get; set; }
    }
   
    // cf app.config for connection string EFStudentDemoCF.StudentContext
    // data stored in a SQL Server Express Local DB file studentDB1.mdf
    // table names are pluralised based on entity name by default in generated db
    
    class Program
    {
        static void Main()
        {
            using (StudentContext studentContext = new StudentContext())       // DbContext : IDisposable
            {
                // add 2 students, omit identity column values 
                Student s1 = new Student() { StudentNo = "X00001111", Name = "Gary Clynch", Email = "garyclynch@ittd.ie" };
                Student s2 = new Student() { StudentNo = "X00002222", Name = "Jane Doe", Email = "janedoe@ittd.ie" };

                studentContext.Students.Add(s1);
                studentContext.Students.Add(s2);
                studentContext.SaveChanges();                                   // commit

                // query students in order of ascending student number
                // LINQ to entities
                var studentsQueryInOrder = studentContext.Students.OrderBy(student => student.StudentNo);
                foreach (var student in studentsQueryInOrder)
                {
                    Console.WriteLine(student.ID + " " + student.StudentNo + " " + student.Name + " " + student.Email);
                }
            } 

            // using disposes of unmanaged resources correctly
            // is try / finally with Dispose called on resource in finally
        }
    }
}

/*
Conventions:
Primary Key - property called ID or <classname>ID, int or GUID makes it identity column also
*/
