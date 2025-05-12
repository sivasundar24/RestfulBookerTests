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
        private readonly BookingHelper _bookingHelper;
        private int _bookingId;
        private string _token = string.Empty;

        public UpdateBookingSteps(ScenarioContext context)
        {
            _context = context;
            _bookingHelper = new BookingHelper(_context);
        }

        [Given(@"I create a booking with valid data")]
        public void GivenICreateABooking()
        {
            _bookingId = _bookingHelper.CreateBooking(_client);
            _context["bookingid"] = _bookingId;
        }

        [When(@"I update that booking with the following details:")]
        public void WhenIUpdateThatBookingWithTheFollowingDetails(Table table)
        {
            dynamic data = table.CreateDynamicInstance();

            _token = AuthHelper.GetToken();
            _context["token"] = _token;

            int bookingId = _context.TryGetValue("bookingid", out int ctxBookingId) ? ctxBookingId : _bookingId;

            var request = new RestRequest($"booking/{bookingId}", Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Cookie", $"token={_token}");

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
