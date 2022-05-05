using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidApp = Android.App.Application;

namespace OwnLinkJitsi.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            System.Diagnostics.Debug.WriteLine(TAG, p0);
        }
        public override void OnMessageReceived(RemoteMessage message)
        {

            System.Diagnostics.Debug.WriteLine(TAG, "From: " + message.From);
            RemoteMessage.Notification tmp = message.GetNotification();
            if (tmp != null)
            {
                System.Diagnostics.Debug.WriteLine(TAG, "Notification Message Body: " + message.GetNotification().Body);
                var body = message.GetNotification().Body;
                ScheduleNotification(body, message.Data);
            }
            else
            {
                ScheduleNotification("123", message.Data);
            }
        }


        const string channelId = "OwnLinkFCM";
        const string channelName = "OwnLinkFCM";
        const string channelDescription = "The OwnLinkFCM channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 100;
        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            CreateNotificationChannel();
        }

        public int ScheduleNotification(string messageBody, IDictionary<string, string> data)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            string title = "Входящий звонок";
            if (data.ContainsKey("type"))
                title = data["type"];
            string body = "";
            if (data.ContainsKey("caller_name"))
                body = data["caller_name"];
            string room = "";
            if (data.ContainsKey("room_name"))
                room = data["room_name"];
            messageId++;

            if (!String.IsNullOrEmpty(room))
            { 
                CrossSettings.Current.AddOrUpdateValue("currentRoom", room);
                double dt = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                
                CrossSettings.Current.AddOrUpdateValue("RoomTime", dt.ToString());
            }


            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, body);

            var acceptIntent = new Intent(AndroidApp.Context, typeof(MainActivity));
            acceptIntent.SetAction("ACCEPT_ACTION");
            
            var pendingIntentAccept = PendingIntent.GetActivity(AndroidApp.Context, 12345, acceptIntent, PendingIntentFlags.OneShot);

            var declineIntent = new Intent(this, typeof(MainActivity));
            declineIntent.SetAction("DECLINE_ACTION");
            var pendingIntentDecline = PendingIntent.GetActivity(AndroidApp.Context, 12345, declineIntent, PendingIntentFlags.OneShot);


            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            //var contentIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.CancelCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                //.SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetFullScreenIntent(pendingIntent, true)
                .SetPriority((int)NotificationPriority.Max)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.icon_large))
                .SetSmallIcon(Resource.Drawable.notification_tile_bg)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone))
                .AddAction(0, "Отклонить", pendingIntentDecline)
                .AddAction(0,"Принять", pendingIntentAccept)              
                //.SetOngoing(true)
                .SetAutoCancel(true)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            manager.Notify(messageId, notification);

            return messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Max)
                {
                    Description = channelDescription
                };
                channel.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone), null);

                channel.LockscreenVisibility = NotificationVisibility.Public;
                channel.EnableVibration(true);

                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
    }
}