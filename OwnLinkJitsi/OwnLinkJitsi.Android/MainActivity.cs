using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.FirebasePushNotification;
using Acr.UserDialogs;
using System.Collections.Generic;
using Android;
using Android.Support.V4.App;
using AndroidApp = Android.App.Application;
using Android.Content;
using Xamarin.Forms;
using Android.Media;
using Plugin.Settings;

namespace OwnLinkJitsi.Droid
{
    [Activity(Label = "Своя связь", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        int PERMISSIONS_REQUEST = 101;
        public static MainActivity instance { set; get; }
        public static int NOTIFICATION_ID { get; internal set; }
        const string channelId = "OwnLinkBG";
        const string channelName = "OwnLinkBG";
        const string channelDescription = "The OwnLinkBG channel for notifications.";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            instance = this;
            CreateNotificationChannel();
            LoadApplication(new App());

            switch (Intent.Action)
            {
                case "ACCEPT_ACTION":
                    {
                        CrossSettings.Current.AddOrUpdateValue("RoomNeedAcc", "");
                        var manager = (NotificationManager)this.GetSystemService(Context.NotificationService);
                        manager.CancelAll();
                        break;
                    }
                case "DECLINE_ACTION":
                    {
                        CrossSettings.Current.AddOrUpdateValue("RoomTime", "0");
                        CrossSettings.Current.AddOrUpdateValue("currentRoom", "");
                        CrossSettings.Current.AddOrUpdateValue("RoomNeedAcc", "");
                        var manager = (NotificationManager)this.GetSystemService(Context.NotificationService);
                        manager.CancelAll();
                        break;
                    }
                default:
                    {
                        CrossSettings.Current.AddOrUpdateValue("RoomNeedAcc", "1");
                        var manager = (NotificationManager)this.GetSystemService(Context.NotificationService);
                        manager.CancelAll();
                        break;
                    }
                    
            }

            //FirebasePushNotificationManager.ProcessIntent(this, Intent);
            
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (Int32.Parse(Build.VERSION.Sdk) >= 23)
            {
                List<string> Permissions = new List<string>();
                if (CheckSelfPermission(Manifest.Permission.Camera) != Permission.Granted)
                {
                    Permissions.Add(Manifest.Permission.Camera);
                }
                if (CheckSelfPermission(Manifest.Permission.RecordAudio) != Permission.Granted)
                {
                    Permissions.Add(Manifest.Permission.RecordAudio);
                }
                if (CheckSelfPermission(Manifest.Permission.ReadCallLog) != Permission.Granted)
                {
                    Permissions.Add(Manifest.Permission.ReadCallLog);
                }
                if (Permissions.Count > 0)
                {
                    RequestPermissions(Permissions.ToArray(), PERMISSIONS_REQUEST);
                }
            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void CreateNotificationChannel()
        {
            NotificationManager manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Max)
                {
                    Description = channelDescription
                };
                channel.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone), null);
                //channel.SetSound(null, null);
                channel.LockscreenVisibility = NotificationVisibility.Public;
                channel.Importance = NotificationImportance.Max;
                channel.EnableVibration(true);

                manager.CreateNotificationChannel(channel);
            }

        }

        protected override void OnNewIntent(Intent intent)
        {
            /*switch (intent.Action)
            {
                case "ACCEPT_ACTION":
                    Console.WriteLine("hit accept action case.");
                    break;
                case "DECLINE_ACTION":
                    Console.WriteLine("hit decline action case.");
                    break;
                default:
                    Console.WriteLine("didn't hit either action.");
                    break;
            }

            var manager = (NotificationManager)this.GetSystemService(Context.NotificationService);
            manager.CancelAll();*/
            base.OnNewIntent(intent);
            //CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }
    }
}