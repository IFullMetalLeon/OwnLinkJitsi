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
            

            //CrossSettings.Current.AddOrUpdateValue("sipPhoneLogin", "79041537453");
            string _login = CrossSettings.Current.GetValueOrDefault("sipPhoneLogin", "");
            string deviceId = CrossDeviceInfo.Current.Id;
            string deviceInfo = CrossDeviceInfo.Current.Manufacturer + " " + CrossDeviceInfo.Current.Model + " " + CrossDeviceInfo.Current.Platform + " " + CrossDeviceInfo.Current.Version;

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
