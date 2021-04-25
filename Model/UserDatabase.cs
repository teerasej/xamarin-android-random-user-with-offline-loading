using System;
using System.Threading.Tasks;
using SQLite;

namespace RandomUser.Model
{
    public class UserDatabase
    {
        readonly SQLiteAsyncConnection database;

        public UserDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<UserDBModel>().Wait();
        }

        public async Task<bool> ResetDatabase()
        {
            await database.DropTableAsync<UserDBModel>();
            await database.CreateTableAsync<UserDBModel>();
            return true;
        }

        public Task<int> SaveUserAsync(Result userRemoteData)
        {
            var userToDB = new UserDBModel();
            userToDB.ApplyDataFromRemoteModel(userRemoteData);

            if (userToDB.Id != 0)
            {
                return database.UpdateAsync(userToDB);
            }
            else
            {
                return database.InsertAsync(userToDB);
            }
        }
    }
}
