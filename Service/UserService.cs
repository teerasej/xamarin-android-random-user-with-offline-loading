using System;
using System.Threading.Tasks;
using RestSharp;
using RandomUser.Model;
using System.Net;
using Android.App;
using System.IO;
using Result = RandomUser.Model.Result;
using Xamarin.Essentials;

namespace RandomUser.Service
{
    public class UserService
    {

        private RestClient Client;
        private string BaseUrl = "https://randomuser.me/api/";
        WebClient webClient;

        public UserService()
        {
            this.Client = new RestClient(this.BaseUrl);
        }

        public async Task<Result[]> GetUserProfiles()
        {
            var request = new RestRequest("", DataFormat.Json).AddParameter("results", 200);
            var response = await Client.ExecuteAsync(request);
            var userModel = UserRemoteModel.FromJson(response.Content);
            return userModel.Results;
        }

        public async Task<Result[]> PrepareOfflineData()
        {
            var users = new Result[] { };

            var storePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

            var dbName = "data.db";
            var dbPath = Path.Combine(storePath, dbName);
            var db = new UserDatabase(dbPath);

            var currentNetwork = Connectivity.NetworkAccess;
            var IsConnected = currentNetwork != Xamarin.Essentials.NetworkAccess.None;
            
            if (IsConnected)
            {
                users = await GetUserProfiles();
                await db.ResetDatabase();
            }
            else
            {
                // load user data from sqlite
                users = await db.GetUsersAsync();

                // return data to Activity
                return users;
            }

            var imageStorePath = storePath;
            webClient = new WebClient();

            // loop through users list to download their's profile
            foreach (var user in users)
            {
                var imageUri = user.Picture.Large;

                var fileName = Path.GetFileName(imageUri.LocalPath);
                var localImagePath = Path.Combine(imageStorePath, fileName);

                if (localImagePath == null || File.Exists(localImagePath))
                {
                    user.Picture.Large = new Uri(localImagePath);
                    await db.SaveUserAsync(user);
                    continue;
                }


                var bytes = webClient.DownloadData(imageUri);
                File.WriteAllBytes(localImagePath, bytes);

                if (File.Exists(localImagePath))
                {
                    user.Picture.Large = new Uri(localImagePath);
                    await db.SaveUserAsync(user);
                }

            }

            // after finish download image file, save its paths, and user's info to db

            // show user's info and image profile from database in listview

            return users;
        }
    }
}