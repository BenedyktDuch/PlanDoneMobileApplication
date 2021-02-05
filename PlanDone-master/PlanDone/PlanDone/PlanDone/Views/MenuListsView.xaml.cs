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
        //RestApiController n_Restctr2 = new RestApiController();
      //  public List<ListProperties> Items2 { get; set; }
   

    public MenuListsView()
        {
            InitializeComponent();
            BindingContext = new MenuListsViewModel(Navigation);
            Init();
          
        }

        void Init()
        {
            Title = "PlanDone";
            BackgroundColor = Color.FromHex("#fffffc");
            //Refresh();

          
        }

       /* async void Refresh()
        {
            
            Items2 = await n_Restctr2.GetLists("https://bscthesis.azurewebsites.net/api/List/Get");
            MyListView.ItemsSource = Items2;
        }
        */

       /*  async void AddListClicked(object sender, EventArgs e)
        {
            //ToolbarItem item = (ToolbarItem)sender;
            string result = await DisplayPromptAsync("New List", "Insert new list name:");
            if (String.IsNullOrEmpty(result))
                return;
            RestApiController n_Restctrl = new RestApiController();
            var addResult = await n_Restctrl.AddList("https://bscthesis.azurewebsites.net/api/List/Add", result);
            if (addResult.StatusCode == System.Net.HttpStatusCode.Created)
                Refresh();
            else
                DisplayAlert("Error", "Something went wrong, check your connection", "Ok");
        }*/


        /* private async void ChangePasswordClicked(object sender, EventArgs e)
        {
            string oldPassword = await DisplayPromptAsync("Password Change", "Insert old password:");
            string newPassword = await DisplayPromptAsync("Password Change", "Insert new password:");
            string confirmPassword = await DisplayPromptAsync("Password Change", "Confirm password:");
            if (String.IsNullOrEmpty(oldPassword) || String.IsNullOrEmpty(newPassword) || String.IsNullOrEmpty(confirmPassword))
                return;
            RestApiController n_Restctrl = new RestApiController();
            var addResult = await n_Restctrl.ChangePassword("https://bscthesis.azurewebsites.net/api/Account/ChangePassword", oldPassword.Trim(),newPassword.Trim(), confirmPassword.Trim());
            if (addResult.StatusCode == System.Net.HttpStatusCode.OK)
                DisplayAlert("Password", "Password successfully changed.", "Ok");
            else
                DisplayAlert("Error", "Something went wrong", "Ok");
        }*/

       /* async void OnItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            MyListView.IsEnabled = false;
            ListView lv = (ListView)sender;
            ListProperties listproperties = (ListProperties)lv.SelectedItem;
            await Navigation.PushAsync(new TasksView(listproperties.ListID,listproperties.Listname));
            MyListView.IsEnabled = true;
        }
       /*
        //async void GoToCredits(object sender, EventArgs e)
       // {
       //     await Navigation.PushAsync(new CreditsView());
       // }
      /*  async void LogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Logout","Do you want to logout?", "yes", "no");
            if(answer)
            {
                Constants.userToken.Username = "";
                Constants.userToken.AccessToken = "";
                Application.Current.MainPage = new NavigationPage(new LoginView());
            }       
        }*/
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}