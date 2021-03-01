using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlanDoneAPI.Models
{
    public class TasksList
    {
        [Key]
        public string TaskID { get; set; }

        public string ListID { get; set; }

        public string Taskname { get; set; }

        public DateTime CreateDate { get; set; }

        public string AuthorID { get; set; }

        public bool Done { get; set; }

        public int Priority { get; set; }

        public TasksList()
        { 
        }

        public TasksList(string TaskID, string ListID, string Taskname,
            DateTime CreateDate, string AuthorID,bool Done, int Priority=3)
        {
            this.TaskID = TaskID;
            this.ListID = ListID;
            this.Taskname = Taskname;
            this.CreateDate = CreateDate;
            this.AuthorID = AuthorID;
            this.Done = Done;
            this.Priority = Priority;
        }
    }
}