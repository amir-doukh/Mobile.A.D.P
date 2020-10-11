using Atelier.Port.Models;
using Firebase.Storage;
using GraphQL;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atelier.Port.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListEmailsPage : ContentPage
    {
        List<ItemRemorque> listeEmail = new List<ItemRemorque>();
       // ceci la liste fixe des email qui sont toujours l'envoi
        List<string> SendToCC = new List<string> { "amir-doukh@protonmail.com" };
        public ListEmailsPage()
        {
            InitializeComponent();
            LoadList();
        }

        public async void LoadList()
        {

            listView.IsVisible = false;
            //WaitLoad.IsVisible = true;
            //refresh listes 
            listView.RefreshCommand = new Command(async (obj) =>
            {
                GetDetails();
                listView.IsRefreshing = false;
                // WaitLoad.IsVisible = false;
                listView.IsVisible = true;
            });
            //loading = true;
            listView.IsRefreshing = false;
            GetDetails();
            // WaitLoad.IsVisible = false;
            listView.IsVisible = true;
            await Task.Delay(2500);
            LabelConnection.Text = "Vous etes connectés";
            LabelConnection.TextColor = Color.Green;
        }

        public void GetDetails()
        {
            try
            {
                List<ItemRemorque> ListAffiche = App.LiteDB.GetAllEmails();
                if (ListAffiche.Count != 0)
                {
                    listView.ItemsSource = ListAffiche;
                }
                else
                {
                    List_vide.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("probleme::::::::" + ex);
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                var infos = ((ListView)sender).SelectedItem as ItemRemorque;
                var page = new NavigationPage(new VisualisationPage(infos));
                Label mylab = new Label();
                mylab.Text = infos.Numero_Remorque;
                mylab.FontSize = 20;
                mylab.TextColor = Color.White; ;
                NavigationPage.SetTitleView(page, mylab);
                this.Navigation.PushModalAsync(page);
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;

            var ob = checkbox.BindingContext as ItemRemorque;

            if (ob != null)
            {
                envoi.IsVisible = e.Value;
                if (checkbox.IsChecked)
                {

                    listeEmail.Add(ob);
                }
            }
            Console.WriteLine("probleme:" + listeEmail);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected == true)
                {
                    if (listeEmail.Count != 0)
                    {
                        int i = 0;
                        string val = "";
                        var Photos = new List<string>();
                        foreach (var item in listeEmail)
                        {
                            var emailMessenger = CrossMessaging.Current.EmailMessenger;
                            if (emailMessenger.CanSendEmailAttachments)
                            {
                                if (emailMessenger.CanSendEmail)
                                {
                                    // file = new File("<path_to_file>");

                                    Photos = item.Photos.Identification_photos.ToList();
                                    i++;
                                    string ConvertDetails = string.Join("\n -", item.Pannes.Identification_panne);

                                    val += "Bonjour, \n Nous vous informons  que votre remorque porte les problèmes suivantes: \n-" + ConvertDetails + "\n";
                                    if (item.Envoi_Email_Client == true)
                                    {
                                        SendToCC.Add(item.Email_Client);
                                    }

                                    string EmailCC = string.Join(",", SendToCC.ToArray());
                                    string ConvertPhoto = JsonConvert.SerializeObject(Photos);
                                    var email = new EmailMessageBuilder()
                                        .Subject("Information sur la remorque numéro: " + item.Numero_Remorque)
                                        .Body(val)
                                        .To("amirdoukh400@gmail.com")
                                        .Cc(EmailCC)
                                        .Build();
                                    foreach (var c in Photos)
                                    {
                                        email.Attachments.Add(new Plugin.Messaging.EmailAttachment(c, "Content-Type: image/jpeg"));
                                    }
                                    emailMessenger.SendEmail(email);
                                }
                            }
                            App.LiteDB.DelateEmails(item._id);
                            await SendData(item);
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Mode deconnectée", " Verifier votre connexion internet et réessayer", "Valider");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("probleme::::::" + ex);
            }
        }

        public async Task SendData(ItemRemorque item)
        {
            try
            {
                await  UploadPhotos(item);
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                var graphQLClient = new HttpClient();
                graphQLClient.DefaultRequestHeaders.Add("Authorization", "bearer " + App.LiteDB.GetAllUser()[0].Token);
                graphQLClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                GraphQLRequest MyRequest = new GraphQLRequest()
                {
                    Query = "mutation{createCollectionPanne(input:{data:{Numero_Remorque:\"" + item.Numero_Remorque + "\"  Email_Client:\"" + item.Email_Client + "\" Nom_Client:\"" + item.Nom_Client + "\"created_by:\"" + App.LiteDB.GetAllUser()[0].Nom + "\"   Pannes:{ Identification_Panne:" + JsonConvert.SerializeObject(item.Pannes.Identification_panne) + "\"   Photos:{ Identification_photos:" + JsonConvert.SerializeObject(item.Photos.Identification_photos) + "} }}){collectionPanne{id }}}",
                };
                var content = new StringContent(JsonConvert.SerializeObject(MyRequest), Encoding.UTF8, "application/json");
                var response = await graphQLClient.PostAsync(Constants.URL, content);
                var json = await response.Content.ReadAsStringAsync();
                // var graphResult = JsonConvert.DeserializeObject<GraphResult<T>(json);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    await DisplayAlert("Notification", "Votre message est bien envoyé", "ok");
                }
                // return graphResult.Data;
            }
            catch (Exception e)
            {
                Console.WriteLine("probleme en >>>>>>>>: " + e);
            }

        }


        public async Task UploadPhotos(ItemRemorque item)
        {
            try
            {
                List<string> ListUrl = new List<string>();
                var listAEnvoi = item.Photos.Identification_photos;
                foreach (var val in listAEnvoi)
                {
                    FileStream fs = File.OpenRead(val);
                    ListUrl.Add(await StoreImages(fs, Path.GetFileName(val)));
                }

                item.Photos.Identification_photos.Clear();
                item.Photos.Identification_photos.AddRange(ListUrl);
                App.LiteDB.UpdatEmails(item);
            }

            catch (Exception e)
            {
                Console.WriteLine("probleme en >>>>>>>>: " + e);
            }

        }
        public async Task<string> StoreImages(Stream imageStream, string fileName)
        {
            var stroageImage = await new FirebaseStorage("atelier-port.appspot.com")
                .Child("Upload")
                .Child(fileName)
                .PutAsync(imageStream);
            return stroageImage;
        }
    }
}