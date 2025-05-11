using IntegrationTest.Helpers;
using RestSharp;
using TechTalk.SpecFlow;

namespace IntegrationTest.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun() =>
            ExtentReportManager.GetExtent();

        [AfterTestRun]
        public static void AfterTestRun() =>
            ExtentReportManager.Flush();

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext) =>
            ExtentReportManager.CreateFeature(featureContext.FeatureInfo.Title);

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext) =>
            ExtentReportManager.CreateScenario(scenarioContext.ScenarioInfo.Title);

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                var stepText = scenarioContext.StepContext.StepInfo.Text;

                string details = string.Empty;

                // Try to get the API response (if captured in step definition)
                if (scenarioContext.TryGetValue("response", out RestResponse response))
                {
                    details = $"Status Code: {(int)response.StatusCode}\n\n" +
                              $"Response:\n{response.Content}";
                }

                ExtentReportManager.LogStep(stepType, stepText, "Fail", scenarioContext.TestError.Message, details);
            }
            else
            {
              var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
              var stepText = scenarioContext.StepContext.StepInfo.Text;
              ExtentReportManager.LogStep(stepType, stepText, "Pass");
            }
        }
    }
}
