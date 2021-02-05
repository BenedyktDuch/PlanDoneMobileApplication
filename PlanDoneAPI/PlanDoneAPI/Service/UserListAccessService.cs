using PlanDoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanDoneAPI.Service
{
    public class UserListAccessService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool isAdmin(string listID, string userEmail)
        {
            int accessLevel = (db.UserListAccesses.Where(x => x.ListID == listID && x.UserEmail == userEmail).FirstOrDefault()).AccessLevel;

            if (accessLevel == 3)
                return true;
            else
                return false;
        }
        public bool IsEditor(string listID, string userEmail)
        {
            int accessLevel = (db.UserListAccesses.Where(x => x.ListID == listID && x.UserEmail == userEmail).FirstOrDefault()).AccessLevel;

            if (accessLevel == 2)
                return true;
            else
                return false;
        }
        public bool IsViewer(string listID, string userEmail)
        {
            int accessLevel = (db.UserListAccesses.Where(x => x.ListID == listID && x.UserEmail == userEmail).FirstOrDefault()).AccessLevel;

            if (accessLevel == 1)
                return true;
            else
                return false;
        }
        public bool isAdminOrEditor(string listID, string userEmail)
        {

            int accessLevel = (db.UserListAccesses.Where(x => x.ListID == listID && x.UserEmail == userEmail).FirstOrDefault()).AccessLevel;
            if (accessLevel == 3 || accessLevel==2)
                return true;
            else
                return false;
        }

        public bool ifAccessExists(UserListAccess newUserAccess)
        {
            var accessExists = 
                (db.UserListAccesses.Where
                (x => x.ListID == newUserAccess.ListID &&
                x.UserEmail == newUserAccess.UserEmail)
                .FirstOrDefault());

            if (accessExists == null)
                return false;
            else
                return true;
        }
        public bool ifAccessExists(string accessID)
        {
            var accessExists = (db.UserListAccesses.Where(x => x.AccessID == accessID).FirstOrDefault());
            if (accessExists == null)
                return false;
            else
                return true;
        }

        public UserListAccess FindAccess(string accessID) // to which list access corressponds
        {
            return db.UserListAccesses.Where(x => x.AccessID == accessID).FirstOrDefault();
        }

        public void addAccess(UserListAccess newUserAccess)
        {
            newUserAccess.AccessID = Guid.NewGuid().ToString();
            db.UserListAccesses.Add(newUserAccess);
            db.SaveChanges();

        }

        public void changeAccessLevel(UserListAccess userAccessToModify, int newAccessLevel)
        {

            var AccessToChange = db.UserListAccesses.Where(x => x.AccessID==userAccessToModify.AccessID).FirstOrDefault();
            AccessToChange.AccessLevel = newAccessLevel;
            db.SaveChanges();
        }

        public void deleteOneAccess(UserListAccess Access)
        {
            var AccessToRemove = db.UserListAccesses.Where(x => x.AccessID == Access.AccessID).FirstOrDefault();
            db.UserListAccesses.Remove(AccessToRemove);
            db.SaveChanges();
        }
        public int AdminCount(string listID)
        {
            return (db.UserListAccesses.Where(x => x.ListID == listID && x.AccessLevel == 3).ToList()).Count();
        }
    }
}