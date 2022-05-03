using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwnLinkJitsi.Droid;

namespace OwnLinkJitsi.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            //Set the default notification channel for your app when running Android Oreo
            //if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            //{
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
                FirebasePushNotificationManager.NotificationActivityType = typeof(MainActivity);
                FirebasePushNotificationManager.SoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
                FirebasePushNotificationManager.NotificationActivityFlags = ActivityFlags.SingleTop | ActivityFlags.ClearTop;
            //}


            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
                {
                    new NotificationUserCategory("request",new List<NotificationUserAction> {
                        new NotificationUserAction("Reply","Reply", NotificationActionType.Foreground),
                        new NotificationUserAction("Forward","Forward", NotificationActionType.Foreground)

                    }),
                    new NotificationUserCategory("request",new List<NotificationUserAction> {
                        new NotificationUserAction("Reject","Отклонить", NotificationActionType.Default, "cancel"),
                        new NotificationUserAction("Accept","Принять", NotificationActionType.Default, "AcceptCall")
                    
                    })
                },false);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
               

                p.Data.TryGetValue("title", out var Title);
                p.Data.TryGetValue("body", out var Text);

               
                
            };
        }
    }
}