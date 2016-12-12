# TestingMobile123
Testing mostly Xamarin to see which platforms (Windows, iOS, Android, Web?) I can deploy to AND to see how much code sharing there is.

Loading a Picture
-----------------

> Uses the FileOpenPicker(), StorageFile.OpenAsync(), and BitmapImage.SetSourceAsync() to load a picture
> and assign it to a property to bind to the UI.

Geolocation
-----------

> Uses Geolocator.GetGeopositionAsync() to retrieve the user's location and display it on the main page.

Launch Bing Maps
----------------

> Uses Launcher.LaunchUriAsync(new Uri("bingmaps:")) to launch the Windows Store Maps application with the
> location found (or entered) into the Lat/Long fields.