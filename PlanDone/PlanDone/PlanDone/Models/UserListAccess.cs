using System;
using System.Collections.Generic;
using System.Text;

namespace PlanDone.Models
{
    public class UserListAccess
    {
        public string AccessID { get; set; }

        public string ListID { get; set; }

        public string UserEmail { get; set; }

        public int AccessLevel { get; set; }

        public UserListAccess(string ListID, string UserID, int AccessLevel)
        {
            this.ListID = ListID;
            this.UserEmail = UserID;
            this.AccessLevel = AccessLevel;
        }
    }
}
