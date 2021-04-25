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

        public Task<int> ResetDatabase()
        {
            return database.DropTableAsync<UserDBModel>();
        }
    }
}
