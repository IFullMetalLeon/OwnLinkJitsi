using OwnLinkJitsi.ViewModel;
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


        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            mdmvm.endPage();
            
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

        private void showCall(string content)
        {
            Detail = new NavigationPage(new CallPage(content));
            IsPresented = false;
        }
    }
}