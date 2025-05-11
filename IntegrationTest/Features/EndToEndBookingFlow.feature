@endtoend @regression
Feature: End-to-End Booking Flow
  This scenario simulates a real user journey: creating a booking, verifying it, updating it, and searching via filters.

  Scenario: Complete booking flow with create, get, update, and filter
    When I create a booking with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      | Jim       | Brown    | 111        | true        | 2025-01-01  | 2025-01-04  | Breakfast       |
    Then the response status code should be 200
    And the response should contain booking confirmation

    When I request booking details for the created booking
    Then the response status code should be 200
    And the response should contain valid booking fields

    When I update that booking with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      | James     | Brown    | 111        | true        | 2025-01-01  | 2025-01-04  | Breakfast       |
    Then the response status code should be 200
    And the response should reflect the updated booking details

    When I request booking IDs with "James" and "Brown"
    Then the response status code should be 200
    And the response should not contain empty
