using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PlanDone.Droid
{   //przeanalizować działanie krok po kroku

    [Activity(Label = "PlanDone", Theme ="@style/Theme.Splash", MainLauncher =true, NoHistory =true)]
    class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        protected override void OnResume()
        {
            base.OnRestart();
            Task startupwork = new Task(() => { SimulateStartup(); });  //przeanalzować dokładnie
                startupwork.Start();
        }

        async void SimulateStartup()
        {
            await Task.Delay(2000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}