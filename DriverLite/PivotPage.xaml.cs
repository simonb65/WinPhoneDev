using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using DriverLite.DataModel;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DriverLite
{
    public class PivotPageViewModel
    {
        public ObservableCollection<MovementDetail> PendingAccepted { get; set; }
    }

    public class GpsLocData
    {
        public BasicGeoposition Position { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public sealed partial class PivotPage : Page
    {
        private Timer _gpsTimer;
        private Geolocator _geolocator;
        private Timer _gpsUpdateTimer;

        public PivotPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            DefaultViewModel = new PivotPageViewModel
            {
                PendingAccepted = DataSource.MovementDetails
            };

            DataContext = this;
            _geolocator = new Geolocator
            {
                DesiredAccuracyInMeters = 5,
                MovementThreshold = 10,
                DesiredAccuracy = PositionAccuracy.High
            };
            _geolocator.PositionChanged += GeolocationPositionChanged;

            _gpsTimer = new Timer(GpsTimerCallback, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
            _gpsUpdateTimer = new Timer(GpsUpdateTimerCallback, null, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
        }


        private Geocoordinate _lastGeocoordinate = null;

        void GeolocationPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Debug.WriteLine("GeolocationPositionChanged");
            _lastGeocoordinate = args.Position.Coordinate;
        }

        void GpsTimerCallback(object state)
        {
            var lastPos = (_lastGeocoordinate == null)
                ? "<starting>"
                : string.Format("{0},{1},{2:o}", _lastGeocoordinate.Point.Position.Latitude, _lastGeocoordinate.Point.Position.Longitude, _lastGeocoordinate.Timestamp);

            var now = DateTimeOffset.Now;
            Debug.WriteLine("GpsTimerCallback - " + now.ToString("o") + " => " + lastPos);

            if (_lastGeocoordinate != null)
            {
                if (_toUploadPositions == null)
                    _toUploadPositions = new List<GpsLocData>();

                _toUploadPositions.Add(new GpsLocData { Timestamp = now, Position = _lastGeocoordinate.Point.Position });
            }
        }

        private List<GpsLocData> _toUploadPositions;

        void GpsUpdateTimerCallback(object state)
        {
            var toUpload = _toUploadPositions;
            _toUploadPositions = null;

            if (toUpload == null)
                return;

            Debug.WriteLine("Uploading ...");
            var data = JsonConvert.SerializeObject(toUpload);
            Debug.WriteLine(data);
        }







        public PivotPageViewModel DefaultViewModel { get; set; }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var movementKey = ((MovementDetail)e.ClickedItem).MovementKey;
            if (!Frame.Navigate(typeof(ItemPage), movementKey))
            {
                throw new Exception("NavigationFailedExceptionMessage");
            }
        }

        private void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LogoutAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }    
    }
}
