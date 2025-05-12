using RestSharp;
using System.Text.Json;

namespace IntegrationTest.Helpers
{
    public static class AuthHelper
    {
        public static string GetToken()
        {
            var client = new RestClient(TestConfig.BaseUrl);
            var request = new RestRequest("auth", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { username = "admin", password = "password123" });

            var response = client.Execute(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                throw new Exception("Failed to retrieve token from /auth");

            return JsonDocument.Parse(response.Content!)
                               .RootElement.GetProperty("token")
                               .GetString() ?? throw new Exception("Token not found in response");
        }
    }
}
