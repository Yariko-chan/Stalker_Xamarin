
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Stalker
{
	public class NotesListAdapter : BaseAdapter<string>
	{
		const int PreviewWidth = 100;
		const int PreviewHeigth = 100;

		private List<Note> Notes;
		Activity Context;

		public NotesListAdapter(Activity context, List<Note> notes) : base() {
			Context = context;
			Notes = notes;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override string this[int position] {  
			get { return Notes[position].Text; }
		}
		public override int Count {
			get { return Notes.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; 
			if (view == null) 
				view = Context.LayoutInflater.Inflate(Resource.Layout.Note, null);
			var note = Notes[position];
			view.FindViewById<TextView>(Resource.Id.note).Text = note.Text;
			view.FindViewById<TextView>(Resource.Id.date).Text = note.DateTime;
			ImageView i = view.FindViewById<ImageView> (Resource.Id.photo);
			i.SetImageBitmap(Utils.GetPreview(note.PhotoURI, PreviewWidth, PreviewHeigth));
			return view;
		}
	}
}


