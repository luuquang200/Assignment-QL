using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeProject
{
	public class Offer
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("title")]
		public string? Title { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("category")]
		public int Category { get; set; }

		[JsonProperty("merchants")]
		public List<Merchant>? Merchants { get; set; }

		[JsonProperty("valid_to")]
		public string? ValidTo { get; set; }
	}
}
