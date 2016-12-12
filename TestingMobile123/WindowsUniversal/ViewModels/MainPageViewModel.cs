using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;

using SM.Common;

namespace SM.WindowsUniversal.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private BitmapImage _loadedImage;

		public MainPageViewModel()
		{
			LoadPictureComand = new RelayCommand(LoadPicture);
		}

		public ICommand LoadPictureComand { get; private set; }

		public BitmapImage LoadedImage
		{
			get { return _loadedImage; }
			private set
			{
				_loadedImage = value;
				OnPropertyChanged(nameof(LoadedImage));
			}
		}

		void OnPropertyChanged(string propertyName)
		{
			if (propertyName != null)
			{
				PropertyChangedEventHandler handler = PropertyChanged;

				if (handler != null)
				{
					handler(this, new PropertyChangedEventArgs(propertyName));
				}
			}
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
	}
}