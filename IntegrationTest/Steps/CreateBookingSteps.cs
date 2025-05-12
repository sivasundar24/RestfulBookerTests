using IntegrationTest.Helpers;
using RestSharp;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace RestfulBookerTests.IntegrationTest.Steps;

[Binding]
public class CreateBookingSteps(ScenarioContext context)
{
    private readonly ScenarioContext _context = context;
    private readonly RestClient _client = new(TestConfig.BaseUrl);
    private readonly BookingHelper _bookingHelper = new(context);

    [When(@"I create a booking with the following details:")]
    public void WhenICreateABookingWithTheFollowingDetails(Table table)
    {
       
        var row = table.Rows[0];

        var body = new
        {
            firstname = row["firstname"],
            lastname = row["lastname"],
            totalprice = int.TryParse(row["totalprice"], out int price) ? price : 0,
            depositpaid = bool.TryParse(row["depositpaid"], out bool paid) && paid,
            bookingdates = new
            {
                checkin = row["checkin"],
                checkout = row["checkout"]
            },
            additionalneeds = row["additionalneeds"]
        };

        var response = _bookingHelper.CreateBookingWithBody(_client, body);
        _context["response"] = response;
    }

    [When(@"I send an empty body to create a booking")]
    public void WhenISendAnEmptyBodyToCreateABooking()
    {
        var request = new RestRequest("booking", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddStringBody("{}", DataFormat.Json);

        var response = _client.Execute(request);
        _context["response"] = response;
    }

    [Then(@"the response should contain booking confirmation")]
    public void ThenTheResponseShouldContainBookingConfirmation()
    {
        var response = _context.Get<RestResponse>("response");

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Content, Is.Not.Null);

        var json = JsonDocument.Parse(response.Content!);
        var root = json.RootElement;

        Assert.That(root.TryGetProperty("bookingid", out var bookingIdElement), Is.True);
        Assert.That(root.TryGetProperty("booking", out _), Is.True);

        int bookingId = bookingIdElement.GetInt32();
        _context["bookingid"] = bookingId;
    }
}
