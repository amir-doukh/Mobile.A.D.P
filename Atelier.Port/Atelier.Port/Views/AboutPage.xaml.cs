using Atelier.Port.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atelier.Port.Views
{
    public partial class AboutPage : ContentPage
    {
        private MediaFile file;
        public Identification_Remorque item = new Identification_Remorque();
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (IsValidEmail(id_email.Text) == true)
            {
                item.Numero_Remorque = nb_remorque.Text;
                item.Email_Client = id_email.Text;
                item.Nom_Client = Nom_Client.Text;
                App.LiteDB.AddRemorque(item);
                await DisplayAlert("Ajout", "Remorque Ajouté", "validé");
               // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
               App.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlert("Notification", "Veuillez verifier email ", "Validé");
            }


        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async void image_boutton_Clicked(object sender, EventArgs e)
        {
            item.Photos = new Identification_Photos();
            item.Photos.Identification_photos = new List<string>();
            if (await DisplayAlert("Photo", " vous voullez choisir?", "camera", "Galerie"))
            {
                var ter = await TakePhotoL();
                image_boutton.Source = ter;


                item.Photos.Identification_photos.Add(ter);
            }
            else
            {
                var v = await GetPhoto();

                image_boutton.Source = v;
                item.Photos.Identification_photos.Add(v);


            }

        }

        public async Task<string> GetPhoto()
        {
            var ter = "Image_inconnu.png";
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                // await DisplayAlert("Error", "le ", "OK");

                return ter;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)

                    return ter;
            }

            return file.Path;
        }
        public async Task<string> TakePhotoL()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("ALERT", "Camera non detectée", "OK");
                file.AlbumPath = "Image_inconnu.png";
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
                    Name = "Remorque.jpg",
                    Directory = "sample"
                });
                if (file == null) file.AlbumPath = "Image_inconnu.png";
            }

            else
            {
                await DisplayAlert("ALERT", "imposible de prendre photo", "OK");
            }

            return file.AlbumPath;
        }
    }
}