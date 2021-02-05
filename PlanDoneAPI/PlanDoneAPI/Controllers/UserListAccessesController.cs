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
    public class UserListAccessesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public UserListAccessService ulas = new UserListAccessService();
        public UserService us = new UserService();


        // GET: api/UserListAccesses
        public IQueryable<UserListAccess> GetUserListAccesses()
        {
            string userEmail = User.Identity.GetUserName();
            return db.UserListAccesses.Where(x => x.UserEmail ==  userEmail);
        }

        [Route("api/UserListAccess/ListsGet")]
        [HttpGet]
        public IQueryable<UserListAccess> GetTasksLists(string listID)
        {
            string userID = User.Identity.GetUserId();
            return db.UserListAccesses.Where(x => x.ListID == listID);
        }

        [Route("api/UserListAccess/Add")]
        [HttpPost]
        public IHttpActionResult AddUserToList([FromBody] UserListAccess newUserAccess)
        {
            string userID = User.Identity.GetUserId();
            string userEmail = User.Identity.GetUserName();

            int accessLevel = (db.UserListAccesses.Where(x => x.ListID == newUserAccess.ListID && x.UserEmail == userEmail).FirstOrDefault()).AccessLevel;

            if (!us.isThereSuchAnEmail(newUserAccess.UserEmail))
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
            else if (ulas.isAdmin(newUserAccess.ListID, userEmail))
            {

                if (!ulas.ifAccessExists(newUserAccess))
                {

                    try
                    {
                        ulas.addAccess(newUserAccess);
                    }
                    catch
                    {
                        return StatusCode(HttpStatusCode.InternalServerError);
                    }

                    return StatusCode(HttpStatusCode.Created);
                }
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }
            else
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            } //if you are admin, only another admin can change your access level -> list cannot be without admin
        }
        [Route("api/UserListAccess/ChangeLevel")]
        [HttpPut]
        public IHttpActionResult ChangeAccessLevel([FromBody] string AccessID, int newAccessLevel)
        {
            string userEmail = User.Identity.GetUserName();
            var userAccessToModify = ulas.FindAccess(AccessID);

            if (!ulas.ifAccessExists(userAccessToModify))
                return StatusCode(HttpStatusCode.NotFound);

            if (ulas.isAdmin(userAccessToModify.ListID, userEmail) && userEmail != userAccessToModify.UserEmail)
            {
                try
                {
                    ulas.changeAccessLevel(userAccessToModify, newAccessLevel);
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

        [Route("api/UserListAccess/Delete")]
        [HttpDelete]
        public IHttpActionResult DeleteUserFromList(string AccessID)
        {
            string userID = User.Identity.GetUserId();
            string userEmail = User.Identity.GetUserName();

            if (!ulas.ifAccessExists(AccessID))
                return StatusCode(HttpStatusCode.NotFound);

            var Access = ulas.FindAccess(AccessID);

            if (ulas.isAdmin(Access.ListID, userEmail))
            {
                if (userEmail != Access.UserEmail) //check if admin do not delete own access
                {

                    try
                    {
                        ulas.deleteOneAccess(Access);
                    }
                    catch
                    {
                        return StatusCode(HttpStatusCode.InternalServerError);
                    }

                    return StatusCode(HttpStatusCode.OK);
                }
                else //(userID == editedUserID) if admin is deleting own access
                {


                    if (ulas.AdminCount(Access.ListID) == 1) //=> if only one admin exists, he cannot delete access -> he can delete list
                    {
                        return StatusCode(HttpStatusCode.Forbidden);
                    }
                    else
                    {
                        try
                        {
                            ulas.deleteOneAccess(Access);
                        }
                        catch
                        {
                            return StatusCode(HttpStatusCode.InternalServerError);
                        }
                        return StatusCode(HttpStatusCode.OK);
                    }
                }

            }
            else if ((ulas.IsEditor(Access.ListID, userEmail) || ulas.IsViewer(Access.ListID, userEmail)) && userEmail == Access.UserEmail)
            {

                try
                {
                    ulas.deleteOneAccess(Access);
                }
                catch
                {
                    throw;
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