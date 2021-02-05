using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using PlanDoneAPI.Data;
using PlanDoneAPI.Models;
using PlanDoneAPI.Service;

namespace PlanDoneAPI.Controllers
{
    [Authorize]
    public class TasksListsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public UserListAccessService ulas = new UserListAccessService();
        public TasksListService tls = new TasksListService();

        [Route("api/Task/Get")]
        [HttpGet]
        public IQueryable<TasksList> GetTasksLists(string listID)
        {
            string userID = User.Identity.GetUserId();
            return db.TasksLists.Where(x=>x.ListID==listID).OrderByDescending(x=>x.CreateDate);
        }

        [Route("api/Task/Add")]
        [HttpPost]
        public IHttpActionResult AddTask([FromBody] TasksList newTask)
        {
            if (ulas.isAdminOrEditor(newTask.ListID, User.Identity.GetUserName()))
            {

                try
                {
                    newTask.AuthorID = User.Identity.GetUserId();
                    tls.AddTask(newTask);
                }
                catch
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
                return StatusCode(HttpStatusCode.Created);
            }
            else
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

        }
        [Route("api/Task/Delete")]
        [HttpDelete]
        public IHttpActionResult DeleteTask(string taskID)
        {
            string listID = db.TasksLists.Where(x => x.TaskID == taskID).FirstOrDefault().ListID;
            if (ulas.isAdminOrEditor(listID, User.Identity.GetUserName()))
            {
                try
                {
                    tls.DeleteTask(taskID);
                }
                catch
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }

                return StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

        }

        [Route("api/Task/DoneChange")]
        [HttpPut]
        public IHttpActionResult TaskDoneChange([FromBody] string taskID)
        {
            string listID = db.TasksLists.Where(x => x.TaskID == taskID).FirstOrDefault().ListID;

            if (ulas.isAdminOrEditor(listID, User.Identity.GetUserName()))
            {
                try
                {
                    tls.TaskDoneChange(taskID);
                }
                catch
                {
                    return StatusCode(HttpStatusCode.InternalServerError);

                }
                return StatusCode(HttpStatusCode.OK);

            }
            else
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

        }

    }
}
