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
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";

                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
                FirebasePushNotificationManager.NotificationContentTitleKey = "TitleLocal";
                FirebasePushNotificationManager.NotificationContentTextKey = "TextLocal";

            }

            //If debug you should reset the token each time.
            FirebasePushNotificationManager.Initialize(this, false);
           // FirebasePushNotificationManager.Initialize(this, false);

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                foreach (var key in p.Data.Keys)
                {
                    Console.WriteLine("String key: " + key + " = " + p.Data[key].ToString());
                    intent.PutExtra(key, p.Data[key].ToString());
                }

                p.Data.TryGetValue("type", out object Title);
                p.Data.TryGetValue("caller_name", out object Body);

                //if (Title.ToString())

                var pendingIntent = PendingIntent.GetActivity(this, 100, intent, PendingIntentFlags.OneShot);
                var notificationBuilder = new NotificationCompat.Builder(this, "FCMChannel")
                                      .SetContentTitle("TitleGlobal")
                                      .SetContentText("BodyGlobal")
                                     . SetVisibility((int)NotificationVisibility.Public)
                                    .SetFullScreenIntent(pendingIntent, true)
                                    .SetSmallIcon(Resource.Drawable.notification_tile_bg)
                                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone))
                                    //.SetOngoing(true)
                                    .SetAutoCancel(true)
                                    .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);
                //.SetStyle(new BigPictureStyle().SetSummaryText(Body.ToString()))

                var notificationManager = NotificationManagerCompat.From(this);
                notificationManager.Notify(100, notificationBuilder.Build());

            };
        }
    }
}