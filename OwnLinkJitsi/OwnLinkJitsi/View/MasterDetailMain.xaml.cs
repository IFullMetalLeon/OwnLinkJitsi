﻿using OwnLinkJitsi.ViewModel;
using Plugin.DeviceInfo;
using Plugin.FirebasePushNotification;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OwnLinkJitsi.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailMain : MasterDetailPage
    {
        public MasterDetailMainViewModel mdmvm { get; set; }
        public int flag;
        public int isCallShow;
        public MasterDetailMain()
        {
            InitializeComponent();
            mdmvm = new MasterDetailMainViewModel() { Navigation = this.Navigation };
            this.BindingContext = mdmvm;
            flag = 0;
            isCallShow = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            


            mdmvm.startPage();
            isCallShow = 0;
            if (flag == 0)
            {
                Detail = new NavigationPage(new HistoryCallPage()) { BarBackgroundColor = Color.FromHex("#FFFFFF"), BarTextColor = Color.FromHex("#000000") };
                IsPresented = true;
                NavigationPage.SetHasNavigationBar(this, false);
                NavigationPage.SetHasBackButton(this, false);
                flag = 1;
            }
            MessagingCenter.Subscribe<string, string>("Call", "CallState", (sender, arg) =>
            {
                showCall(arg.Trim());
            });
            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            mdmvm.endPage();
            MessagingCenter.Unsubscribe<string, string>("Call", "CallState");
        }

        private void activeCall_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new CallPage(mdmvm.Room));
            IsPresented = false;
        }

        private void historyCall_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HistoryCallPage()) { BarBackgroundColor = Color.FromHex("#FFFFFF"), BarTextColor = Color.FromHex("#000000") };
            IsPresented = false;
        }

        private void setting_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SettingPage()) { BarBackgroundColor = Color.FromHex("#FFFFFF"), BarTextColor = Color.FromHex("#000000") };
            IsPresented = false;
        }

        private void web_Clicked(object sender, EventArgs e)
        {
            mdmvm.showUri();
        }

        public void showCall(string content)
        {
            //Detail = new NavigationPage(new SettingPage());
            IsPresented = false;
        }

        private bool OnTimerTick()
        {
            if (mdmvm.ReceiveCall)
            {
                string deviceId = CrossDeviceInfo.Current.Id;
                string phone = CrossSettings.Current.GetValueOrDefault("sipPhoneLogin", "");
                HttpControler.ReadySignSend(phone, deviceId, mdmvm.Room, "Accept");
                CrossSettings.Current.AddOrUpdateValue("currentRoom", "");
                CrossFirebasePushNotification.Current.ClearAllNotifications();
                enterRoom(mdmvm.Room);
                //Detail = new NavigationPage(new CallPage(mdmvm.Room));
                IsPresented = false;
                return false;
            }
            else
                return true;
        }

        public async void enterRoom(string _room)
        {
            await DependencyService.Get<IAppHandler>().LaunchApp(_room);
        }
    }
}