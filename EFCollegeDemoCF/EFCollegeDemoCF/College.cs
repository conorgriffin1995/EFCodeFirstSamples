// Entity Framework demo, Code First
// 1:N relationship, navigation property, a lecturer teaches many modules

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCollegeDemoCF
{
    public class Lecturer
    {
        public int ID { get; set; }                 // PK and identity
        public string Name { get; set; }            // null
        public string Phone { get; set; }           // null

        // navigation property to modules that Lecturer teaches, virtual => lazy loading  
        public virtual ICollection<Module> Modules { get; set; }      
    }

    public class Module
    {
        public int ID { get; set; }                             // PK and identity
        public string Name { get; set; }                        // null
        public int Credits { get; set; }                        // not null, use int? for null
        
        // foreign key property, null, follows convention for naming
        public int? LecturerID { get; set; }    
        // update relationship through this property, not through navigation property
        // int would not allow null for LecturerID                 

        // navigation property to Lecturer for this module
        public virtual Lecturer Lecturer { get; set; }           // virtual enables "lazy loading" 
    }

   
    // context class
    public class CollegeContext : DbContext
    {
        public CollegeContext(): base("DefaultConnection")
        {
           // always drop and re-create the schema
           Database.SetInitializer<CollegeContext>(new DropCreateDatabaseAlways<CollegeContext>());
        }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Module> Modules { get; set; }
    }


    class CollegeRepository
    {
        
        // print lecturers, their ids, names, and the names of modules they teach 
        public void DoLecturerQuery()
        {
            using (CollegeContext db = new CollegeContext())
            {
                var lecturers = db.Lecturers.OrderBy(l => l.ID).Select(l => new { Id = l.ID, Name = l.Name, Modules = l.Modules });

                Console.WriteLine("\nLecturers:");
                foreach (var lecturer in lecturers)
                {
                    Console.WriteLine("id: " + lecturer.Id + " name: " + lecturer.Name);
                    Console.WriteLine("teaches: ");

                    // Modules is a navigation propery of type ICollection<Module>
                    var lecturerModules = lecturer.Modules;
                    foreach (var lecturerModule in lecturerModules)
                    {
                        Console.WriteLine(lecturerModule.Name);
                    }
                }
            }
        }

        // prints the ID and name of each module and the name of the lecturer who teaches it
        public void DoModuleQuery()
        {
            using (CollegeContext db = new CollegeContext())
            {
                // select all modules, ordered by module name
                var modules = db.Modules.OrderBy(module => module.ID).ToList();       // load

                Console.WriteLine("\nModules:");
                foreach (var module in modules)
                {
                    Console.WriteLine("id: " + module.ID + " name: " + module.Name + " ");

                    if (module.Lecturer != null)
                    {
                        // Lecturer is a navigation property of type Lecturer
                        Console.WriteLine(" lectured by: " + module.Lecturer.Name);
                    }
                }
            }
        }

        // add a lecturer, modules being taught left null for moment
        public void AddLecturer(Lecturer lecturer)
        {
            using (CollegeContext db = new CollegeContext())
            {
                try
                {
                    // add and save
                    db.Lecturers.Add(lecturer);
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        // add a module, contains lecturerID
        public void AddModule(Module module)
        {
            using (CollegeContext db = new CollegeContext())
            {
                try
                {
                    // add and save
                    db.Modules.Add(module);
                    db.SaveChanges();
                    // navigation properties updated on both sides
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }

    class College
    {
        static void Main()
        {
          
            CollegeRepository repository = new CollegeRepository();

            Lecturer gc = new Lecturer() { Name = "GC", Phone = "2898" };
            repository.AddLecturer(gc);         // ID now assigned
      
            // teaches 2 modules
            Module oosdev1 = new Module() { Name = "OOSDEV1", Credits = 5, LecturerID = gc.ID };
            Module oosdev2 = new Module() { Name = "OOSDEV2", Credits = 5, LecturerID = gc.ID };

            Module ead = new Module() { Name = "EAD", Credits = 10};       // null for LecturerID

            repository.AddModule(oosdev1);
            repository.AddModule(oosdev2);
            repository.AddModule(ead);

            repository.DoLecturerQuery();
            repository.DoModuleQuery();

            Console.ReadLine();
        }
    }
}

// conventions
// foreign key property:
// - <navigation property name><principal primary key property name> e.g LecturerID
// - if allows null then relationship optional, otherwise required and delete cascade

