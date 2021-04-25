using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Result = RandomUser.Model.Result;

namespace RandomUser
{
    public class UserProfileListAdapter : BaseAdapter<Result>
    {
        private Result[] users;
        private Activity context;

        public UserProfileListAdapter(Activity context, Result[] users)
        {
            this.users = users;
            this.context = context;
        }

        public override Result this[int position] => this.users[position];

        public override int Count => this.users.Length;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.list_item_user_profile, null);
            }

            var user = this.users[position];

            var fullName = user.Name.First + " " + user.Name.Last;
            view.FindViewById<TextView>(Resource.Id.textFullName).Text = fullName;

            view.FindViewById<TextView>(Resource.Id.textPhoneNumber).Text = user.Phone;

            return view;
        }
    }
}