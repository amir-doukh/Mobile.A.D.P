using Atelier.Port.Models;
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
    public partial class VisualisationPage : ContentPage
    {
        public VisualisationPage(ItemRemorque info)
        {
            InitializeComponent();
            cvBanners.ItemsSource = info.Photos.Identification_photos;
            N_Remorque.Text = info.Numero_Remorque;
            NOM_Client.Text = info.Nom_Client;
            foreach (var c in info.Pannes.Identification_panne)
            {
                Affichage.Children.Add(new Label { Text = "-" + c, FontSize = 20 });
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync();
        }
    }
}