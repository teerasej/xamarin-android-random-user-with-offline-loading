﻿using System;
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
            var request = new RestRequest("", DataFormat.Json).AddParameter("results", 100);
            var response = await Client.ExecuteAsync(request);
            var userModel = UserRemoteModel.FromJson(response.Content);
            return userModel.Results;
        }

        public async Task<Result[]> PrepareOfflineData()
        {
            var users = new Result[] { };

            var currentNetwork = Connectivity.NetworkAccess;

            if(currentNetwork != Xamarin.Essentials.NetworkAccess.None)
            {
                users = await GetUserProfiles();
            }
            else
            {
                // load user data from sqlite

                // return data to Activity
            }

            

            var imageStorePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

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
                    continue;
                }


                var bytes = webClient.DownloadData(imageUri);
                File.WriteAllBytes(localImagePath, bytes);

                if (File.Exists(localImagePath))
                {
                    user.Picture.Large = new Uri(localImagePath);
                }

            }

            // after finish download image file, save its paths, and user's info to db

            // show user's info and image profile from database in listview

            return users;
        }
    }
}