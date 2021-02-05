using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.ApplicationServices;
using PlanDoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PlanDoneAPI.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private Models.ApplicationDbContext db = new ApplicationDbContext();
    }
}
