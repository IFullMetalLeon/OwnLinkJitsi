using Plugin.DeviceInfo;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OwnLinkJitsi.ViewModel
{
    public class CallPageViewModel : INotifyPropertyChanged
    {
        public string Room { get; set; }
        public ICommand CancelCall { get; set; }
        public ICommand AcceptCall { get; set; }
        public CallPageViewModel()
        {
            Name = CrossSettings.Current.GetValueOrDefault("callerName", "");
            Room = CrossSettings.Current.GetValueOrDefault("currentRoom", "");
            CancelCall = new Command(cancelCall);
            AcceptCall = new Command(accCall);
        }

        public void beginPage()
        {

        }

        public void endPage()
        {

        }

        public void cancelCall()
        {
            CrossSettings.Current.AddOrUpdateValue("RoomTime", "0");
            CrossSettings.Current.AddOrUpdateValue("currentRoom", "");
            CrossSettings.Current.AddOrUpdateValue("RoomNeedAcc", "");
            MessagingCenter.Send<string, string>("Call", "CallState", "End");
        }

        public void accCall()
        {
            string deviceId = CrossDeviceInfo.Current.Id;
            string phone = CrossSettings.Current.GetValueOrDefault("sipPhoneLogin", "");
            HttpControler.ReadySignSend(phone, deviceId, Room, "Accept");
            CrossSettings.Current.AddOrUpdateValue("RoomTime", "0");
            CrossSettings.Current.AddOrUpdateValue("currentRoom", "");
            CrossSettings.Current.AddOrUpdateValue("RoomNeedAcc", "");
            enterRoom();
        }

        public async void enterRoom()
        {
            await DependencyService.Get<IAppHandler>().LaunchApp(Room);           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string _name { get; set; }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }
}
