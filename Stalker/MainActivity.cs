using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Database;
using Android.Locations;
using Android.Views;

using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using System;

namespace Stalker
{
	[Activity (Label = "Stalker", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		const string Note = "note_text";
		const string Date = "timestamp"; 
		const string PhotoPath = "photo_path";
		const string DatabaseName = "notes_database";
		const string ID = "id";
		const int RequestAddNewNote = 1;

		private NoteslistCursorAdapter NoteslistAdapter;
		private NotesDatabase ndb;
		private ICursor cursor;

		private Button AddNote;
		private ListView NotesList;

		Location _currentLocation;
		LocationManager _locationManager;
		string _locationProvider;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);
			InitializeLocationManager ();
			InitialiseDatabase ();
			InitializeControls();
		}

		private void InitialiseDatabase()
		{
			ndb = new NotesDatabase(this);
			cursor = UpdateCursor ();
			StartManagingCursor(cursor);
		}

		private void SetActions()
		{
			AddNote.Click += delegate {
				var intent = new Intent (this, typeof(AddNoteActivity));
				StartActivityForResult(intent, RequestAddNewNote);
			};
			NoteslistAdapter = new NoteslistCursorAdapter(this, cursor);
			NotesList.Adapter = (IListAdapter)NoteslistAdapter;
			NotesList.ItemLongClick += SetMultiselect;
		}

		private void OpenFullNote(object sender, AdapterView.ItemClickEventArgs e)
		{
			var intent = new Intent (this, typeof(NoteActivity));
			View note = (View) NoteslistAdapter.GetItem (e.Position);
			TextView t = (TextView)note.FindViewById (Resource.Id.note);
			intent.PutExtra (Note, t.Text);
			StartActivity (intent);
		}

		private void SetMultiselect(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			NotesList.ChoiceMode = Android.Widget.ChoiceMode.Multiple;
			NotesList.SetItemChecked (e.Position, true);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == RequestAddNewNote && resultCode == Result.Ok) {
				double lat = 0;
				double lon = 0;
				_currentLocation = _locationManager.GetLastKnownLocation (_locationProvider);
				if (_currentLocation != null) {
					lat = _currentLocation.Latitude;
					lon = _currentLocation.Longitude;
				}
				ndb.insertNote(new Note() 
					{
						Text = data.GetStringExtra (Note), 
						DateTime =  DateTime.Now.ToString (), 
						PhotoURI = data.GetStringExtra (PhotoPath),
						Latitude = lat,
						Longitude = lon
					}, ndb.WritableDatabase);
				NoteslistAdapter.SwapCursor(UpdateCursor());
				NoteslistAdapter.NotifyDataSetChanged ();
			}

		}

		private void InitializeControls()
		{
			AddNote = FindViewById<Button> (Resource.Id.add);
			NotesList = FindViewById<ListView> (Resource.Id.notesList);
			SetActions ();
		}

		private ICursor UpdateCursor()
		{
			return ndb.ReadableDatabase.RawQuery("SELECT * FROM notes", null);
		}

		void InitializeLocationManager()
		{
			_locationManager = (LocationManager) GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = string.Empty;
			}
		}


	}
}


