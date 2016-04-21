using System;
using SQLite;
using System.Linq;
using System.Collections.Generic;


namespace Stalker
{
	public class DatabaseHelper
	{
		public static string CreateDatabase(String name)
		{
			var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
			string path = System.IO.Path.Combine(docsFolder, name + ".db");
			try
			{
				using (var connection = new SQLiteConnection(path)) 
				{
					connection.CreateTable<Note>();
					return path;
				}
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		public static string InsertUpdateData(Note data, string path)
		{
			try
			{
				using (var db = new SQLiteConnection(path))
				{
					if (db.Insert(data) != 0)
						db.Update(data);

					var a = db.Table<Note>().ToList();


					return "Single data file inserted or updated";
				}
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		public static string DeleteData(int primaryKey, string path)
		{
			try
			{
				using (var db = new SQLiteConnection(path))
				{
					db.Delete(primaryKey);
					return "Single data file deleted";
				}
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		public static Note GetData(int primaryKey, string path)
		{
			using(var db = new SQLiteConnection(path))
			{
				Note n = db.Get<Note>(primaryKey);
				return n;
			}
		}

		public static List<Note> GetData(string path)
		{
			try
			{
				using (var db = new SQLiteConnection(path))
				{
					return db.Table<Note>().ToList();
				}
			}
			catch (SQLiteException ex)
			{
				return null;
			}
		}

		public static int GetCount(string path)
		{
			using (var db = new SQLiteConnection(path))
			{
				return db.Table<Note> ().Count ();

			}

		}
	}
}

