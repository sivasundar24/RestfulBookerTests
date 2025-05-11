using IntegrationTest.Helpers;
using RestSharp;
using System.Text.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RestfulBookerTests.IntegrationTest.Steps
{
    [Binding]
    public class UpdateBookingSteps
    {
        private readonly ScenarioContext _context;
        private readonly RestClient _client = new(TestConfig.BaseUrl);
        private int _bookingId;
        private string _token = string.Empty;

        public UpdateBookingSteps(ScenarioContext context) => _context = context;

        [Given(@"I create a booking with valid data")]
        public void GivenICreateABooking()
        {
            // Get token
            var auth = new RestRequest("auth", Method.Post);
            auth.AddHeader("Content-Type", "application/json");
            auth.AddJsonBody(new { username = "admin", password = "password123" });

            var authResp = _client.Execute(auth);
            _token = JsonDocument.Parse(authResp.Content!).RootElement.GetProperty("token").GetString() ?? string.Empty;
            _context["token"] = _token;

            // Create booking
            var create = new RestRequest("booking", Method.Post);
            create.AddHeader("Content-Type", "application/json");
            create.AddHeader("Accept", "application/json");
            create.AddJsonBody(new
            {
                firstname = "Initial",
                lastname = "User",
                totalprice = 100,
                depositpaid = true,
                bookingdates = new { checkin = "2025-01-01", checkout = "2025-01-05" },
                additionalneeds = "None"
            });

            var createResp = _client.Execute(create);
            _bookingId = JsonDocument.Parse(createResp.Content!).RootElement.GetProperty("bookingid").GetInt32();
            _context["bookingid"] = _bookingId;
        }

        [When(@"I update that booking with the following details:")]
        public void WhenIUpdateThatBookingWithTheFollowingDetails(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            var bookingId = _context.TryGetValue("bookingid", out int ctxBookingId) ? ctxBookingId : _bookingId;
            var token = _context.TryGetValue("token", out string? ctxToken) ? ctxToken ?? _token : _token;

            var request = new RestRequest($"booking/{bookingId}", Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Cookie", $"token={token}");

            request.AddJsonBody(new
            {
                firstname = data.firstname,
                lastname = data.lastname,
                totalprice = int.Parse(data.totalprice.ToString()),
                depositpaid = bool.Parse(data.depositpaid.ToString()),
                bookingdates = new { checkin = data.checkin, checkout = data.checkout },
                additionalneeds = data.additionalneeds
            });

            var response = _client.Execute(request);
            _context["response"] = response;
        }

        [Then(@"the response should reflect the updated booking details")]
        public void ThenTheResponseShouldReflectTheUpdatedBookingDetails()
        {
            var response = _context.Get<RestResponse>("response");
            var root = JsonDocument.Parse(response.Content!).RootElement;

            var common = new CommonSteps(_context);
            common.ValidateUpdatedBookingDetails(root);
        }
    }
}
