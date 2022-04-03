using Acr.UserDialogs;
using OwnLinkJitsi.View;
using Plugin.DeviceInfo;
using Plugin.FirebasePushNotification;
using Plugin.Settings;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace OwnLinkJitsi
{
    public partial class App : Application
    {
        public string FCMToken { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            FCMToken = "";
            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;

            string _login = CrossSettings.Current.GetValueOrDefault("sipPhoneLogin", "");
            string deviceId = CrossDeviceInfo.Current.Id;
            string deviceInfo = CrossDeviceInfo.Current.Manufacturer + " " + CrossDeviceInfo.Current.Model + " " + CrossDeviceInfo.Current.Platform + " " + CrossDeviceInfo.Current.Version;

            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;

            //MainPage = new NavigationPage(new TestPage());
            if (_login == "")
            {
                MainPage = new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.FromHex("#FFFFFF"), BarTextColor = Color.FromHex("#000000") };
                //((NavigationPage)Xamarin.Forms.Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#FF4C3F");


            }
            else
            {
                MainPage = new NavigationPage(new MasterDetailMain()) { BarBackgroundColor = Color.FromHex("#FFFFFF"), BarTextColor = Color.FromHex("#000000") };
                //((NavigationPage)Xamarin.Forms.Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#FF4C3F");
            }


        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            string room = "test";
            try
            {
                room = e.Data["room_name"].ToString();
            }
            catch (Exception ex)
            { }
            try
            {
                MessagingCenter.Send<string, string>("App", "CallPage", room);
            }
            catch(Exception ex) { }
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            MessagingCenter.Send<string, string>("App", "ChangeToken", e.Token);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
