using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atelier.Port.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListRemorques : ContentPage
    {
        public ListRemorques()
        {
            InitializeComponent();

            GetDetails();
        }
        public void GetDetails()
        {
            var ListRemorque = App.LiteDB.GetAllRemorque();
            if (ListRemorque.Count != 0)
            {
                listView.ItemsSource = ListRemorque;
            }
            else
            {
                List_vide.IsVisible = true;
            }
        }
    }
}