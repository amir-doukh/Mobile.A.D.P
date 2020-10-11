using System.ComponentModel;
using Xamarin.Forms;
using Atelier.Port.ViewModels;

namespace Atelier.Port.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}