namespace EFStudentDemoCF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MobileNoAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "MobileNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "MobileNo");
        }
    }
}
