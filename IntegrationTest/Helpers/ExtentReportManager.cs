using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;

namespace IntegrationTest.Helpers
{
    public static class ExtentReportManager
    {
        private static ExtentReports? _extent;
        private static ExtentTest? _feature;
        private static ExtentTest? _scenario;

        public static ExtentReports GetExtent()
        {
            if (_extent == null)
            {
                var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;

                if (projectRoot == null)
                    throw new InvalidOperationException("Unable to locate project root directory.");

                var resultsDir = Path.Combine(projectRoot, "Results");
                Directory.CreateDirectory(resultsDir);

                var reportPath = Path.Combine(resultsDir, "ExtentReport.html");

                var reporter = new ExtentSparkReporter(reportPath);
                _extent = new ExtentReports();
                _extent.AttachReporter(reporter);
                _extent.AddSystemInfo("Environment", "QA");
                _extent.AddSystemInfo("User", Environment.UserName);
            }

            return _extent;
        }

        public static void CreateFeature(string title)
        {
            _feature = GetExtent().CreateTest<Feature>(title);
        }

        public static void CreateScenario(string title)
        {
            if (_feature == null)
                throw new InvalidOperationException("Feature node is not initialized. Call CreateFeature() before CreateScenario().");

            _scenario = _feature.CreateNode<Scenario>(title);
        }

        public static void LogStep(string stepType, string stepText, string status, string? error = null, string? details = null)
        {
            if (_scenario == null)
                throw new InvalidOperationException("Scenario node is not initialized. Call CreateScenario() before logging steps.");

            ExtentTest node = stepType switch
            {
                "Given" => _scenario.CreateNode<Given>(stepText),
                "When" => _scenario.CreateNode<When>(stepText),
                "Then" => _scenario.CreateNode<Then>(stepText),
                "And" => _scenario.CreateNode<And>(stepText),
                _ => _scenario.CreateNode(stepText),
            };

            if (status == "Fail")
            {
                if (!string.IsNullOrEmpty(error))
                    node.Fail(error);

                if (!string.IsNullOrEmpty(details))
                    node.Info($"<pre>{details}</pre>");
            }
            else
            {
                node.Pass("Step passed");
            }
        }

        public static void Flush()
        {
            Console.WriteLine("📄 Flushing Extent Report to disk...");
            _extent?.Flush();
        }
    }
}
