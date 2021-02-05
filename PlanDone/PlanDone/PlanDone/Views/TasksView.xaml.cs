using PlanDone.Controllers;
using PlanDone.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanDone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksView : ContentPage
    {
        string ListName { get; set; }
        
        public TasksView(string listID, string listName)
        {
            
            InitializeComponent();
            BindingContext = new TasksViewModel(Navigation, listID);
            ListName = listName;
            Init();
        }

        void Init()
        {
            Title = ListName;
            BackgroundColor = Color.FromHex("#fffffc");
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#55805c");

        }
    }

}

