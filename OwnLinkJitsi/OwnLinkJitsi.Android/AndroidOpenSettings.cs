using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Net;
using Android.Support.V4.App;
using OwnLinkJitsi;
using OwnLinkJitsi.Droid;
using Xamarin.Android;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using AndroidApp = Android.App.Application;
using Uri = Android.Net.Uri;

[assembly: Dependency(typeof(OwnLinkJitsi.Droid.AndroidOpenSettings))]

namespace OwnLinkJitsi.Droid
{
    class AndroidOpenSettings : IOpenSettings
    {
        public void GoToSettings()
        {
            Intent intent = new Intent(Settings.ActionApplicationDetailsSettings);
            string package_name = "OwnLink.fsin.com";
            var uri = Uri.FromParts("package", package_name, null);
            intent.SetData(uri);
            MainActivity.instance.StartActivity(intent);
        }
    }
}