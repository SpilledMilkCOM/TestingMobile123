using System;
using System.ComponentModel;
using System.Windows.Input;

using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml.Media.Imaging;

using SM.Common;

namespace SM.WindowsUniversal.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _latLong;
		private BitmapImage _loadedImage;

		public MainPageViewModel()
		{
			LaunchMapsCommand = new RelayCommand(LaunchMaps);
			LoadLocationCommand = new RelayCommand(LoadLocation);
			LoadPictureCommand = new RelayCommand(LoadPicture);
		}

		public string LatitudeLongitude
		{
			get { return _latLong; }
			set
			{
				_latLong = value;
				OnPropertyChanged(nameof(LatitudeLongitude));
			}
		}

		public ICommand LaunchMapsCommand { get; private set; }

		public ICommand LoadLocationCommand { get; private set; }

		public ICommand LoadPictureCommand { get; private set; }

		public BitmapImage LoadedImage
		{
			get { return _loadedImage; }
			private set
			{
				_loadedImage = value;
				OnPropertyChanged(nameof(LoadedImage));
			}
		}

		private async void LaunchMaps()
		{
			// A link to descriptions of other URI schemes...
			// https://msdn.microsoft.com/en-us/windows/uwp/launch-resume/launch-default-app
			// https://msdn.microsoft.com/en-us/windows/uwp/launch-resume/launch-maps-app

			// Center on New York City
			// var uriNewYork = new Uri(@"bingmaps:?cp=40.726966~-74.006076");

			var centerParam = LatitudeLongitude?.Replace(" ", string.Empty).Replace(",", "~");

			if (string.IsNullOrEmpty(centerParam))
			{
				// Just launch the map app...

				await Launcher.LaunchUriAsync(new Uri("bingmaps:"));
			}
			else
			{
				// Uses tilde ~
				//await Launcher.LaunchUriAsync(new Uri($"bingmaps:cp={param}&lvl=10"));

				var pointParam = centerParam.Replace("~", "_");

				await Launcher.LaunchUriAsync(new Uri($"bingmaps:cp=point.{pointParam}_Test%20Point&cp={centerParam}&lvl=16"));
			}
		}

		private async void LoadLocation()
		{
			var geolocator = new Geolocator();

			var position = await geolocator.GetGeopositionAsync();

			LatitudeLongitude = $"{position.Coordinate.Point.Position.Latitude} , {position.Coordinate.Point.Position.Longitude}";
		}

		private async void LoadPicture()
		{
			var picker = new FileOpenPicker();

			picker.ViewMode = PickerViewMode.Thumbnail;
			picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

			picker.FileTypeFilter.Add(".jpg");
			picker.FileTypeFilter.Add(".jpeg");
			picker.FileTypeFilter.Add(".png");

			StorageFile file = await picker.PickSingleFileAsync();

			if (file != null)
			{
				using (var stream = await file.OpenAsync(FileAccessMode.Read))
				{
					var bitmap = new BitmapImage();

					await bitmap.SetSourceAsync(stream);

					LoadedImage = bitmap;
				}
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			if (propertyName != null && PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}