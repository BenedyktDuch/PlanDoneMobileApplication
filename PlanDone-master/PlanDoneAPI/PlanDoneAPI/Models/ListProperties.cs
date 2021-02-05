using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace PlanDoneAPI.Models
{
    public class ListProperties
    {
        [Key]
        public string ListID { get; set; }

        public string Listname { get; set; }

        public string CreatorID { get; set; }

        public ListProperties() { }

        public ListProperties(string ListID, string Listname, string CreatorID)
        {
            this.ListID = ListID;
            this.Listname = Listname;
            this.CreatorID = CreatorID;
        }
    }
}