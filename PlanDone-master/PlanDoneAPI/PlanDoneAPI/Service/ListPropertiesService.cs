using PlanDoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanDoneAPI.Service
{
    public class ListPropertiesService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<ListProperties> GetUserLists(string userEmail)
        {
            List<ListProperties> lista = new List<ListProperties>();
            

            var data = (from lp in db.ListProperties
                        join ula in db.UserListAccesses
                        on lp.ListID equals ula.ListID
                        where ula.UserEmail == userEmail
                        select new
                        {
                            ListID = lp.ListID,
                            Listname = lp.Listname,
                            CreatorID = lp.CreatorID

                        }).ToList();


            foreach (var v in data)
            {
                lista.Add(new ListProperties(v.ListID, v.Listname, v.CreatorID));
            }

            return lista;
        }

        public void AddUserLists(string userID, string userEmail, string listname)
        {

            ListProperties newList = new ListProperties();
            UserListAccess newUserAccess = new UserListAccess();
            newUserAccess.AccessID = Guid.NewGuid().ToString();
            newList.ListID = Guid.NewGuid().ToString();
            newUserAccess.ListID = newList.ListID;

            newList.CreatorID = userID;
            newUserAccess.UserEmail = userEmail;

            newList.Listname = listname;
            newUserAccess.AccessLevel = 3; //1-see list, 2-edit, 3-admin

            db.ListProperties.Add(newList);
            db.UserListAccesses.Add(newUserAccess);
            db.SaveChanges();
            
        }
        public void DeleteList(string listID)
        {
            var listToDelete = db.ListProperties.Where(x => x.ListID == listID).FirstOrDefault();
            var tasksToDelete = db.TasksLists.Where(x => x.ListID == listID).ToList();
            var AccessesToDelete = db.UserListAccesses.Where(x => x.ListID == listID).ToList();

            foreach (TasksList t in tasksToDelete)
                db.TasksLists.Remove(t);
            foreach (UserListAccess v in AccessesToDelete)
                db.UserListAccesses.Remove(v);

            db.ListProperties.Remove(listToDelete);
            db.SaveChanges();
        }

        public void ChangeListName(ListProperties lp)
        {
            var listNameToChange = db.ListProperties.Where(x => x.ListID == lp.ListID).FirstOrDefault();
            listNameToChange.Listname = lp.Listname;
            db.SaveChanges();

        }
    }
}