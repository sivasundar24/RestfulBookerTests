@updatebooking @regression
Feature: Update Booking
  This feature verifies the PUT /booking/{id} endpoint by updating an existing booking with both valid and invalid data.

  Scenario: Create and update a booking with valid data
    Given I create a booking with valid data
    When I update that booking with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      | James     | Brown    | 111        | true        | 2025-01-01  | 2025-01-04  | Breakfast       |
    Then the response status code should be 200
    And the response should reflect the updated booking details

  Scenario: Try to update a booking with invalid data (missing firstname)
    Given I create a booking with valid data
    When I update that booking with the following details:
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      |           | Brown    | 111        | true        | 2025-01-01  | 2025-01-01  | Breakfast       |
    Then the response status code should be 200
