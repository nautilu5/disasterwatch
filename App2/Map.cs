using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;


namespace App2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class Map : AppCompatActivity, IOnMapReadyCallback, ILocationListener
    {


    }
}

