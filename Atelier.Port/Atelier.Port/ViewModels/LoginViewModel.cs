using Atelier.Port.Models;
using Atelier.Port.Views;
using GraphQL.Client.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Atelier.Port.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {   public bool _WaitLoad { get; set; } 
        public bool _log { get; set; } 
        public bool IsPassword { get; set; } = true;
        public Command LoginCommand { get; }
        public Command ShowPassword { get; }
        private string ShowPasswordText { get; set; } = "VOIR";
        private string _Password { get; set; }
        private string _Nom { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);

        }
        //ShowPassword = new Command(ShowPassword_Clicked);
        public bool WaitLoad
        {
            get
            {
                return _WaitLoad;
            }
            set
            {
                WaitLoad= _WaitLoad ;
            }
        }
        public bool log
        {
            get
            {
                return _log;
            }
            set
            {
                 _log = value; ;
            }
           
        }
        public string Nom
        {
            get
            {
                if (_Nom == null)
                    return "";
                else
                    return _Nom.ToString();
            }

            set
            {
                try
                {
                    _Nom = value;
                }
                catch
                {
                    _Nom = null;
                }
            }
        }
        public string Password
        {
            get
            {
                if (_Password == null)
                    return "";
                else
                    return _Password.ToString();
            }

            set
            {
                try
                {
                    _Password = value;
                }
                catch
                {
                    _Password = null;
                }
            }
        }

        /*   private void ShowPassword_Clicked(object sender)
           {
               if (!string.IsNullOrWhiteSpace(Password))
               {
                   IsPassword = !IsPassword;
                   if (ShowPasswordText == "VOIR")
                   {
                       ShowPasswordText = "VOIR PLUS";
                   }
                   else if (ShowPasswordText == "VOIR PLUS")
                   {
                       ShowPasswordText = "VOIR";
                   }
               }
           }*/
        private async void OnLoginClicked(object obj)
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
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Alerte", " Vérifier votre connectivite internet", "Valider");
            }

        }

        public async Task LoadDataAsync()
        {
            _WaitLoad = true;
            _log = false;

            try
            {
                if (!string.IsNullOrEmpty(_Nom) && !string.IsNullOrEmpty(_Password))
                {
                    var NewUser = new { identifier = _Nom.Replace(" ", string.Empty), password = _Password.Replace(" ", string.Empty) };
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

                        _WaitLoad = false;
                        _log = true;
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
                    await Application.Current.MainPage.DisplayAlert("Alerte", "    Email ou mot de passe est incorrect", " Valider");

            }
            catch (Exception e)
            {
                _WaitLoad = false;
                _log = true;
                Console.WriteLine("Probleme de connexion ;   " + e);
               // await Application.Current.MainPage.DisplayAlert("Alerte", " Vérifier votre connectivite internet", "Valider");
            }
        }
       /* public async Task<string> Auth(object user)
        {
            string message = " ";
            string token = " ";
            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(user);
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
                    Console.WriteLine("probleme::"+ex);
                }

                WaitLoad = false;
                log = true;
            }

            return token;
        }*/
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
    }
}
