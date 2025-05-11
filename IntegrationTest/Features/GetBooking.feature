@getbooking @regression
Feature: Get Bookings
  This feature validates the GET /booking/{id} endpoint to ensure it returns correct responses
  for valid, invalid, and malformed booking ID inputs.

  Scenario Outline: Get booking details with valid ID
    When I request booking details with numeric ID <bookingId>
    Then the response status code should be 200
    And the response should contain valid booking fields

    Examples:
      | bookingId |
      | 1         |
      | 2         |

  Scenario Outline: Get booking details with non-existing ID
    When I request booking details with numeric ID <id>
    Then the response status code should be 404

    Examples:
      | id     |
      | 99999  |
      | 123456 |

  Scenario Outline: Get booking details with invalid ID format
    When I request booking details with string ID "<id>"
    Then the response status code should be <statusCode>

    Examples:
      | id   | statusCode |
      | abc  | 404        |
      | !@#  | 404        |
      |      | 200        |
