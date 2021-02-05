using PlanDone.Controllers;
using PlanDone.Data;
using PlanDone.Models;
using PlanDone.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlanDone.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {

        RestApiService n_Restctrl = new RestApiService();
        public string Entry_Email { get; set; }
        public string Entry_Password { get; set; }

        private bool _areBtnsEn = true;
        public bool AreBtnsEn
        {
            get { return _areBtnsEn; }
            set
            {
                _areBtnsEn = value;
                OnPropertyChanges();
            }
        }
        public User user { get; set; }

        public Token userToken = new Token();
        public INavigation Navigation { get; set; }
        public LoginViewModel()
        {

        }

        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.GoToRegistereBtnClicked = new Command(async () => await GotoRegister());
            this.LoginBtnClicked = new Command(async () => await GotoMenu());
        }

        private async System.Threading.Tasks.Task GotoRegister()
        {
            AreBtnsEn = false;
            await Navigation.PushAsync(new RegisterView());
           AreBtnsEn = true;
        }
        private async System.Threading.Tasks.Task GotoMenu()
        {
            AreBtnsEn = false;
            User user = new User(Entry_Email, Entry_Password);
            userToken = await n_Restctrl.GetToken(user);
            Constants.userToken = userToken;
            if (userToken != null)
            {
              await Application.Current.MainPage.DisplayAlert("Login", "Login succesfull", "Ok");

               Application.Current.MainPage = new NavigationPage(new MenuListsView());
            }

            else {
                await Application.Current.MainPage.DisplayAlert("Login", "Login failed", "Ok");
            }
            AreBtnsEn = true;

        }

        public ICommand GoToRegistereBtnClicked
        {

            protected set;
            get;
        }
        public ICommand LoginBtnClicked
        {
            protected set;
            get;
        }

      

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
