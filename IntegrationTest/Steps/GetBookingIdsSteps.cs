using IntegrationTest.Helpers;
using RestSharp;
using TechTalk.SpecFlow;

namespace RestfulBookerTests.IntegrationTest.Steps
{
    [Binding]
    public class GetBookingIdsSteps
    {
        private readonly RestClient _client = new(TestConfig.BaseUrl);
        private readonly ScenarioContext _context;

        public GetBookingIdsSteps(ScenarioContext context)
        {
            _context = context;
        }

        [When(@"I request all booking IDs")]
        public void WhenIRequestAllBookingIDs()
        {
            var request = new RestRequest("booking", Method.Get);
            request.AddHeader("Accept", "application/json");

            var response = _client.Execute(request);
            _context["response"] = response;
        }

        [When(@"I request booking IDs with ""(.*)"" and ""(.*)""")]
        public void WhenIRequestBookingIDsWithName(string firstname, string lastname)
        {
            var request = new RestRequest("booking", Method.Get);
            request.AddHeader("Accept", "application/json");
            if (!string.IsNullOrEmpty(firstname)) request.AddQueryParameter("firstname", firstname);
            if (!string.IsNullOrEmpty(lastname)) request.AddQueryParameter("lastname", lastname);

            var response = _client.Execute(request);
            _context["response"] = response;
        }

        [When(@"I request booking IDs with checkin ""(.*)"" and checkout ""(.*)""")]
        public void WhenIRequestBookingIDsWithCheckinAndCheckout(string checkin, string checkout)
        {
            var request = new RestRequest("booking", Method.Get);
            request.AddHeader("Accept", "application/json");
            if (!string.IsNullOrEmpty(checkin)) request.AddQueryParameter("checkin", checkin);
            if (!string.IsNullOrEmpty(checkout)) request.AddQueryParameter("checkout", checkout);

            var response = _client.Execute(request);
            _context["response"] = response;
        }
    }
}
