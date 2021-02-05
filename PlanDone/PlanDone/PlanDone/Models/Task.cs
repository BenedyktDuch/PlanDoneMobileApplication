using System;
using System.Collections.Generic;
using System.Text;

namespace PlanDone.Models
{
    public class Task
    {
        public string TaskID { get; set; }

        public string ListID { get; set; }

        public string Taskname { get; set; }

        public DateTime CreateDate { get; set; }

        public string AuthorID { get; set; }

        public bool Done { get; set; }

        public int Priority { get; set; }

        public Task(string ListID,string Taskname)
        {
            this.ListID = ListID;
            this.Taskname=Taskname;
        }
    }
}
