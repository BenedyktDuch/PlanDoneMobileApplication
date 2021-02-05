using PlanDone.Controllers;
using PlanDone.Data;
using PlanDone.Models;
using PlanDone.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlanDone.ViewModels
{
    class MenuListsViewModel : INotifyPropertyChanged
    {
        RestApiService n_Restctrl = new RestApiService();
        private ObservableCollection<ListProperties> _items;
        public ObservableCollection<ListProperties> Items
        {
            get { return _items; }
            set {
                _items = value;
                OnPropertyChanges(); }
        
        }
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

        private bool _isRefreshing = false;
        public bool IsRefreshing
        { 
            get 
            { 
                return _isRefreshing;
            }

            set
            {
                _isRefreshing = value;
                OnPropertyChanges();
            }
        }


        public INavigation Navigation { get; set; }
        public MenuListsViewModel()
        {

        }

        public MenuListsViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.RefreshCommandClicked = new Command(async () => await Refresh());
            this.AddListBtnClicked = new Command(async () => await AddList());
            this.CredtisBtnClicked = new Command(async () => await GoToCredits());
            this.ChangePasswordBtnClicked = new Command(async () => await ChangePassword());
            this.LogoutBtnClicked = new Command(async () => await Logout());
            // this.ListBtnClicked = new Command(async () => await GoToList());
            Init();
        }
        async void Init()
        {
            Items = await n_Restctrl.GetLists();
        }
        public ICommand RefreshCommandClicked { get; protected set; }

        private async System.Threading.Tasks.Task Refresh()
        {
            IsRefreshing = true;
            Items = await n_Restctrl.GetLists();
            IsRefreshing = false;
        }
       
        public ICommand AddListBtnClicked { get; protected set; }

        private async System.Threading.Tasks.Task AddList()
        {
            AreBtnsEn = false;
            string result = await Application.Current.MainPage.DisplayPromptAsync("New List", "Insert new list name:");
            if (String.IsNullOrEmpty(result))
                return;
            var addResult = await n_Restctrl.AddList( result);
            if (addResult.StatusCode == System.Net.HttpStatusCode.Created)
                await Refresh();
            else
               await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong, check your connection", "Ok");
            AreBtnsEn = true;
        }
        
        public ICommand CredtisBtnClicked { get; set; }

        private async System.Threading.Tasks.Task GoToCredits()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CreditsView());
        }

        public ICommand ChangePasswordBtnClicked { get; set; }

        private async System.Threading.Tasks.Task ChangePassword()
        {
            AreBtnsEn = false;
            string oldPassword = await Application.Current.MainPage.DisplayPromptAsync("Password Change", "Insert old password:");
            string newPassword = await Application.Current.MainPage.DisplayPromptAsync("Password Change", "Insert new password:");
            string confirmPassword = await Application.Current.MainPage.DisplayPromptAsync("Password Change", "Confirm password:");
            if (String.IsNullOrEmpty(oldPassword) || String.IsNullOrEmpty(newPassword) || String.IsNullOrEmpty(confirmPassword))
                return;
  
            var addResult = await n_Restctrl.ChangePassword( oldPassword.Trim(), newPassword.Trim(), confirmPassword.Trim());
            if (addResult.StatusCode == System.Net.HttpStatusCode.OK)
               await Application.Current.MainPage.DisplayAlert("Password", "Password successfully changed.", "Ok");
            else
               await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong", "Ok");

            AreBtnsEn = true;
        }

        public ICommand LogoutBtnClicked { get; set; }

        private async System.Threading.Tasks.Task Logout()
        {
            AreBtnsEn = false;
            bool answer = await Application.Current.MainPage.DisplayAlert("Logout", "Do you want to logout?", "yes", "no");
            if (answer)
            {
                Constants.userToken.Username = "";
                Constants.userToken.AccessToken = "";
                Application.Current.MainPage = new NavigationPage(new LoginView());
            }
            AreBtnsEn = true;
        }

       
        private ListProperties _selectedItem;

        public ListProperties SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;

                if (_selectedItem == null)
                    return;

                // ListBtnClicked.Execute(_selectedItem);
                //new Command(async () => await GoToList(_selectedItem)).Execute(_selectedItem);
                //Navigation.PushAsync(new TasksView(_selectedItem.ListID, _selectedItem.Listname));
                ListBtnClicked.Execute(_selectedItem);
                SelectedItem = null;
                OnPropertyChanges();
            }
        }

        public ICommand ListBtnClicked
        {
            get
            {
                return new Command(async () => await GoToList(_selectedItem));
            }
                
        }
        private async System.Threading.Tasks.Task GoToList(ListProperties lp)
        {
            await Navigation.PushAsync(new TasksView(lp.ListID, lp.Listname));
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
