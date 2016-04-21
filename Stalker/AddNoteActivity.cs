
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Android.Graphics;
using Android.OS.Storage;
using Android.Net;
using Android.Service;

using Java.Lang;
using Java.IO;

using Environment = Android.OS.Environment;
using File = Java.IO.File;
using Exception = Java.Lang.Exception;
using IOException = Java.IO.IOException;
using Uri = Android.Net.Uri;
using Math = Java.Lang.Math;

namespace Stalker
{
	[Activity (Label = "@string/add_note", Theme = "@android:style/Theme.Dialog")]			
	public class AddNoteActivity : Activity
	{
		const string Text = "note_text";
		const string PhotoPath = "photo_path";
		const int RequestImageCapture = 1;
		const int PreviewWidth = 250;
		const int PreviewHeigth = 200;

		private EditText AddText;
		private Button Save;
		private Button Cancel;
		private ImageButton AddPhoto;
		private ImageView AddedPhoto;

		private string CurrentPhotoPath = "";

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			if (savedInstanceState != null) {
				CurrentPhotoPath = savedInstanceState.GetString(PhotoPath);
			}
			SetContentView (Resource.Layout.AddNoteDialog);
			InitializeControls ();
			SetButtonsActions ();
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			outState.PutString (PhotoPath, CurrentPhotoPath);
			base.OnSaveInstanceState (outState);    
		}

		private void SaveNote(object sender, EventArgs eventArgs){
			Intent intent = new Intent (this, typeof(MainActivity));
			intent.PutExtra (Text, AddText.Text.ToString());
			intent.PutExtra (PhotoPath, CurrentPhotoPath);
			SetResult (Result.Ok, intent);
			Finish();
		}

		public override void OnBackPressed()
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			SetResult (Result.Canceled, intent);
			Finish();
		}

		private void MakePhoto(object sender, EventArgs eventArgs){
			TakePhoto ();
		}

		private void TakePhoto()
		{
			Intent TakePhotoIntent = new Intent (MediaStore.ActionImageCapture);
			if (TakePhotoIntent.ResolveActivity(PackageManager) != null)
			{
				File PhotoFile = null;
				try 
				{
					PhotoFile = CreateImageFile();
					Toast.MakeText(this, PhotoFile.AbsolutePath, ToastLength.Long).Show();
				} catch (IOException ex) 
				{
					Toast.MakeText(this, Resource.String.file_error, ToastLength.Short).Show();
					return;
				}
				if (PhotoFile != null) 
				{
					TakePhotoIntent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(PhotoFile));
					StartActivityForResult (TakePhotoIntent, RequestImageCapture);
				}
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (CurrentPhotoPath == "") {
				TakePhoto ();
			} else {
				HideButton ();
				SetPhoto ();
			}
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == RequestImageCapture & resultCode == Result.Ok) {
				GalleryAddPhoto ();
			}
		}

		private void GalleryAddPhoto() {
			Intent MediaScanIntent = new Intent (Intent.ActionMediaScannerScanFile);
			File f = new File(CurrentPhotoPath);
			Uri contentUri = Uri.FromFile(f);
			MediaScanIntent.SetData(contentUri);
			this.SendBroadcast(MediaScanIntent);
		}

		private void HideButton(){
			AddPhoto.Visibility = ViewStates.Gone;
		}

		private void SetPhoto() {
			AddedPhoto.SetImageBitmap(Utils.GetPreview(CurrentPhotoPath, PreviewWidth, PreviewHeigth));
		}

		private void InitializeControls(){
			AddPhoto = (ImageButton)FindViewById (Resource.Id.addPhoto);
			AddedPhoto = (ImageView)FindViewById (Resource.Id.addedPhoto);
			AddText = (EditText)FindViewById (Resource.Id.addNote);
			Save = (Button) FindViewById (Resource.Id.saveNote);
			Cancel = (Button)FindViewById (Resource.Id.cancel);
		}

		private void SetButtonsActions(){
			Save.Click += SaveNote;
			Cancel.Click += delegate {OnBackPressed();};
			AddPhoto.Click += MakePhoto;
		}

		private File CreateImageFile(){
			string ImageFileName = "JPEG_" + DateTime.Now.ToString ();
			File StorageDirectory = Environment.GetExternalStoragePublicDirectory (Environment.DirectoryPictures);
			File Image = File.CreateTempFile(ImageFileName, ".jpg", StorageDirectory);
			CurrentPhotoPath = Image.AbsolutePath;
			return Image;
		}
	}
}

