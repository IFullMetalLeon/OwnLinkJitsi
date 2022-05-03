using Plugin.DeviceInfo;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OwnLinkJitsi.ViewModel;
using Plugin.FirebasePushNotification;

namespace OwnLinkJitsi.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallPage : ContentPage
    {
        public CallPage(string _url)
        {
            InitializeComponent();           
            string deviceId = CrossDeviceInfo.Current.Id;
            string phone = CrossSettings.Current.GetValueOrDefault("sipPhoneLogin", "");
            HttpControler.ReadySignSend(phone, deviceId, _url,"Accept");
            CrossSettings.Current.AddOrUpdateValue("currentRoom", "");
            CrossFirebasePushNotification.Current.ClearAllNotifications();
            enterRoom(_url);
        }

        public async void enterRoom(string _room)
        {
            await DependencyService.Get<IAppHandler>().LaunchApp(_room);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                var response = await Permissions.RequestAsync<Permissions.Camera>();
                if (response != PermissionStatus.Granted)
                {
                }
            }

            var statusMic = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (statusMic != PermissionStatus.Granted)
            {
                var response = await Permissions.RequestAsync<Permissions.Microphone>();
                if (response != PermissionStatus.Granted)
                {
                }
            }
        }
    }
}