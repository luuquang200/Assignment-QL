using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HomeProject;
using Newtonsoft.Json;

public class Program
{
	public static void Main(string[] args)
	{
		string checkInDate = "";
		DateTime checkInDateTime;

		string inputFile = "./input.json";
		string outputFile = "./output.json";

		while (true)
		{
			Console.WriteLine("Please enter a date in the format YYYY-MM-DD or press 'q' to quit:");
			Console.Write("-> ");
			checkInDate = Console.ReadLine() ?? string.Empty;

			if (checkInDate.ToLower() == "q")
			{
				Console.WriteLine("Exiting...");
				return;
			}

			if (CheckDateFormat(checkInDate) && DateTime.TryParse(checkInDate, out checkInDateTime))
			{
				if (!File.Exists(inputFile))
				{
					Console.WriteLine("File not found. Please place the input file in the same directory as the executable file.");
					continue;
				}

				var offers = LoadOffersFromJson(inputFile);
				var filteredOffers = FilterOffers(offers, checkInDateTime);

				Console.WriteLine("Offers filtered: " + filteredOffers.Count);
				foreach (var offer in filteredOffers)
				{
					Console.WriteLine("+ " + offer.Title);
				}

				SaveOffersToJson(outputFile, filteredOffers);

				Console.WriteLine("Done!");
			}
			else
			{
				Console.WriteLine("Invalid date format. Please try again with the format YYYY-MM-DD.");
			}
		}
	}

	public static bool CheckDateFormat(string date)
	{
		Regex r = new Regex(@"^\d{4}-\d{2}-\d{2}$");
		if (r.IsMatch(date))
		{
			return true;
		}
		return false;
	}

	private static List<Offer> FilterOffers(Offers? offers, DateTime checkInDate)
	{
		if (offers == null || offers.OfferList == null)
		{
			Console.WriteLine("offers is null or the list of offers is empty");
			return new List<Offer>();
		}

		// Filter offers by category and valid date and select nearest merchant
		List<Offer> validOffers = new List<Offer>();
		foreach (var offer in offers.OfferList)
		{
			if (CategoryHandler.IsValidCategory(offer.Category) && offer.ValidTo != null && IsValidDate(offer.ValidTo, checkInDate))
			{
				var nearestMerchantOffer = SelectNearestMerchant(offer);
				if (nearestMerchantOffer != null)
				{
					validOffers.Add(nearestMerchantOffer);
				}
			}
		}

		// Group offers by category and select nearest offer
		Dictionary<int, List<Offer>> offersGroupedByCategory = new Dictionary<int, List<Offer>>();
		foreach (var offer in validOffers)
		{
			if (!offersGroupedByCategory.ContainsKey(offer.Category))
			{
				offersGroupedByCategory[offer.Category] = new List<Offer>();
			}
			offersGroupedByCategory[offer.Category].Add(offer);
		}

		List<Offer> nearestOffersInEachCategory = new List<Offer>();
		foreach (var group in offersGroupedByCategory)
		{
			var nearestOffer = SelectNearestOffer(group.Value);
			if (nearestOffer != null)
			{
				nearestOffersInEachCategory.Add(nearestOffer);
			}
		}

		// Sort offers by distance 
		nearestOffersInEachCategory.Sort((o1, o2) =>
		{
			if (o1.Merchants != null && o1.Merchants.Count > 0 && o2.Merchants != null && o2.Merchants.Count > 0)
			{
				return o1.Merchants[0].Distance.CompareTo(o2.Merchants[0].Distance);
			}
			else
			{
				return 0;
			}
		});

		// Return top 2 nearest offers
		if (nearestOffersInEachCategory.Count > 2)
		{
			nearestOffersInEachCategory = nearestOffersInEachCategory.GetRange(0, 2);
		}

		return nearestOffersInEachCategory;
	}

	// Check valid date of offer to checkin date + 5 days
	private static bool IsValidDate(string validToDate, DateTime checkInDate)
	{
		if (string.IsNullOrEmpty(validToDate))
		{
			return false;
		}

		DateTime validToDateTime = DateTime.Parse(validToDate);
		return validToDateTime >= checkInDate.AddDays(5);
	}

	private static Offer? SelectNearestMerchant(Offer offer)
	{
		if (offer.Merchants == null || offer.Merchants.Count == 0)
		{
			return null;
		}

		Merchant nearestMerchant = offer.Merchants[0];
		double minDistance = nearestMerchant.Distance;

		foreach (var merchant in offer.Merchants)
		{
			double currentDistance = merchant.Distance;
			if (currentDistance < minDistance)
			{
				minDistance = currentDistance;
				nearestMerchant = merchant;
			}
		}

		return new Offer
		{
			Id = offer.Id,
			Title = offer.Title,
			Description = offer.Description,
			Category = offer.Category,
			Merchants = new List<Merchant> { nearestMerchant },
			ValidTo = offer.ValidTo
		};
	}


	private static Offer? SelectNearestOffer(List<Offer> offerGroup)
	{
		if (offerGroup == null || offerGroup.Count == 0)
		{
			return null;
		}

		Offer nearestOffer = offerGroup[0];
		if (nearestOffer.Merchants == null || nearestOffer.Merchants.Count == 0)
		{
			return null;
		}

		double minDistance = nearestOffer.Merchants[0].Distance;

		foreach (var offer in offerGroup)
		{
			if (offer.Merchants != null && offer.Merchants.Count > 0)
			{
				double currentDistance = offer.Merchants[0].Distance;
				if (currentDistance < minDistance)
				{
					minDistance = currentDistance;
					nearestOffer = offer;
				}
			}
		}

		return nearestOffer;
	}

	// Load and save offers 
	private static Offers? LoadOffersFromJson(string filePath)
	{
		string inputJson = File.ReadAllText(filePath);
		return JsonConvert.DeserializeObject<Offers?>(inputJson);
	}

	private static void SaveOffersToJson(string filePath, List<Offer> offers)
	{
		var outputOffers = new Offers { OfferList = offers };
		string outputJson = JsonConvert.SerializeObject(outputOffers, Newtonsoft.Json.Formatting.Indented);
		File.WriteAllText(filePath, outputJson);
	}
}


