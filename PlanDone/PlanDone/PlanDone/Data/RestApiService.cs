using Newtonsoft.Json;
using PlanDone.Data;
using PlanDone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


// KLASA DO UPORZĄDKOWANIA, ROZDZIELIĆ GENEROWANIE JSON I WYSYŁANIE POST REQUEST
namespace PlanDone.Controllers
{
    public class RestApiService
    {
        HttpClient client;
        string grant_type = "password";

        public RestApiService()
        {
            client = new HttpClient();
        }

        public async Task<Token> GetToken(User user) 
        {
            string url="https://bscthesis.azurewebsites.net/Token";
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", grant_type));
            postData.Add(new KeyValuePair<string, string>("Username", user.UserEmail));
            postData.Add(new KeyValuePair<string, string>("Password", user.Password));
            client.DefaultRequestHeaders.Clear();
            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(postData)
            };
            try
            {
                var res = await client.SendAsync(req);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonResult = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<Token>(JsonResult);
                }
                else
                    return null;
            }
            catch 
            { 
                return null;
            }
            
        }

        public async Task<HttpResponseMessage> Register(string email, string password, string confirmPassword )
        {
            string url = "https://bscthesis.azurewebsites.net/api/Account/Register";
            var postData = new Dictionary<string, string>();    
            postData.Add("Email", email);
            postData.Add("Password", password);
            postData.Add("ConfirmPassword", confirmPassword);
            client.DefaultRequestHeaders.Clear();
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(postData);
            try
            {
                var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                    return result;
                
            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
           
        }

        public async Task<ObservableCollection<ListProperties>> GetLists()
        {
            string url = "https://bscthesis.azurewebsites.net/api/List/Get";
            ObservableCollection<ListProperties> receivedList = new ObservableCollection<ListProperties>();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            try
            {
                var result = await client.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var content = result.Content.ReadAsStringAsync().Result;
                    receivedList = JsonConvert.DeserializeObject<ObservableCollection<ListProperties>>(content);
                    return receivedList;
                }                 
            }
            catch
            {
                return null;
            }
            return null;
        }

        public async Task<HttpResponseMessage> AddList( string Listname)
        {
            string url = "https://bscthesis.azurewebsites.net/api/List/Add";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(Listname);
            try
            {
                var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }

        public async Task<HttpResponseMessage> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            string url = "https://bscthesis.azurewebsites.net/api/Account/ChangePassword";
            var postData = new Dictionary<string, string>();    // Creating list with parameters going through Urlencoded
            postData.Add("OldPassword", oldPassword);
            postData.Add("NewPassword", newPassword);
            postData.Add("ConfirmPassword", confirmPassword);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(postData);
            try
            {
                var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }

        }
        public async Task<ObservableCollection<Models.Task>> GetTasks(string listID)
        {
            string url = "https://bscthesis.azurewebsites.net/api/Task/Get?listID=" + listID;
            ObservableCollection<Models.Task> receivedList = new ObservableCollection<Models.Task>();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            try
            {
                var result = await client.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var stringResult = result.Content.ReadAsStringAsync().Result;
                    receivedList = JsonConvert.DeserializeObject<ObservableCollection<Models.Task>>(stringResult);
                    return receivedList;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public async Task<HttpResponseMessage> AddTask( Models.Task newTask)
        {
            string url = "https://bscthesis.azurewebsites.net/api/Task/Add";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(newTask);
            try
            {
                var result = await client.PostAsync(url, new StringContent(content,
                    Encoding.UTF8, contentType));
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.RequestTimeout;
                return catchedMessage;
            }
        }

        public async Task<HttpResponseMessage> DeleteTask(string taskID)
        {
            string url = "https://bscthesis.azurewebsites.net/api/Task/Delete?taskID=" + taskID;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            try
            {
                var result = await client.DeleteAsync(url);
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }
        public async Task<HttpResponseMessage> CheckTask( string taskID)
        {
            string url = "https://bscthesis.azurewebsites.net/api/Task/DoneChange";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(taskID);
            try
            {
                var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }
        public async Task<HttpResponseMessage> DeleteList(string ListID)
        {
            string url = "https://bscthesis.azurewebsites.net/api/List/Delete?listID=" + ListID;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            try
            {
                var result = await client.DeleteAsync(url);
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }

        public async Task<ObservableCollection<Models.UserListAccess>> GetUserAccess(string listID)
        {
            string url = "https://bscthesis.azurewebsites.net/api/UserListAccess/ListsGet?listID=" + listID;
            ObservableCollection<Models.UserListAccess> receivedList = new ObservableCollection<UserListAccess>();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            try
            {
                var result = await client.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var stringResult = result.Content.ReadAsStringAsync().Result;
                    receivedList = JsonConvert.DeserializeObject<ObservableCollection<Models.UserListAccess>>(stringResult);
                    return receivedList;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public async Task<HttpResponseMessage> AddUserAccess( UserListAccess newUserAccess)
        {
            string url = "https://bscthesis.azurewebsites.net/api/UserListAccess/Add";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(newUserAccess);
            try
            {
                var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }
        public async Task<HttpResponseMessage> ChangeUserAccess(int accessLvL,string accessID)
        {
            string url = "https://bscthesis.azurewebsites.net/api/UserListAccess/ChangeLevel?newAccessLevel=" + accessLvL;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(accessID);
            try
            {
                var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }
        public async Task<HttpResponseMessage> DeleteUserAccess(string accessID)
        {
            string url = "https://bscthesis.azurewebsites.net/api/UserListAccess/Delete?AccessID=" + accessID;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            try
            {
                var result = await client.DeleteAsync(url);
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }


        /*
        public async Task<List<string>> ConvertIds(string url, List<string> IDs)
        {
            List<string> receivedList = new List<string>();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(IDs);
            try
            {
                var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                if (result.IsSuccessStatusCode)
                {
                    var resultContent = result.Content.ReadAsStringAsync().Result;
                    receivedList = JsonConvert.DeserializeObject<List<string>>(resultContent);
                    return receivedList;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }/*

        /*public async Task<HttpResponseMessage> AddTask(string url, UserListAccess newAccess)        //bad practice -> going to send userEmail property by UserID field what is mixing concept and assumptions
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Constants.userToken.AccessToken);
            string contentType = "application/json";
            var content = JsonConvert.SerializeObject(newAccess);
            try
            {
                var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, contentType));
                return result;

            }
            catch
            {
                HttpResponseMessage catchedMessage = new HttpResponseMessage();
                catchedMessage.StatusCode = HttpStatusCode.InternalServerError;
                return catchedMessage;
            }
        }*/
    }
}
