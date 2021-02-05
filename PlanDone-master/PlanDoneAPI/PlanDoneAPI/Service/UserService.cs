using PlanDoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanDoneAPI.Service
{
    public class UserService
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool isThereSuchAnEmail(string userEmail)
        {
           var checkIfExists = db.Users.Where(x => x.Email == userEmail).FirstOrDefault();

            if (checkIfExists == null)
                return false;
            else
                return true;
        }
    }
}