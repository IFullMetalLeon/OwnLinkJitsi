using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(OwnLinkJitsi.Droid.AndroidFCMService))]
namespace OwnLinkJitsi.Droid
{
    class AndroidFCMService : IFCMService
    {
        public string GetToken()
        {
            return FirebaseInstanceId.Instance.Token;
        }
    }
}