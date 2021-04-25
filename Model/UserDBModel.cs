using System;
using SQLite;

namespace RandomUser.Model
{
    public class UserDBModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("image_path")]
        public string ImagePath { get; set; }

        public UserDBModel()
        {
        }

        public void ApplyDataFromRemoteModel(Result result)
        {
            this.FirstName = result.Name.First;
            this.LastName = result.Name.Last;

            this.PhoneNumber = result.Phone;
            this.Email = result.Email;

            this.ImagePath = result.Picture.Large.AbsoluteUri;
        }
    }
}
