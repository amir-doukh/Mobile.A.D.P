using System;
using System.Collections.Generic;
using Atelier.Port.Models;
using Atelier.Port.ViewModels;
using Atelier.Port.Views;
using Xamarin.Forms;

namespace Atelier.Port
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
          
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            /* if(!string.IsNullOrEmpty(App.LiteDB.GetAllUser()[0].Nom))
             {
                 Nome_Menu.Text = App.LiteDB.GetAllUser()[0].Nom;
             }*/
            //Titre.Text= App.LiteDB.GetAllUser()[0].Nom;

            //Shell.FlyoutHeaderTemplate.SetValue();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Constants.Token = " ";
            App.LiteDB.DropAll();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
