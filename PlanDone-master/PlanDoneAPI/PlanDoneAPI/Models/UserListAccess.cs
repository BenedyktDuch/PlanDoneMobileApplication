using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlanDoneAPI.Models
{
    public class UserListAccess
    {
        [Key]
        public string AccessID { get; set; }

        public string ListID { get; set; }

        public string UserEmail { get; set; }

        public int AccessLevel { get; set; }
    }
}