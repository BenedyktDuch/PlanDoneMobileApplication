using PlanDone.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlanDone.ViewModels
{
    class RegisterViewModel : INotifyPropertyChanged
    {

        RestApiService n_Restctrl = new RestApiService();
        public string Entry_Email { get; set; }
        public string Entry_Password { get; set; }
        public string Entry_ConfirmedPassword { get; set; }

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
        public INavigation Navigation { get; set; }
        public RegisterViewModel()
        {

        }

        public RegisterViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.SignUpBtnClicked = new Command(async () => await SignUp());
        }


        private async System.Threading.Tasks.Task SignUp()
        {
            AreBtnsEn = false;
            if (string.Equals(Entry_Password, Entry_ConfirmedPassword))
            {
                var isRegistered = await n_Restctrl.Register( Entry_Email, Entry_Password, Entry_ConfirmedPassword);
                if (isRegistered.StatusCode == System.Net.HttpStatusCode.OK)
                    await Application.Current.MainPage.DisplayAlert("Register", "Account created", "Ok");
                else
                    await Application.Current.MainPage.DisplayAlert("Register", "Something went wrong, check your connection", "Ok");

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Password", "Passwords are different", "Ok");

            }
            AreBtnsEn = true;

        }

        public ICommand SignUpBtnClicked
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

