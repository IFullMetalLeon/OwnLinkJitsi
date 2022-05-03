using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OwnLinkJitsi.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(OpenAppAndroid))]
namespace OwnLinkJitsi.Droid
{
    public class OpenAppAndroid : IAppHandler
    {
        public Task<bool> LaunchApp(string packageName)
        {


            bool result = true;
            Intent intent = new Intent();

            intent.SetComponent(new ComponentName("net.jitsi.sdktest", "net.jitsi.sdktest.MainActivity"));
            intent.PutExtra("room", packageName);
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);

            return Task.FromResult(result);
        }
    }
}