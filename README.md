# Integration Test Framework for Restful-Booker APIs

This project implements an automated **integration test framework** for validating the [Restful Booker](https://restful-booker.herokuapp.com/apidoc/index.html#api-Ping-Ping) APIs. The focus is on acceptance testing using Gherkin-style feature files with SpecFlow and .NET.

---

## Purpose

To ensure API stability and reliability by verifying:

- Retrieval of booking IDs
- Retrieval of booking details
- Creation of bookings
- Updating existing bookings

All test results are reported with `ExtentReports` and can be run locally or inside Docker.

---

## Tools & Frameworks Used

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SpecFlow](https://specflow.org/) (Gherkin-style BDD)
- [NUnit](https://nunit.org/)
- [RestSharp](https://restsharp.dev/)
- [ExtentReports](https://github.com/extent-framework/extentreports-dotnet-cli)
- Docker
- Azure DevOps (CI pipeline integration)

---

## Folder Structure

```
RestfulBookerTests/
├── IntegrationTest/
│   ├── Features/                # Gherkin feature files
│   ├── Steps/                   # Step definitions
│   ├── Hooks/                   # Setup/teardown hooks
│   ├── Models/                  # Request/response models
│   ├── Helpers/                 # Utility classes (e.g., ExtentReportManager)
│   ├── Dockerfile               # Dockerfile for test container
│   ├── .dockerignore            # Ignore files during Docker build
│   └── RestfulBookerTests.csproj
├── CaseStudy/                   # Testing Approach for the New Payment Method
├── Results/                     # Generated reports (e.g., ExtentReport.html)
├── azure-pipelines.yml          # CI/CD pipeline definition
├── .gitignore                   # Git ignore rules
└── README.md                    # Project documentation

```

---

## Running the Tests

### Run **all feature files** locally

```bash
dotnet test
```

### Run a **single feature file**

```bash
dotnet test --filter "FullyQualifiedName~CreateBooking"
```

### Run scenarios by **tag**

If your feature files use tags like `@getbooking`, use:

```bash
dotnet test --filter TestCategory=getbooking
```

Make sure your `[Category("smoke")]` attribute is applied in the code or SpecFlow binding.

---

## Running Tests in Docker

### Build the image

```bash
docker build -t restful-booker-tests .
```

### Run the tests in Docker

```bash
docker run -v "${PWD}/Results:/app/Results" restful-booker-tests
```

> **Note**: On Windows CMD, use `%cd%\Results:/app/Results` instead.

---

## Where to Find Results

After execution, the test report will be available in:

```
Results/ExtentReport.html
```

You can open it in a browser to view the test execution summary.

## CI Integration

Azure DevOps pipeline is included in `azure-pipelines.yml`. It:

- Builds the Docker image
- Runs tests inside the container
- Extracts the report from `/app/Results`
- Publishes `ExtentReport.html` as a downloadable artifact

---

## Author

Developed by:Sivasundar Anbumani 
QA Automation Engineer

---

## Contributions

Feel free to fork and extend tests, add negative test scenarios, or integrate with other tools.
