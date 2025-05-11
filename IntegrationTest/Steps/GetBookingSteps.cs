using System.Text.Json;
using RestSharp;
using TechTalk.SpecFlow;
using IntegrationTest.Helpers;

namespace RestfulBookerTests.IntegrationTest.Steps
{
    [Binding]
    [Category("getbooking")]
    public class GetBookingSteps
    {
        private readonly RestClient _client = new(TestConfig.BaseUrl);
        private readonly ScenarioContext _context;

        public GetBookingSteps(ScenarioContext context)
        {
            _context = context;
        }

        [When(@"I request booking details with numeric ID (.*)")]
        public void WhenIRequestBookingDetailsWithNumericID(int bookingId)
        {
            var request = new RestRequest($"booking/{bookingId}", Method.Get);
            request.AddHeader("Accept", "application/json");

            var response = _client.Execute(request);
            _context["response"] = response;
        }

        [When(@"I request booking details with string ID ""(.*)""")]
        public void WhenIRequestBookingDetailsWithStringID(string id)
        {
            var request = new RestRequest($"booking/{id}", Method.Get);
            request.AddHeader("Accept", "application/json");

            var response = _client.Execute(request);
            _context["response"] = response;
        }

        [Then(@"the response should contain valid booking fields")]
        public void ThenTheResponseShouldContainValidBookingFields()
        {
            var response = _context.Get<RestResponse>("response");
            var root = JsonDocument.Parse(response.Content!).RootElement;

            var common = new CommonSteps(_context);
            common.ValidateBookingFields(root);
        }
    }
}
