using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OwnLinkJitsi
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        int ScheduleNotification(string title, string message);

        void ReceiveNotification(string title, string message);
    }

    public interface IPlaySoundService
    {
        void InitSystemSound();
        void PlaySystemSound();
        void StopSystemSound();
    }
    public interface ICallJournal
    {
        string GetLastNumber();
    }

    public interface ISpeakerPhone
    {
        void SpeakerphoneOn();
        void SpeakerphoneOff();
    }

    public interface IFCMService
    {
        string GetToken();
    }

    public interface IOpenSettings
    {
        void GoToSettings();
    }

    public interface IAppHandler
    {
        Task<bool> LaunchApp(string uri);
        Task<bool> CheckApp(string uri);
    }
}
