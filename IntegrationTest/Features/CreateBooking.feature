@createbooking @regression
Feature: Create Booking
  This feature validates the CreateBooking API with various input scenarios.
  It ensures the endpoint works correctly with valid data and handles invalid or edge cases gracefully.

  Scenario: Create booking with valid details
    When I create a booking with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      | Ole       | Stian    | 111        | true        | 2025-04-12  | 2025-04-15  | Breakfast       |
    Then the response status code should be 200
    And the response should contain booking confirmation

  Scenario: Create booking with missing firstname
    When I create a booking with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      |           | Brown    | 100        | true        | 2024-01-01  | 2025-01-05  | Breakfast       |
    Then the response status code should be 200

  Scenario: Create booking with invalid date
    When I create a booking with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      | Jane      | Doe      | 150        | false       | invalidDate | 2025-01-01  | Lunch           |
    Then the response status code should be 200

  Scenario: Create booking with empty body
    When I send an empty body to create a booking
    Then  the response status code should be 500
