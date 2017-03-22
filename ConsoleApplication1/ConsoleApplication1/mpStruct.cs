using System;

namespace mpStruct
{
	// This is the struct with information about the Media Players.
	public struct MediaPlayer
	{

		public int Id;
		public int Stock;
		public string Make;
		public string Model;
		public float Mb;
		public float Price;

		// Constructor creates the struct when the program starts.
		public MediaPlayer(int p_id, int p_stock, string p_make, string p_model, float p_mb, float p_price)
		{
			this.Id = p_id;
			this.Stock = p_stock;
			this.Make = p_make;
			this.Model = p_model;
			this.Mb = p_mb;
			this.Price = p_price;
		}
	}
}

