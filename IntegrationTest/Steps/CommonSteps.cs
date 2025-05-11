using System.Text.Json;
using IntegrationTest.Helpers;

//using NUnit.Framework;
using RestSharp;
using TechTalk.SpecFlow;

namespace RestfulBookerTests.IntegrationTest.Steps
{
    [Binding]
    public class CommonSteps
    {
        private readonly ScenarioContext _context;

        public CommonSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            var response = _context.Get<RestResponse>("response");
            Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode),
                $"Expected status code {expectedStatusCode}, but got {(int)response.StatusCode} - {response.StatusDescription}");
        }

        [Then(@"the response should (contain empty|not contain empty)")]
        public void ThenTheResponseShouldBeEmptyOrNot(string expectation)
        {
            var response = _context.Get<RestResponse>("response");

            if ((int)response.StatusCode != 200)
            {
                TestContext.WriteLine("⚠ Skipping response body validation due to non-200 status.");
                return;
            }

            try
            {
                var json = JsonDocument.Parse(response.Content!);
                var root = json.RootElement;

                if (root.ValueKind != JsonValueKind.Array)
                {
                    Assert.Fail("Expected JSON array but got a different structure.");
                }

                int count = root.GetArrayLength();

                if (expectation == "contain empty")
                    Assert.That(count, Is.EqualTo(0), "Expected empty result but found booking IDs.");
                else if (expectation == "not contain empty")
                    Assert.That(count, Is.GreaterThan(0), "Expected results but found none.");
                else
                    Assert.Fail($"Invalid expectation provided: '{expectation}'");
            }
            catch (JsonException ex)
            {
                Assert.Fail("Invalid JSON in response: " + ex.Message);
            }
        }

        [When(@"I request booking details for the created booking")]
        public void WhenIRequestBookingDetailsForTheCreatedBooking()
        {
            if (!_context.TryGetValue("bookingid", out int bookingId))
                Assert.Fail("bookingid not found in ScenarioContext");

            var request = new RestRequest($"booking/{bookingId}", Method.Get);
            request.AddHeader("Accept", "application/json");

            var client = new RestClient(TestConfig.BaseUrl);
            var response = client.Execute(request);
            _context["response"] = response;
        }

        public void ValidateBookingFields(JsonElement root)
        {
            Assert.Multiple(() =>
            {
                Assert.That(root.TryGetProperty("firstname", out _), Is.True);
                Assert.That(root.TryGetProperty("lastname", out _), Is.True);
                Assert.That(root.TryGetProperty("totalprice", out _), Is.True);
                Assert.That(root.TryGetProperty("depositpaid", out _), Is.True);
                Assert.That(root.TryGetProperty("bookingdates", out var dates), Is.True);
                Assert.That(dates.TryGetProperty("checkin", out _), Is.True);
                Assert.That(dates.TryGetProperty("checkout", out _), Is.True);
            });
        }

        public void ValidateUpdatedBookingDetails(JsonElement root)
        {
            Assert.Multiple(() =>
            {
                Assert.That(root.GetProperty("firstname").GetString(), Is.EqualTo("James"));
                Assert.That(root.GetProperty("lastname").GetString(), Is.EqualTo("Brown"));
                Assert.That(root.GetProperty("totalprice").GetInt32(), Is.EqualTo(111));
                Assert.That(root.GetProperty("depositpaid").GetBoolean(), Is.True);
                Assert.That(root.GetProperty("bookingdates").GetProperty("checkin").GetString(), Is.EqualTo("2025-01-01"));
                Assert.That(root.GetProperty("bookingdates").GetProperty("checkout").GetString(), Is.EqualTo("2025-01-04"));
                Assert.That(root.GetProperty("additionalneeds").GetString(), Is.EqualTo("Breakfast"));
            });
        }
    }
}
