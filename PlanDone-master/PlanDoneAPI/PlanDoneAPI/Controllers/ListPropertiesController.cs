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
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PlanDoneAPI.Data;
using PlanDoneAPI.Models;
using PlanDoneAPI.Service;

namespace PlanDoneAPI.Controllers
{
    [Authorize]
    public class ListPropertiesController : ApiController
    {
        public ListPropertiesService lps = new ListPropertiesService();
        public UserListAccessService ulas = new UserListAccessService();

        [Route("api/List/Get")]
        [HttpGet]
        public async Task<List<ListProperties>> GetLists()
        {
             return lps.GetUserLists(User.Identity.GetUserName());         
        }


        [Route("api/List/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddList([FromBody] string listname)
        {
            string userID = User.Identity.GetUserId();
            string userEmail = User.Identity.GetUserName();

            lps.AddUserLists(userID, userEmail, listname);
            return StatusCode(HttpStatusCode.Created);
        }

        [Route("api/List/Delete")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteList(string listID)
        {
            string userID = User.Identity.GetUserId();
            string userEmail = User.Identity.GetUserName();
            if (ulas.isAdmin(listID,userEmail))
            {
                try
                {
                    lps.DeleteList(listID);
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
        [Route("api/List/ChangeName")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangeListName([FromBody] ListProperties newList)
        {
            string userEmail = User.Identity.GetUserName();        
            if (ulas.isAdmin(newList.ListID,userEmail))
            {    
                try
                {
                    lps.ChangeListName(newList);
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