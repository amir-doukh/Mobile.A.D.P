using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Atelier.Port.Models;
using Atelier.Port.Views;
using Atelier.Port.ViewModels;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Atelier.Port.Views
{
    public partial class ItemsPage : ContentPage
    {
        public string[] array_N_Remorque = new string[] { "" };

        private MediaFile file;
        ItemRemorque Info = new ItemRemorque();
        // public PhotoPage  photoPage=new PhotoPage();

        int i = 0;
        public ItemsPage()
        {
            InitializeComponent();
            var v = App.LiteDB.GetAllRemorque();
            Info.Pannes = new Identification_Panne();
            Info.Photos = new Identification_Photos();
            Info.Pannes.Identification_panne = new List<string> { };
            Info.Photos.Identification_photos = new List<string> { };
            //  BindingContext = viewModel = new ItemsViewModel();


        }

        protected override void OnAppearing()
        {

            //   Info.Photo.Identification_photos.Add(PhotoPage.);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image.IsVisible = e.Value;

            // Info.Pannes.Identification_panne.ToList().Add(label.Text);

        }

        private void CheckBox1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image1.IsVisible = e.Value;
            //  Info.Pannes.Identification_panne.ToList().Add(label.Text);
        }
        private async void image_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image, label);
            // listMateriel.Add(item);
        }

        public async Task chargerValue(ImageButton images, Label labels)
        {

            var photo = "";
            try
            {
                var str = labels.Text;
                // Page page = new PhotoPage(str);
                Info.Pannes.Identification_panne.Add(str);


                if (await DisplayAlert("Notification", "voullez vous choisir ", "Prendre une Photo", "Accés à la galerie"))
                {
                    photo = await TakePhotoL(str);
                }
                else
                {
                    photo = await GetPhoto();
                }

                images.Source = photo;

                Info.Photos.Identification_photos.Add(photo);

            }

            catch (Exception e)
            {
                await DisplayAlert("Alert", "probleme du camera", "cancel");
                Console.WriteLine("probleme:" + e);
            }

            //  return info;
        }
        public async Task<string> TakePhotoL(string val)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("ALERT", "Camera non detectée", "OK");
                file.AlbumPath = "camera_icon.png";
            }
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }
            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 40,
                    Name = val + "-" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ".jpg",
                    Directory = "sample"
                });
                if (file == null) file.AlbumPath = "camera_icon.png";
            }

            else
            {
                await DisplayAlert("ALERT", "imposible de prendre photo", "OK");
            }

            return file.AlbumPath;
        }
        public async Task<string> GetPhoto()
        {
            var ter = "camera_icon.png";
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("ALERT", "Camera non detectée", "OK");

                return ter;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 30,


                };
                file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)

                    return ter;
            }

            return file.Path;
        }


        private async void image1_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image1, label1);
            //listMateriel.Add(item);
        }

        private void CheckBox_Details1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

            image_Detail_1.IsVisible = e.Value;

        }

        private async void image_Details_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image1, label1);

        }

        private void CheckBox_Details2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail_2.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
        {
            image1.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_2(object sender, CheckedChangedEventArgs e)
        {
            image2.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_3(object sender, CheckedChangedEventArgs e)
        {
            image3.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_4(object sender, CheckedChangedEventArgs e)
        {

            image4.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_5(object sender, CheckedChangedEventArgs e)
        {
            image5.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_6(object sender, CheckedChangedEventArgs e)
        {

            image6.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_7(object sender, CheckedChangedEventArgs e)
        {
            image7.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_8(object sender, CheckedChangedEventArgs e)
        {
            image8.IsVisible = e.Value;
        }

        private void CheckBox_CheckedChanged_9(object sender, CheckedChangedEventArgs e)
        {
            image9.IsVisible = e.Value;
        }

        private async void image_Details_1_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail_1, label_Detail_1);
        }

        private async void image_Details_2_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail_2, label_Detail_2);

        }

        private async void image2_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image2, label2);
        }

        private async void image3_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image3, label3);
        }

        private async void image4_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image4, label4);

        }

        private async void image5_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image5, label5);

        }

        private async void image6_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image6, label6);

        }

        private async void image7_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image7, label7);

        }

        private async void image8_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image8, label8);

        }

        private async void image9_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image9, label9);

        }


        private async void Button_Clicked(object sender, EventArgs e)
        {

            try
            {
                Info.Numero_Remorque = SuggestBox1.Text;
                Info.Email_Client = App.LiteDB.GetEmailByNbRemorque(SuggestBox1.Text);
                Info.Nom_Client = App.LiteDB.GetNomClientByNbRemorque(SuggestBox1.Text);

                if (Info.Photos == null)
                {
                    Info.Photos = new Identification_Photos();
                    Info.Photos.Identification_photos = new List<string> { "Image_inconnu.png" };
                }
                App.LiteDB.AddEmails(Info);
                // App.LiteDB.AddEmails(EmailItem);
                await DisplayAlert("Base local", "   Email est ajouté en local", "validé");
                //App.Current.MainPage = new MainPage();
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("probleme ::: " + ex);
                await DisplayAlert("Base local", "   Email n'est pas ajouté en local", "validé");
            }
        }

        private void CheckBox_Details3_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail_3.IsVisible = e.Value;
        }

        private async void image_Details_3_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail_3, label_Detail_3);

        }

        private void CheckBox_Details4_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail_4.IsVisible = e.Value;
        }

        private async void image_Details_4_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail_4, label_Detail_4);

        }
        private void CheckBox_Details5_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail_5.IsVisible = e.Value;
        }

        private async void image_Details_5_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail_5, label_Detail_5);

        }

        private void CheckBox_Details6_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail_6.IsVisible = e.Value;
        }
        private async void image_Details_6_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail_6, label_Detail_6);

        }

        private void CheckBox_Details2_1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail2_1.IsVisible = e.Value;

        }

        private async void image_Details2_1_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail2_1, label_Detail2_1);


        }

        private void CheckBox_Details2_2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail2_2.IsVisible = e.Value;
        }

        private async void image_Details2_2_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail2_2, label_Detail2_2);

        }

        private void CheckBox_Details2_3_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail2_3.IsVisible = e.Value;
        }

        private async void image_Details2_3_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail2_3, label_Detail2_3);

        }

        private void CheckBox_Details3_1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail3_1.IsVisible = e.Value;
        }

        private async void image_Details3_1_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail3_1, label_Detail3_1);

        }
        private void CheckBox_Details3_2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail3_2.IsVisible = e.Value;
        }
        private async void image_Details3_2_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail3_2, label_Detail3_2);

        }
        private void CheckBox_Details3_3_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            image_Detail3_3.IsVisible = e.Value;
        }
        private async void image_Details3_3_Clicked(object sender, EventArgs e)
        {
            await chargerValue(image_Detail3_3, label_Detail3_3);

        }

        private void imageexp_Clicked(object sender, EventArgs e)
        {
            Details.IsVisible = !Details.IsVisible;
            if (Details.IsVisible == true)
            {
                imageexp.Source = "dexpand.png";
            }
            else
            {
                imageexp.Source = "expand_icon.png";
            }
        }

        private void imageexp1_Clicked(object sender, EventArgs e)
        {
            Details2.IsVisible = !Details2.IsVisible;
            if (Details2.IsVisible == true)
            {
                imageexp2.Source = "dexpand.png";
            }
            else
            {
                imageexp2.Source = "expand_icon.png";
            }
        }

        private void imageexp2_Clicked(object sender, EventArgs e)
        {
            Details3.IsVisible = !Details3.IsVisible;
            if (Details3.IsVisible == true)
            {
                imageexp3.Source = "dexpand.png";
            }
            else
            {
                imageexp3.Source = "expand_icon.png";
            }
        }

        private void imageexp4_Clicked(object sender, EventArgs e)
        {
            Details4.IsVisible = !Details4.IsVisible;
            if (Details4.IsVisible == true)
            {
                imageexp4.Source = "dexpand.png";
            }
            else { imageexp4.Source = "expand_icon.png"; }

        }

        private void SuggestBox1_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            var val = SuggestBox1.Text;
            array_N_Remorque = App.LiteDB.GetNbRemorque();
            var suggetion = array_N_Remorque.Where(c => c.Contains(val));
            SuggestBox1.ItemsSource = suggetion.ToList();
            EmailTo.Text = App.LiteDB.GetEmailByNbRemorque(SuggestBox1.Text);
        }
        private void Etat_Envoi_OnChanged(object sender, ToggledEventArgs e)
        {
            Info.Envoi_Email_Client = e.Value;
        }


    }
}