using PlanDone.Controllers;
using PlanDone.Data;
using PlanDone.Models;
using PlanDone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanDone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        ActivityIndicator activityIndicator;
       // public Token userToken = new Token();
       
        public LoginView()
        {
            
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
            Init();
        }

        private void Init()
        {
            BackgroundColor = Color.FromHex("#e6fafa");
            Btn_GoToRegister.BackgroundColor = Color.FromHex("#55805c"); 
            Btn_Login.BackgroundColor= Color.FromHex("#55805c");
            activityIndicator = new ActivityIndicator { IsRunning = false };
        }

    }
}