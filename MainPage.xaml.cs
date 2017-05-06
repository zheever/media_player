using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace MEDIA
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MediaInitial();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Media.NaturalDuration.TimeSpan.TotalSeconds == 0) return;
            this.Media.Play();
            this.TimeSlider.Maximum = (int)this.Media.NaturalDuration.TimeSpan.TotalSeconds;
            this.storyboard1.Begin();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this.Media.Stop();
            this.storyboard1.Stop();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Media.Pause();
            this.storyboard1.Pause();
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.Media.Volume = ((double)VolumeSlider.Value) / 100;
        }

        private void MediaInitial()
        {
            this.Media.Volume = 0.2;
            this.VolumeSlider.Value = 20;
            this.TimeSlider.Value = 0;
            MusicPlayInitial();
        }



        private async void PickMediaFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();

            // mediaPlayer is a MediaElement defined in XAML
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                
                this.Media.SetSource(stream, file.ContentType);

                if (file.ContentType == "audio/mpeg")
                {
                    MusicPlayInitial();
                } else
                {
                    VideoPlayInitial();
                }
            }

        }

        private void MusicPlayInitial()
        {
            this.DiskCanvas.Visibility = Visibility.Visible;
            this.Stick.Visibility = Visibility.Visible;
            this.MediaRelPanel.BorderThickness = new Thickness(0);
            //this.TimeSlider.Margin = new Thickness(20,0,0,0);
        }
        private void VideoPlayInitial()
        {
            this.DiskCanvas.Visibility = Visibility.Collapsed;
            this.Stick.Visibility = Visibility.Collapsed;
            this.MediaRelPanel.BorderThickness = new Thickness(1);
            //this.TimeSlider.Margin = new Thickness(0);
        }

        private void FullWindow_Click(object sender, RoutedEventArgs e)
        {

            this.Media.IsFullWindow = !this.Media.IsFullWindow;
        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }

        private void TimeSlider_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (this.Media.NaturalDuration.TimeSpan.TotalSeconds != 0) this.TimeSlider.Opacity = 1.0;
        }

        private void TimeSlider_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.TimeSlider.Opacity = 0;
        }



    }
}
