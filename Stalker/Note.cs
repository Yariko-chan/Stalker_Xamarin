using System;
using SQLite;

namespace Stalker
{
	public class Note
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set;}

		public string Text { get; set;}
		public string PhotoURI { get; set;}
		public string DateTime { get; set;}
		public double Latitude { get; set;}
		public double Longitude { get; set;}
	}
}

