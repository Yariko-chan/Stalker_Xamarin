using System;
using Android.Widget;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Views;
using Android.Locations;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Stalker
{
	public class NoteslistCursorAdapter : CursorAdapter {
		
		Activity context;
		const int PreviewWidth = 100;
		const int PreviewHeigth = 100;

		public NoteslistCursorAdapter(Activity context, ICursor c)
			: base(context, c)
		{
			this.context = context;
		}
		public override void BindView(View view, Context context, ICursor cursor)
		{
			view.FindViewById<TextView>(Resource.Id.note).Text = cursor.GetString(1);
			view.FindViewById<TextView>(Resource.Id.date).Text = cursor.GetString(2);
			ImageView i = view.FindViewById<ImageView> (Resource.Id.photo);
			i.SetImageBitmap(Utils.GetPreview(cursor.GetString(3), PreviewWidth, PreviewHeigth));
			double n = cursor.GetDouble (4);
			view.FindViewById<TextView> (Resource.Id.location).Text = string.Format("{0}, {1}", cursor.GetDouble (4), cursor.GetDouble(5));
		}
		public override View NewView(Context context, ICursor cursor, ViewGroup parent)
		{
			return this.context.LayoutInflater.Inflate(Resource.Layout.Note, parent, false);
		}

//		private string getAddress(double lat, double lon)
//		{
//			Address address = ReverseGeocodeCurrentLocation(lat, lon);
//			if (address != null)
//			{
//				StringBuilder deviceAddress = new StringBuilder();
//				for (int i = 0; i < address.MaxAddressLineIndex; i++)
//				{
//					deviceAddress.AppendLine(address.GetAddressLine(i));
//				}
//				return deviceAddress.ToString();
//			}
//			else
//			{
//				return context.GetString(Resource.String.location_undefined);
//			}
//		}
//
//		private async Task<Address> ReverseGeocodeCurrentLocation(double lat, double lon)
//		{
//			Geocoder geocoder = new Geocoder(this);
//			IList<Address> addressList = await geocoder.GetFromLocationAsync(lat, lon, 10);
//
//			Address address = addressList.FirstOrDefault();
//			return address;
//		}
	}
}

