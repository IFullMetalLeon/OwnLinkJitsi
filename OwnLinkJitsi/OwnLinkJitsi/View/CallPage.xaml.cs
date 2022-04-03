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

namespace OwnLinkJitsi.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallPage : ContentPage
    {
        public CallPage(string _url)
        {
            InitializeComponent();

            var urlSource = new UrlWebViewSource();
            //string baseUrl = DependencyService.Get<IWebViewService>().GetContent();
            urlSource.Url = "https://meet.pismo-fsin.ru/" + _url;
            CallWebView.Source = urlSource;
            string deviceId = CrossDeviceInfo.Current.Id;
            string phone = CrossSettings.Current.GetValueOrDefault("sipPhoneLogin", "");
            HttpControler.ReadySignSend(phone, deviceId, _url);

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            /*await Task.Delay(1000);

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
            }*/
        }
    }
}