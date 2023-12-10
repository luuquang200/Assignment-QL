using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeProject
{
	public class Merchant
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("distance")]
		public double Distance { get; set; }
	}
}
