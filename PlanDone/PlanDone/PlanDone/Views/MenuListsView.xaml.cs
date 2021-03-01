using PlanDone.Controllers;
using PlanDone.Data;
using PlanDone.Models;
using PlanDone.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanDone.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuListsView : ContentPage
    {

    public MenuListsView()
        {
            InitializeComponent();
            BindingContext = new MenuListsViewModel(Navigation);
            Init();
          
        }

        void Init()
        {
            BackgroundColor = Color.FromHex("#fffffc");        
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}