using System;
using System.Collections.Generic;
using System.Text;

namespace PlanDone.Models
{
    public class User
    {
        public string UserID;
        public string UserEmail;
        public string Password;

        public User(string UserEmail, string Password)
        {
            this.UserEmail = UserEmail;
            this.Password = Password;
        }
        public User()
         {

        }
    }
}
