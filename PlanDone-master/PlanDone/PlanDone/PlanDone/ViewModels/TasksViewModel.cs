using PlanDone.Controllers;
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
    class TasksViewModel : INotifyPropertyChanged
    {
        RestApiService n_Restctrl = new RestApiService();
        public INavigation Navigation { get; set; }
        private ObservableCollection<Models.Task> _items;
        public ObservableCollection<Models.Task> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanges();
            }

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


       public string ListID { get; set; } 

        public TasksViewModel()
        {

        }

        public TasksViewModel(INavigation navigation, string listID)
        {
            
            this.Navigation = navigation;
            this.ListID = listID;
            this.RefreshTaskClicked = new Command(async () => await Refresh());
            this.AccessBtnClicked = new Command(async () => await GoToAccess());
            this.AddTaskBtnClicked = new Command(async () => await AddTask());
            this.DeleteListBtnClicked = new Command(async () => await DeleteList());
            this.ChangeStateClicked = new Command<string>(async (taskid) => await ChangeTaskState(taskid));
            this.DeleteTaskClicked = new Command<string>(async (taskid) => await DeleteTask(taskid));

            Init();
        }

        private async void Init()
        {
            Items = await n_Restctrl.GetTasks(ListID); 
        }
        public ICommand RefreshTaskClicked { get; set; }

        public async System.Threading.Tasks.Task Refresh()
        {
            IsRefreshing = true;
            Items = await n_Restctrl.GetTasks(ListID);
            IsRefreshing = false;
        }
        public ICommand AccessBtnClicked { get; set; }

        private async System.Threading.Tasks.Task GoToAccess()
        {
           await Navigation.PushAsync(new UserAccessView(ListID));

        }

        public ICommand AddTaskBtnClicked { get; set; }

        private async System.Threading.Tasks.Task AddTask()
        {
            AreBtnsEn = false;
            string taskName = await Application.Current.MainPage.DisplayPromptAsync("New Task", "Insert new task name:");
            if (String.IsNullOrEmpty(taskName))
                return;
            Models.Task newTask = new Models.Task(ListID, taskName);
            var addResult = await n_Restctrl.AddTask(newTask);
            if (addResult.StatusCode == HttpStatusCode.Created)
               await Refresh();
            else if (addResult.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Application.Current.MainPage.DisplayAlert("Sorry", "You do not have permission to do this", "Ok");
            }
            else
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong, check your connection", "Ok");

            AreBtnsEn = true;
        }

        public ICommand DeleteListBtnClicked { get; set; }

        private async System.Threading.Tasks.Task DeleteList()
        {
            AreBtnsEn = false;
            var deleteResult = await n_Restctrl.DeleteList(ListID);
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete List", "Are you sure?", "Yes", "No");
            if (answer)
            {
                if (deleteResult.StatusCode == HttpStatusCode.OK)
                {
                    await Navigation.PushAsync(new MenuListsView());
                }
                else if (deleteResult.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Sorry", "You do not have permission to do this", "Ok");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Sorry", "Something went wrong, check your connection", "Ok");
            }
            else
                return;
            AreBtnsEn = true;
        }

        private string _taskid;

        public string TaskID
        {
            get
            {
                return _taskid;
            }
            set
            {
                _taskid = value;
                OnPropertyChanges();
            }
        }
       
        public ICommand ChangeStateClicked { get; set; }

        private async System.Threading.Tasks.Task ChangeTaskState(string taskid)
        {
            AreBtnsEn = false;
            var checkResult = await n_Restctrl.CheckTask( taskid);
            if (checkResult.StatusCode == HttpStatusCode.OK)
                await Refresh();
            else if (checkResult.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Application.Current.MainPage.DisplayAlert("Sorry", "You do not have permission to do this", "Ok");
            }
            else
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong, check your connection", "Ok");
            AreBtnsEn = true;
        }

        public ICommand DeleteTaskClicked { get; set; }

        private async System.Threading.Tasks.Task  DeleteTask(string taskid)
        {
            AreBtnsEn = false;
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are you sure?", "Yes", "No");
            if (answer)
            {

                var deleteResult = await n_Restctrl.DeleteTask( taskid);
                if (deleteResult.StatusCode == HttpStatusCode.OK)
                    await Refresh();
                else if (deleteResult.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Sorry", "You do not have permission to do this", "Ok");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong, check your connection", "Ok");
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
