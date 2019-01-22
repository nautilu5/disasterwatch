using System;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace App2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class Weather : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.weather_main);

            EditText latitude = FindViewById<EditText>(Resource.Id.latText);
            EditText longitude = FindViewById<EditText>(Resource.Id.longText);
            Button button = FindViewById<Button>(Resource.Id.getWeatherButton);

            // When the user clicks the button, send the REST request to geonames.org,
            button.Click += async (sender, e) => {

                // Get the latitude and longitude entered by the user and create a query. 
                // Note that input error checking is ignored here.

                string url = "http://35.22.42.160:3000/tasks";

                // Fetch the weather information asynchronously, parse the results,
                // then update the screen:
                JsonValue json = await FetchWeatherAsync(url);
                ParseAndDisplay (json);
            };

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
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

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        // Gets weather data from the passed URL.  
        private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }

        // Parses the weather data, then writes temperature, humidity, conditions, and 
        // location to the screen.
        private void ParseAndDisplay(JsonValue json)
        {
            // Get the weather reporting fields from the layout resource: 
            TextView location = FindViewById<TextView>(Resource.Id.locationText);

            // Extract the array of name/value results for the field name "weatherObservation":
            // Note that there is no exception handling for when this field is not found.
            JsonArray weatherResults = json as JsonArray;
            JsonValue entry0 = weatherResults[0];
            String created = entry0["created_at"];
            location.Text = created;
        }
    }
}

