using PlanDone.Controllers;
using PlanDone.Models;
using PlanDone.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanDone
{
    //DO ZROBIENIA:
    //1. Generować jeden token, jeden api kontroller w jakiś dobry sposób
    //2. Rozdzielić metody w RestAPIController do wysyłania żądań bo dużo kodu się powtarza
    public partial class App : Application
    {
        static RestApiService restApiController;
        public App()
        {

            InitializeComponent();

            MainPage = new NavigationPage(new LoginView());
            



        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static RestApiService RestApiController   //????????????????????????????????
        {
            get
            {
                if (restApiController == null)
                {
                    restApiController = new RestApiService();
                }
                return RestApiController;
            }
        }
    }
}
