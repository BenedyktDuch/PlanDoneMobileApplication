using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PlanDone.Models
{
    public class ListProperties
    {
        public string ListID { get; set; }

        public string Listname { get; set; }

        public string CreatorID { get; set; }

        public ListProperties(string listID, string listname, string creatorID)
        {
            ListID = listID;
            Listname = listname;
            CreatorID = creatorID;
        }

       
    }
}
