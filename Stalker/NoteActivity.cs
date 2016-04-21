
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Stalker
{
	[Activity (Label = "NoteActivity")]			
	public class NoteActivity : Activity
	{
		const string Note = "note_text";
		const string Date = "timestamp"; 
		const string PhotoPath = "photo_path";
		const string DatabaseName = "notes_database";
		const string ID = "id";
		const int RequestAddNewNote = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.FullNote);
		}
	}
}

