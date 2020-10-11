using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Atelier.Port.Services;
using Atelier.Port.Views;
using System.IO;

namespace Atelier.Port
{
    public partial class App : Application
    {
        static LiteDBHelper db;
        public App()
        {
            InitializeComponent();

           // DependencyService.Register<MockDataStore>();
           // MainPage = new AppShell();
        }
        public static LiteDBHelper LiteDB
        {
            get
            {
                if (db == null)
                {
                    db = new LiteDBHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Materiel_Email.db"));
                }
                return db;
            }
        }
        protected override void OnStart()
        {
            Verification();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public void Verification()
        {
            try
            {
                DependencyService.Register<MockDataStore>();
                var Token = App.LiteDB.GetAllUser()[0].Token;
                if (!String.IsNullOrEmpty(Token))
                {
                    MainPage = new AppShell();
                }
                else
                {
                    // App.LiteDB.DropAll();
                    MainPage = new NavigationPage(new LoginPage());

                }
            }
            catch (Exception)
            {
                MainPage = new NavigationPage(new LoginPage());
            }

        }
    }
}
