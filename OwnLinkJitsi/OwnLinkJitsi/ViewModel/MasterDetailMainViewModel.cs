using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Acr.UserDialogs;
using Plugin.Settings;
using OwnLinkJitsi.Model;
using Xamarin.Forms;
using OwnLinkJitsi.View;
using System.Net;
using OwnLinkJitsi.ViewModel;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Xamarin.Essentials;
using Plugin.FirebasePushNotification;

namespace OwnLinkJitsi.ViewModel
{
    public class MasterDetailMainViewModel : INotifyPropertyChanged
    {

        public INotificationManager notificationManager;
        public IFCMService fCMService;
        public IOpenSettings openSettings;

        public string _phone { get; set; }
        public string _pass { get; set; }
        public string _regStatus { get; set; }
        public string _versionNumber { get; set; }
        public string _regStatusIcon { get; set; }
        public string _room { get; set; }
        public INavigation Navigation { get; set; }
        public Command ChangePhone { get; set; }
        public Command Reconnect { get; set; }

        public int isNotifySend;

        public string _uri;
        public MasterDetailMainViewModel()
        {
            ChangePhone = new Command(changePhone);

            notificationManager = DependencyService.Get<INotificationManager>();
            fCMService = DependencyService.Get<IFCMService>();
            openSettings = DependencyService.Get<IOpenSettings>();

            isNotifySend = 0;
            _uri = "http://xn--b1akbuscr4eza.xn--p1ai/";
        }

        public void startPage()
        {
            MessagingCenter.Subscribe<string, string>("HttpControler", "GetServerVersion", (sender, arg) => {
                checkVersion(arg.Trim());
            });

            //HttpControler.GetServerVersion();
            string s = CrossDeviceInfo.Current.DeviceName;
            string deviceId = CrossDeviceInfo.Current.Id;
            s = CrossDeviceInfo.Current.Model;
            s = CrossDeviceInfo.Current.Platform.ToString();

            Room= CrossSettings.Current.GetValueOrDefault("currentRoom", "");

            Phone = CrossSettings.Current.GetValueOrDefault("sipPhoneLogin", "");
            _pass = CrossSettings.Current.GetValueOrDefault("sipPhonePass", "");

            VersionNumber = "Версия " + CrossDeviceInfo.Current.AppVersion;


            string curFCMToken = fCMService.GetToken();

            if (!String.IsNullOrEmpty(curFCMToken))
                HttpControler.FCMTokenSend(Phone, curFCMToken, deviceId);


            ReceiveCall = false;
            RegStatus = "Оффлайн";
            RegStatusIcon = "StatusOffline.png";


            isNotifySend = 0;


            string isNotificationPermission = CrossSettings.Current.GetValueOrDefault("sipNotifyPer", "0");
            if (isNotificationPermission == "0")
            {
                //checkNotifyPermission();
            }

            /*CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
            CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened;
            CrossFirebasePushNotification.Current.OnNotificationAction += Current_OnNotificationAction;*/

            /*if (!String.IsNullOrEmpty(Room))
            {
                enterRoom();
            }*/
        }

        

        public void endPage()
        {
            MessagingCenter.Unsubscribe<string, string>("HttpControler", "GetServerVersion");
        }

        public void changePhone()
        {
            MessagingCenter.Send<string, string>("Call", "CallState", Room);
            //Navigation.PushAsync(new LoginPage());
        }

        public async void checkVersion(string versionNum)
        {
            string curVer = CrossDeviceInfo.Current.AppVersion;
            if (curVer != versionNum)
            {
                var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Title = "Обновление",
                    Message = "Доступна новая версия приложения",
                    OkText = "Скачать",
                    CancelText = "Отмена"
                });
                if (result)
                {
                    //await Launcher.OpenAsync("https://ic.pismo-fsin.ru/upgrade/OwnLink.fsin.apk");
                }

            }
        }

        public async void checkNotifyPermission()
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Title = "Дополнительные разрешения",
                Message = "Для корректной работы приложения нужно перейти в Настройки и включить для приложения Автозапуск и разрешить Уведомления.",
                OkText = "Настройки",
                CancelText = "Отмена"
            });
            if (result)
            {
                openSettings.GoToSettings();
                CrossSettings.Current.AddOrUpdateValue("sipNotifyPer", "1");
            }
        }

        public async void enterRoom()
        {

            ReceiveCall = true;
        }

        public void showCall(string _url)
        {
            Room = _url;
            //enterRoom();
        }

        public void showUri()
        {
            Launcher.OpenAsync(_uri);
        }

        private void Current_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e)
        {
            /*string room = "test";
            try
            {
                Room = e.Data["room_name"].ToString();
                CrossSettings.Current.AddOrUpdateValue("currentRoom", room);
                enterRoom();
            }
            catch (Exception ex)
            { }*/
        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            /*string room = "test";
            try
            {
                Room = e.Data["room_name"].ToString();
                CrossSettings.Current.AddOrUpdateValue("currentRoom", room);
                enterRoom();
            }
            catch (Exception ex)
            { }*/

        }

        /*private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            CrossSettings.Current.AddOrUpdateValue("FCMToken", e.Token);
            string deviceId = CrossDeviceInfo.Current.Id;
            if (!String.IsNullOrEmpty(e.Token))
                HttpControler.FCMTokenSend(Phone, e.Token, deviceId);
        }

        private void Current_OnNotificationAction(object source, FirebasePushNotificationResponseEventArgs e)
        {
            CrossFirebasePushNotification.Current.ClearAllNotifications();
            if (e.Identifier == "Accept")
            {
                string room = "test";
                try
                {
                    Room = e.Data["room_name"].ToString();
                    CrossSettings.Current.AddOrUpdateValue("currentRoom", room);
                    enterRoom();
                }
                catch (Exception ex)
                { }
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }

        public string RegStatus
        {
            get
            {
                return _regStatus;
            }
            set
            {
                if (_regStatus != value)
                {
                    _regStatus = value;
                    OnPropertyChanged("RegStatus");
                }
            }
        }

        public string VersionNumber
        {
            get
            {
                return _versionNumber;
            }
            set
            {
                if (_versionNumber != value)
                {
                    _versionNumber = value;
                    OnPropertyChanged("VersionNumber");
                }
            }
        }

        public string RegStatusIcon
        {
            get
            {
                return _regStatusIcon;
            }
            set
            {
                if (_regStatusIcon != value)
                {
                    _regStatusIcon = value;
                    OnPropertyChanged("RegStatusIcon");
                }
            }
        }

        public string Room
        {
            get
            {
                return _room;
            }
            set
            {
                if (_room != value)
                {
                    _room = value;
                    OnPropertyChanged("Room");
                    OnPropertyChanged("IsActiveCall");

                }
            }
        }

        public bool IsActiveCall
        {
            get
            {
                if (!String.IsNullOrEmpty(Room))
                    return true;
                else
                    return false;
            }
        }

        public bool ReceiveCall { get; set; }
    }
}
