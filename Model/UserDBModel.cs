﻿using System;
using SQLite;

namespace RandomUser.Model
{
    [Table("users")]
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

            this.ImagePath = result.Picture.Large.AbsolutePath;
        }

        public Result GenerateUserResultIntance()
        {
            var result = new Result
            {
                Name = new Name
                {
                    First = this.FirstName,
                    Last = this.LastName
                },

                Phone = this.PhoneNumber,
                Email = this.Email,

                Picture = new Picture
                {
                    Large = new Uri(this.ImagePath)
                }
            };

            return result;
        }
    }
}
