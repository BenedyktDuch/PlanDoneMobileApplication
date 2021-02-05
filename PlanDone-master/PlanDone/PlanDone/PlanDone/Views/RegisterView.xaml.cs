using PlanDone.Controllers;
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
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            
            InitializeComponent();
            BindingContext = new RegisterViewModel(Navigation);
            Init();
        }
        void Init()
        {
            BackgroundColor = Color.FromHex("#e6fafa");
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#55805c");
        }

    }
}