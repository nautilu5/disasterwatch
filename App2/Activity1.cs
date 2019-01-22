using System;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace App2.Resources.layout
{
    [Activity(Label = "Activity1")]
    public class Activity1 : AppCompatActivity, IOnMapReadyCallback, ILocationListener
    {
        double latitude;
        Location currentLocation;
        double longitude;
        string permissionFine;
        string permissionCoarse;

        int tagcode;
        
        private async Task<JsonValue> FetchDBAsync(string url, string method)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = method;

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                    return jsonDoc;
                }
            }
        }

        public async void pinproccessor(GoogleMap map)
        {

            string fullname = Intent.GetStringExtra("fullname") ?? "0";
            string email = Intent.GetStringExtra("email") ?? "0";
            string url = "http://35.22.42.160:3000/users?fullname=" + fullname + "&email=" + email;
            JsonValue doc = await FetchDBAsync(url, "POST");
            JsonArray jsonarray = doc as JsonArray;
            foreach (JsonValue item in jsonarray)
            {
                String name = item["name"];
                Double Lat = item["latitude"];
                Double Long = item["longitude"];
                String type = item["type"];
                pinplacer(map, name, Lat, Long, type);
            }
        }

        public void pinplacer(GoogleMap map, string name, Double Lat, Double Long, string type)
        {
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(new LatLng(Lat, Long));
            markerOpt1.SetTitle(name);
            if (name == "Water Hazard")
            {
                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure);
                markerOpt1.SetIcon(bmDescriptor);
            }
            else if (name == "Fire Hazard")
            {
                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed);
                markerOpt1.SetIcon(bmDescriptor);
            }
            else if (name == "Debris Hazard")
            {
                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueYellow);
                markerOpt1.SetIcon(bmDescriptor);
            }
            else if (name == "Closed Road")
            {
                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueOrange);
                markerOpt1.SetIcon(bmDescriptor);
            }
            else
            {
                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen);
                markerOpt1.SetIcon(bmDescriptor);
            }
            
            markerOpt1.Visible(true);
            markerOpt1.Draggable(false);

            map.AddMarker(markerOpt1);
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            LocationManager locationManager = (LocationManager)GetSystemService(Context.LocationService);
            ActivityCompat.RequestPermissions(this, new String[] {
             Manifest.Permission.AccessFineLocation,
             Manifest.Permission.AccessCoarseLocation },
                 34);
            permissionFine = Manifest.Permission.AccessFineLocation;
            permissionCoarse = Manifest.Permission.AccessCoarseLocation;
            System.Threading.Thread.Sleep(3000);


            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 2000, 1, this);

            this.Title = "Disaster Watch";
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.maps_main);

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFrag.GetMapAsync(this);
        }
        public void OnMapReady(GoogleMap map)
        {
            pinproccessor(map);
            map.MyLocationEnabled = true;
            map.MyLocationChange += (latitude, longitude) =>
            {
                var latitude1 = latitude;
                var longitude1 = longitude;
            };

            map.MoveCamera(
            CameraUpdateFactory.NewLatLng(
                new LatLng(39.8283, -98.5795)));
            map.AnimateCamera(
                CameraUpdateFactory.ZoomTo(3));

            Button button = FindViewById<Button>(Resource.Id.button);

            Button button2 = FindViewById<Button>(Resource.Id.button_2);
            button2.Visibility = ViewStates.Invisible;

            Button button3 = FindViewById<Button>(Resource.Id.button_3);
            button3.Visibility = ViewStates.Invisible;

            Button button4 = FindViewById<Button>(Resource.Id.button_4);
            button4.Visibility = ViewStates.Invisible;

            Button button5 = FindViewById<Button>(Resource.Id.button_5);
            button5.Visibility = ViewStates.Invisible;

            Button button6 = FindViewById<Button>(Resource.Id.safehouse);

            EditText supplies = FindViewById<EditText>(Resource.Id.supplies);
            supplies.Visibility = ViewStates.Invisible;

            TextView supplies_text = FindViewById<TextView>(Resource.Id.supplies_text);
            supplies_text.Visibility = ViewStates.Invisible;

            TextView pins = FindViewById<TextView>(Resource.Id.pins);

            TextView water = FindViewById<TextView>(Resource.Id.water);
            water.Visibility = ViewStates.Invisible;

            TextView fire = FindViewById<TextView>(Resource.Id.fire);
            fire.Visibility = ViewStates.Invisible;

            TextView debris = FindViewById<TextView>(Resource.Id.debris);
            debris.Visibility = ViewStates.Invisible;

            TextView road = FindViewById<TextView>(Resource.Id.road);
            road.Visibility = ViewStates.Invisible;


            button.Click += (o, e) =>
            {
                if (pins.Visibility == ViewStates.Invisible)
                {
                    pins.Visibility = ViewStates.Visible;
                }

                else
                {
                    pins.Visibility = ViewStates.Invisible;
                }

                if (button2.Visibility == ViewStates.Invisible)
                {
                    button2.Visibility = ViewStates.Visible;
                }

                else
                {
                    button2.Visibility = ViewStates.Invisible;
                }

                if (water.Visibility == ViewStates.Invisible)
                {
                    water.Visibility = ViewStates.Visible;
                }

                else
                {
                    water.Visibility = ViewStates.Invisible;
                }

                if (fire.Visibility == ViewStates.Invisible)
                {
                    fire.Visibility = ViewStates.Visible;
                }

                else
                {
                    fire.Visibility = ViewStates.Invisible;
                }

                if (button3.Visibility == ViewStates.Invisible)
                {
                    button3.Visibility = ViewStates.Visible;
                }

                else
                {
                    button3.Visibility = ViewStates.Invisible;
                }

                if (debris.Visibility == ViewStates.Invisible)
                {
                    debris.Visibility = ViewStates.Visible;
                }

                else
                {
                    debris.Visibility = ViewStates.Invisible;
                }

                if (button4.Visibility == ViewStates.Invisible)
                {
                    button4.Visibility = ViewStates.Visible;
                }

                else
                {
                    button4.Visibility = ViewStates.Invisible;
                }

                if (road.Visibility == ViewStates.Invisible)
                {
                    road.Visibility = ViewStates.Visible;
                }

                else
                {
                    road.Visibility = ViewStates.Invisible;
                }

                if (button5.Visibility == ViewStates.Invisible)
                {
                    button5.Visibility = ViewStates.Visible;
                }

                else
                {
                    button5.Visibility = ViewStates.Invisible;
                }

            };


            var markerlist = new List<Marker>();


            button2.Click += (o, e) =>
            {

                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(latitude, longitude));
                Title = "Water Hazard";
                markerOpt1.SetTitle(Title);

                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure);
                markerOpt1.SetIcon(bmDescriptor);
                markerOpt1.Visible(true);
                markerOpt1.Draggable(true);
                Marker newe = map.AddMarker(markerOpt1);
                markerlist.Append(newe);

                string email = Intent.GetStringExtra("email") ?? "0";
                string url = "http://35.22.42.160:3000/pins?name=" + Title + "&lat=" + latitude + "&lon=" + longitude + "&type=water&email=" + email;

                FetchDBAsync(url, "GET");

            };

            button3.Click += (o, e) =>
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(latitude, longitude));
                Title = "Fire Hazard";
                markerOpt1.SetTitle(Title);

                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed);
                markerOpt1.SetIcon(bmDescriptor);
                markerOpt1.Visible(true);
                markerOpt1.Draggable(true);
                Marker newe = map.AddMarker(markerOpt1);
                markerlist.Append(newe);

                string email = Intent.GetStringExtra("email") ?? "0";
                string url = "http://35.22.42.160:3000/pins?name=" + Title + "&lat=" + latitude + "&lon=" + longitude + "&type=fire&email=" + email;

                FetchDBAsync(url, "GET");
                Console.Out.Write(markerlist.ToString());
            };

            button4.Click += (o, e) =>
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(latitude, longitude));
                Title = "Debris Hazard";
                markerOpt1.SetTitle(Title);

                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueYellow);
                markerOpt1.SetIcon(bmDescriptor);
                markerOpt1.Visible(true);
                markerOpt1.Draggable(true);
                Marker newe = map.AddMarker(markerOpt1);
                markerlist.Append(newe);

                string email = Intent.GetStringExtra("email") ?? "0";
                string url = "http://35.22.42.160:3000/pins?name=" + Title + "&lat=" + latitude + "&lon=" + longitude + "&type=debris&email=" + email;

                FetchDBAsync(url, "GET");
            };

            button5.Click += (o, e) =>
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(latitude, longitude));
                Title = "Closed Road";
                markerOpt1.SetTitle(Title);

                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueOrange);
                markerOpt1.SetIcon(bmDescriptor);
                markerOpt1.Visible(true);
                markerOpt1.Draggable(true);
                Marker newe = map.AddMarker(markerOpt1);
                markerlist.Append(newe);

                string email = Intent.GetStringExtra("email") ?? "0";
                string url = "http://35.22.42.160:3000/pins?name=" + Title + "&lat=" + latitude + "&lon=" + longitude + "&type=safehouse&email=" + email;

                FetchDBAsync(url, "GET");
            };

            button6.Click += (o, e) =>
            {
                //supplies.Visibility = ViewStates.Visible;
                //supplies_text.Visibility = ViewStates.Visible;
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(latitude, longitude));
                Title = "Safe House";
                markerOpt1.SetTitle(Title);

                var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen);
                markerOpt1.SetIcon(bmDescriptor);
                markerOpt1.Visible(true);
                markerOpt1.Draggable(true);
                Marker newe = map.AddMarker(markerOpt1);
                markerlist.Append(newe);

                string email = Intent.GetStringExtra("email") ?? "0";
                string url = "http://35.22.42.160:3000/pins?name=" + Title + "&lat=" + latitude + "&lon=" + longitude + "&type=safehouse&email=" + email;

                FetchDBAsync(url, "GET");
            };

        }

        public void Update(List<MarkerOptions> markerlist)
        {
            /*
            foreach (MarkerOptions markerOpt1 in markerlist)
            {
                markerOpt1.GetPosition
            }
            */
            
        }

        public void OnLocationChanged(Location location)
        {
            currentLocation = location;

            if (currentLocation == null)
            {
                //Error Message
            }
            else
            {

                latitude = currentLocation.Latitude;
                longitude = currentLocation.Longitude;
            }
        }

        public void OnProviderDisabled(string provider)
        {
            ;
        }

        public void OnProviderEnabled(string provider)
        {
            ;
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            ;
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}