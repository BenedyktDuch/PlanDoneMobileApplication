using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PlanDoneAPI.Data
{
    public class PlanDoneAPIContext : DbContext
    {
        
        public PlanDoneAPIContext() : base("name=DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<PlanDoneAPI.Models.ListProperties> ListProperties { get; set; }

        public System.Data.Entity.DbSet<PlanDoneAPI.Models.TasksList> TasksLists { get; set; }

        public System.Data.Entity.DbSet<PlanDoneAPI.Models.UserListAccess> UserListAccesses { get; set; }
    }
}
// You can add custom code to this file. Changes will not be overwritten.
// 
// If you want Entity Framework to drop and regenerate your database
// automatically whenever you change your model schema, please use data migrations.
// For more information refer to the documentation:
// http://msdn.microsoft.com/en-us/data/jj591621.aspx