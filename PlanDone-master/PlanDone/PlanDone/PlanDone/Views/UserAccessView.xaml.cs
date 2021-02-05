using PlanDone.Controllers;
using PlanDone.Data;
using PlanDone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanDone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserAccessView : ContentPage
    {
     
        public List<Models.UserListAccess> Items2 { get; set; }

        public UserAccessView(string listID)
        {
            InitializeComponent();
            BindingContext = new UserAccessViewModel(Navigation, listID);
            Title = "Users' accesses";
            BackgroundColor = Color.FromHex("#fffffc");
        }

      
        
    }
}