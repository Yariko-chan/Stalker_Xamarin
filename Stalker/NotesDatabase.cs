using System;
using System.Text;
using System.Globalization;
using Android.Database.Sqlite;
using Android.Content;

namespace Stalker
{
	public class NotesDatabase: SQLiteOpenHelper
	{
		const string TableName = "notes";
		const string TextColumn = "text";
		const string DateTimeColumn = "date_time";
		const string PhotoURIColumn = "photo_uri";
		const string LatitudeColumn = "latitude";
		const string LongitudeColumn = "longitude";

		public static readonly string MyDatabaseName = "notes.db";
		public static readonly int DatabaseVersion = 1;

		public NotesDatabase(Context context) : base(context, MyDatabaseName, null, DatabaseVersion) { }

		public override void OnCreate(SQLiteDatabase db)
		{
			var query = new StringBuilder ();
			query.AppendFormat ("CREATE TABLE [{0}] ", TableName);
			query.AppendFormat ("([_id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, ");
			query.AppendFormat ("[{0}] TEXT, ", TextColumn);
			query.AppendFormat ("[{0}] TEXT NOT NULL UNIQUE, ", DateTimeColumn);
			query.AppendFormat ("[{0}] TEXT UNIQUE, ", PhotoURIColumn);
			query.AppendFormat ("[{0}] REAL, ", LatitudeColumn);
			query.AppendFormat ("[{0}] REAL)", LongitudeColumn);
			db.ExecSQL(query.ToString());
		}

		public void insertNote (Note note, SQLiteDatabase db)
		{
			var query = new StringBuilder ("INSERT INTO ");
			query.Append (TableName);
			query.AppendFormat(" ({0}, {1}, {2}, {3}, {4}) ", TextColumn, DateTimeColumn, PhotoURIColumn, LatitudeColumn, LongitudeColumn);
			query.AppendFormat(CultureInfo.InvariantCulture, "VALUES ('{0}', '{1}', '{2}', {3}, {4})", note.Text, note.DateTime, note.PhotoURI, note.Latitude, note.Longitude);
			db.ExecSQL(query.ToString());
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{   
			throw new NotImplementedException();
		}
	}
}

