using RestSharp;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace IntegrationTest.Helpers
{
	public class BookingHelper
	{
		private readonly ScenarioContext _context;

		public BookingHelper(ScenarioContext context)
		{
			_context = context;
		}

		public RestResponse CreateBookingWithBody(RestClient client, object body)
		{
			var request = new RestRequest("booking", Method.Post);
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "application/json");
			request.AddJsonBody(body);

			return client.Execute(request);
		}

		public int CreateBooking(RestClient client)
		{
			var defaultBody = new
			{
				firstname = "Initial",
				lastname = "User",
				totalprice = 100,
				depositpaid = true,
				bookingdates = new { checkin = "2025-01-01", checkout = "2025-01-05" },
				additionalneeds = "None"
			};

			var response = CreateBookingWithBody(client, defaultBody);
			var root = JsonDocument.Parse(response.Content!).RootElement;
			return root.GetProperty("bookingid").GetInt32();
		}
	}
}

