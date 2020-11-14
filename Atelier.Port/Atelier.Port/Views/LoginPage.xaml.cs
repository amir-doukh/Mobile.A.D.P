using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Atelier.Port.Models;
using Atelier.Port.ViewModels;
using GraphQL.Client.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atelier.Port.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
           // this.BindingContext = new LoginViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
      /*  private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }*/

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Alerte", " Vérifier votre connectivite internet", "Valider");
            }

        }

        public async Task LoadDataAsync()
        {
            WaitLoad.IsVisible = true;
            log.IsEnabled = false;

            try
            {
                if (!string.IsNullOrEmpty(Nom.Text) && !string.IsNullOrEmpty(Password.Text))
                {
                    var NewUser = new { identifier = Nom.Text.Replace(" ", string.Empty), password =Password.Text.Replace(" ", string.Empty) };
                    string message = " ";
                    string token = " ";
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var json = JsonConvert.SerializeObject(NewUser);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var result = await client.PostAsync(Constants.URL_AUTH, content);
                            message = result.StatusCode.ToString();
                            var resultString = await result.Content.ReadAsStringAsync();
                            JObject val = JObject.Parse(resultString);
                            Constants.Token = val["jwt"].ToString();
                            token = Constants.Token;
                            Constants.Nom = val["user"]["username"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (message == "BadRequest")
                        {

                            await Application.Current.MainPage.DisplayAlert("Alerte", "La combinaison nom d'utilisateur ou le mot de passe est incorrect réessayez", "Valider");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Alerte", "Problème de connexion au serveur", "Valider");
                            Console.WriteLine("probleme::" + ex);
                        }

                        WaitLoad.IsVisible = false;
                        log.IsEnabled = true;
                    }
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        Personel person = new Personel()
                        {
                            Token = Constants.Token,
                            Nom = Constants.Nom
                        };
                        App.LiteDB.AddPersonel(person);
                        GetListRemorque();
                        App.Current.MainPage = new AppShell();
                        //  await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");

                    }
                }
                else
                {
                    WaitLoad.IsVisible = false;
                    log.IsEnabled = true;
                    await Application.Current.MainPage.DisplayAlert("Alerte", "    Email ou mot de passe est incorrect", " Valider");

                }

            }
            catch (Exception e)
            {
                WaitLoad.IsVisible = false;
                log.IsEnabled = true;
                Console.WriteLine("Probleme de connexion ;   " + e);
                // await Application.Current.MainPage.DisplayAlert("Alerte", " Vérifier votre connectivite internet", "Valider");
            }
        }
       
        public async void GetListRemorque()
        {
            try
            {
                var graphQLClient = new GraphQLHttpClient(Constants.URL);
                graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + App.LiteDB.GetAllUser()[0].Token);
                GraphQL.GraphQLRequest MyRequest = new GraphQL.GraphQLRequest()
                {
                    Query = "query{remorques(where:{isActive:true}){Numero_Remorque Nom_Client Email_Client Photos}}",

                };
                var graphResponse = await graphQLClient.SendQueryAsync<object>(MyRequest);
                var Data = graphResponse.Data;
                JObject _Data = JObject.Parse(Data.ToString());
                IList<JToken> results = _Data["remorques"].Children().ToList();
                List<Identification_Remorque> listMarque2 = new List<Identification_Remorque>();
                foreach (JToken result in results)
                {
                    Identification_Remorque referances = result.ToObject<Identification_Remorque>();
                    listMarque2.Add(referances);
                }
                foreach (var c in listMarque2)
                {
                    App.LiteDB.AddRemorque(c);
                }
                var v = App.LiteDB.GetAllRemorque();

            }
            catch (Exception e)
            {
                Console.WriteLine("probleme:" + e.Message);
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected == true)
            {
                Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
                await LoadDataAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerte", " Vérifier votre connectivite internet", "Valider");
            }
        }
    }
}
