namespace PlanDoneAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksLists", "Taskname", c => c.String());
            DropColumn("dbo.TasksLists", "Task");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TasksLists", "Task", c => c.String());
            DropColumn("dbo.TasksLists", "Taskname");
        }
    }
}
