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
        public CallPageViewModel cpvm { get; set; }
        public CallPage()
        {
            InitializeComponent();
            cpvm = new CallPageViewModel();
            this.BindingContext = cpvm;
 
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            cpvm.beginPage();
            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            cpvm.endPage();
        }

        private bool OnTimerTick()
        {
            string _room = CrossSettings.Current.GetValueOrDefault("currentRoom", "");
            if(String.IsNullOrEmpty(_room))
            {
                MessagingCenter.Send<string, string>("Call", "CallState", "End");
                return true;
            }
            else
                return false;
        }
    }
}