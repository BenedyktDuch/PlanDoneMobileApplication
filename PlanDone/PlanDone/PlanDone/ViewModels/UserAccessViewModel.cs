using PlanDone.Controllers;
using PlanDone.Data;
using PlanDone.Models;
using PlanDone.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlanDone.ViewModels
{
    class UserAccessViewModel : INotifyPropertyChanged
    {
        RestApiService n_Restctrl = new RestApiService();
        private ObservableCollection<Models.UserListAccess> _items;
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
        public ObservableCollection<Models.UserListAccess> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanges();
            }

        }
        public UserAccessViewModel()
        {

        }

        public INavigation Navigation { get; set; }
        private string ListID { get; set; }

        public UserAccessViewModel(INavigation navigation, string listID)
        {
            
            this.Navigation = navigation;
            this.ListID = listID;
            this.RefreshClicked = new Command(async () => await Refresh());
            this.AddUserAccessClicked = new Command(async () => await AddUserAccess());
            this.DeleteAccessClicked = new Command<UserListAccess>(async (access) => await DeleteAccess(access));
            this.ChangeAccessLevelClicked = new Command<UserListAccess>(async (access) => await ChangeAccessLevel(access));
            Init();
        }

        private async void Init()
        {
            Items = await n_Restctrl.GetUserAccess(ListID);

        }

        private bool _isRefreshing;
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

        public ICommand RefreshClicked { get; set; }

        private async System.Threading.Tasks.Task Refresh()
        {
            IsRefreshing = true;
            Items = await n_Restctrl.GetUserAccess( ListID);
            IsRefreshing = false;
        }

        public ICommand AddUserAccessClicked { get; set; }

        private async System.Threading.Tasks.Task AddUserAccess()
        {
            AreBtnsEn = false;
            string newUserEmail = await Application.Current.MainPage.DisplayPromptAsync("New Access", "Insert new user Email:");
            if (String.IsNullOrEmpty(newUserEmail))
                return;
            string inputNewAccessLevel = await Application.Current.MainPage.DisplayPromptAsync("New Access", "Insert his/her access level: \n 1 - Viewer \n 2 - Editor \n 3 - Admin");
            if (String.IsNullOrEmpty(inputNewAccessLevel))
                return;

            int newAccessLevel = Convert.ToInt32(inputNewAccessLevel);
            if (newAccessLevel > 0 && newAccessLevel < 4)
            {
                Models.UserListAccess newAccess = new Models.UserListAccess(ListID, newUserEmail, Convert.ToInt32(newAccessLevel));
                var addResult = await n_Restctrl.AddUserAccess(newAccess);
                if (addResult.StatusCode == System.Net.HttpStatusCode.Created)
                    await Refresh();
                else if (addResult.StatusCode == HttpStatusCode.NotFound)
                    await Application.Current.MainPage.DisplayAlert("Sorry", "This user does not exists", "Ok");
                else if (addResult.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    await Application.Current.MainPage.DisplayAlert("Sorry", "This user is in the list already", "Ok");
                else if (addResult.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    await Application.Current.MainPage.DisplayAlert("Sorry", "You do not have permission to do this", "Ok");
                else
                    await Application.Current.MainPage.DisplayAlert("Sorry", "Something went wrong, check your connection", "Ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Sorry", "Wrong input format", "Ok");
            }
            AreBtnsEn = true;
        }

        public ICommand DeleteAccessClicked { get; set; }

        private async System.Threading.Tasks.Task DeleteAccess (UserListAccess access)
        {
            AreBtnsEn = false;
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete Access", "Are you sure? \n Remember: If you are only one admin, deleting cause removing list and all tasks", "Yes", "No");
            if (answer)
            {
                var deleteResult = await n_Restctrl.DeleteUserAccess(access.AccessID);

                if (deleteResult.StatusCode == HttpStatusCode.OK)
                {
                    await Refresh();
                    if (access.UserEmail == Constants.userToken.Username)
                        await Navigation.PushAsync(new MenuListsView());
                }
                else if (deleteResult.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    await Application.Current.MainPage.DisplayAlert("Sorry", "You do not have permission to do this", "Ok");
                else
                    await Application.Current.MainPage.DisplayAlert("Sorry", "Something went wrong, check your connection", "Ok");

            }
            AreBtnsEn = true;

        }

        public ICommand ChangeAccessLevelClicked { get; set; }

        private async System.Threading.Tasks.Task ChangeAccessLevel(UserListAccess access)
        {
            AreBtnsEn = false;

            string inputNewAccessLevel = await Application.Current.MainPage.DisplayPromptAsync("New Access", "Insert his/her access level: \n 1 - Viewer \n 2 - Editor \n 3 - Admin");
            if (String.IsNullOrEmpty(inputNewAccessLevel))
                return;

            int newAccessLevel = Convert.ToInt32(inputNewAccessLevel);
            if (newAccessLevel > 0 && newAccessLevel < 4)
            {
               
                var checkResult = await n_Restctrl.ChangeUserAccess(newAccessLevel, access.AccessID);
                if (checkResult.StatusCode == HttpStatusCode.OK)
                    await Refresh();
                else if (checkResult.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    await Application.Current.MainPage.DisplayAlert("Sorry", "You do not have permission to do this", "Ok");
                else
                    await Application.Current.MainPage.DisplayAlert("Sorry", "Something went wrong, check your connection", "Ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Sorry", "Wrong input format", "Ok");
            }
            AreBtnsEn = true;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

