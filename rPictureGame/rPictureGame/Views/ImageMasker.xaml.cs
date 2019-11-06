using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Picturegame.Views;
using System.Drawing;
using System.IO;
using rPictureGame.Interfaces;

namespace Picturegame.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageMasker : ContentPage
    {
        public ImageMasker()
        {
            InitializeComponent();
        }

        private async void ImageBtn_Clicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                image.Source = ImageSource.FromStream(() => stream);
            }

            (sender as Button).IsEnabled = true;
        }
    }
}