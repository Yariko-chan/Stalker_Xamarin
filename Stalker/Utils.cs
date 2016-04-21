using System;
using Android.Graphics;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Stalker
{
	public class Utils
	{
		public Utils ()
		{
		}

		public static Bitmap GetPreview(String photoPath, int targetW,  int targetH){
			BitmapFactory.Options BMOptions = new BitmapFactory.Options();
			var bmp = BitmapFactory.DecodeFile(photoPath, BMOptions);
			int photoW = BMOptions.OutWidth;
			int photoH = BMOptions.OutHeight;
			bmp.Dispose ();

			int scaleFactor = Math.Min (photoW/targetW, photoH/targetH);

			BMOptions.InJustDecodeBounds = false;
			BMOptions.InSampleSize = scaleFactor;
			BMOptions.InPurgeable = true;

			return BitmapFactory.DecodeFile(photoPath, BMOptions);
		}

		public static Bitmap GetPreview(String photoPath, ImageView iv){
			int targetW = 250;
			int targetH = 200;
			BitmapFactory.Options BMOptions = new BitmapFactory.Options();
			var bmp = BitmapFactory.DecodeFile(photoPath, BMOptions);
			int photoW = BMOptions.OutWidth;
			int photoH = BMOptions.OutHeight;
			bmp.Dispose ();

			int scaleFactor = Math.Min (photoW/targetW, photoH/targetH);

			BMOptions.InJustDecodeBounds = false;
			BMOptions.InSampleSize = scaleFactor;
			BMOptions.InPurgeable = true;

			return BitmapFactory.DecodeFile(photoPath, BMOptions);
		}
	}
}

