using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;

namespace SudokuUI.Droid
{
	[Activity (Label = "SudokuUI", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

            string fname = "sudoku_db.sqlite";
            string flocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fpath = Path.Combine(flocation, fname);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new SudokuUI.App (fpath));
		}
	}
}

