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
    public partial class HistoryCallPage : ContentPage
    {
        public HistoryCallPageViewModel hcpvm { get; set; }
        public HistoryCallPage()
        {
            InitializeComponent();
            hcpvm = new HistoryCallPageViewModel();
            this.BindingContext = hcpvm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            hcpvm.startPage();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            hcpvm.endPage();
        }
    }
}