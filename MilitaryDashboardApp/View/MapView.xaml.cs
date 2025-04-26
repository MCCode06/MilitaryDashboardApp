using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using MilitaryDashboardApp.Models;
using static GMap.NET.Entity.OpenStreetMapGraphHopperRouteEntity;
using Geometry = System.Windows.Media.Geometry;

namespace MilitaryDashboardApp.View
{
    public partial class MapView : UserControl
    {
        private ObservableCollection<Unit> _units;
        private ObservableCollection<Destination> _destinations;

        public MapView()
        {
            InitializeComponent();
            InitializeMap();
        }

        private void InitializeMap()
        {
            MapControl.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            MapControl.Position = new PointLatLng(40.4093, 49.8671); // Baku
            MapControl.MinZoom = 1;
            MapControl.MaxZoom = 18;
            MapControl.Zoom = 10;
            MapControl.ShowCenter = false;
            MapControl.CanDragMap = true;
            MapControl.DragButton = System.Windows.Input.MouseButton.Left;
            MapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            MapControl.IgnoreMarkerOnMouseWheel = true;

            


            _units = UnitRegistry.Instance.Units;
            _destinations = new ObservableCollection<Destination>();
            _units.CollectionChanged += (s, e) => RefreshMarkers();
            RefreshMarkers();
        }

        private Unit _selectedUnit;

        public void SetSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;
            if (_selectedUnit != null)
            {
                MapControl.MouseLeftButtonDown += MapControl_MouseLeftButtonDown;
            }
            else
            {
                MapControl.MouseLeftButtonDown -= MapControl_MouseLeftButtonDown;
            }
        }


        public void AddDestination(Destination destination)
        {
            _destinations.Add(destination);
            RefreshMarkers();
        }

        private void MapControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_selectedUnit == null) return;

            var point = e.GetPosition(MapControl);
            var latLng = MapControl.FromLocalToLatLng((int)point.X, (int)point.Y);

            var destination = new Destination(_selectedUnit, latLng.Lat, latLng.Lng);
            _destinations.Add(destination);
            RefreshMarkers(); 
        }


        public void RefreshMarkers()
        {
            try
            {
                MapControl.Markers.Clear();

                foreach (var unit in _units)
                {
                    if (!double.IsNaN(unit.Latitude) && !double.IsNaN(unit.Longitude))
                    {
                        var marker = CreateMarker(unit); 
                        if (marker != null)
                        {
                            MapControl.Markers.Add(marker);
                        }
                    }
                }
                foreach (var destination in _destinations)
                {
                    var destinationMarker = CreateDestinationPin(destination);
                    if (destinationMarker != null)
                    {
                        MapControl.Markers.Add(destinationMarker);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing markers: {ex.Message}");
            }
        }

        private DispatcherTimer _marchingTimer;

        public void StartMarching()
        {
            bool anyUnitsMarching = _units.Any(u => u.IsMarching);

            if (!anyUnitsMarching)
            {
                Debug.WriteLine("No units are marching.");
                return;
            }

            if (_marchingTimer == null)
            {
                _marchingTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100) 
                };
                _marchingTimer.Tick += MarchingTimer_Tick;
            }

            if (!_marchingTimer.IsEnabled)
            {
                _marchingTimer.Start();
                Debug.WriteLine("Marching timer started.");
            }
        }

        private void MarchingTimer_Tick(object sender, EventArgs e)
        {
            bool anyStillMarching = false;

            foreach (var unit in _units)
            {
                if (unit.IsMarching)
                {
                    MoveUnitTowardsDestination(unit);
                    if (unit.IsMarching) 
                    {
                        anyStillMarching = true;
                    }
                }
            }

            RefreshMarkers();

            if (!anyStillMarching)
            {
                _marchingTimer?.Stop();
                Debug.WriteLine("Marching timer stopped - all units arrived.");
            }
        }



        private void MoveUnitTowardsDestination(Unit unit)
        {
            double currentLat = unit.Latitude;
            double currentLng = unit.Longitude;
            double targetLat = unit.DestinationLat;
            double targetLng = unit.DestinationLng;

            double dLat = targetLat - currentLat;
            double dLng = targetLng - currentLng;
            double distance = Math.Sqrt(dLat * dLat + dLng * dLng);

            double arrivalThreshold = 0.0005;  
            double stepSize = 0.005;           

            if (distance <= arrivalThreshold)
            {
                unit.Latitude = targetLat;
                unit.Longitude = targetLng;
                unit.IsMarching = false;
                unit.Status = "Arrived";
                Debug.WriteLine($"{unit.Name} has arrived.");
                return;
            }

            double directionLat = dLat / distance;
            double directionLng = dLng / distance;

            double moveDistance = Math.Min(stepSize, distance);
            double nextLat = currentLat + directionLat * moveDistance;
            double nextLng = currentLng + directionLng * moveDistance;

            unit.Trail.Add(new PointLatLng(unit.Latitude, unit.Longitude));
            unit.Latitude = nextLat;
            unit.Longitude = nextLng;

            Debug.WriteLine($"{unit.Name} moved to ({unit.Latitude:F6}, {unit.Longitude:F6})");
            Debug.WriteLine($"Remaining distance: {distance - moveDistance:F6}");

            RefreshMarkers();
        }




        private GMapMarker CreateMarker(Unit unit)
        {
            var marker = new GMapMarker(new PointLatLng(unit.Latitude, unit.Longitude))
            {
                Shape = new Ellipse
                {
                    Width = 12,
                    Height = 12,
                    Fill = unit.Status == "Arrived" ? Brushes.Gray : Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    ToolTip = unit.Name
                }
            };
            return marker;
        }

        private GMapMarker CreateDestinationPin(Destination destination)
        {
            return new GMapMarker(new PointLatLng(destination.Latitude, destination.Longitude))
            {
                Shape = new System.Windows.Shapes.Path
                {
                    Fill = Brushes.Blue,
                    Stroke = Brushes.White,
                    StrokeThickness = 1,
                    Data = Geometry.Parse("M 0 0 L 4 10 L -4 10 Z"), 
                    ToolTip = $"{destination.Unit.Name} destination"  
                },
                Offset = new System.Windows.Point(-4, -10)  
            };
        }

    }
}
