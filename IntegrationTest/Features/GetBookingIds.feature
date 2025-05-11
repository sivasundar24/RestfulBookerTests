@getbookingId @regression
Feature: Get Booking IDs
  This feature verifies the GET /booking endpoint, which returns a list of booking IDs.
  The endpoint supports optional filters: firstname, lastname, checkin, and checkout.

  Scenario: Get all booking IDs without filters
    When I request all booking IDs
    Then the response status code should be 200

  Scenario Outline: Get booking IDs by firstname and lastname
    When I request booking IDs with "<firstname>" and "<lastname>"
    Then the response status code should be <statusCode>
    And the response should <Result>

    Examples:
      | firstname | lastname | Result            | statusCode |
      | Sally     | Brown    | not contain empty | 200        |
      | Jim       | Brown    | not contain empty | 200        |
      | Invalid   | Name     | contain empty     | 200        |
      | 123       |          | contain empty     | 200        |
      | !@#       | $%^      | contain empty     | 200        |
      |           |          | not contain empty | 200        |

  Scenario Outline: Get booking IDs by checkin and checkout
    When I request booking IDs with checkin "<checkin>" and checkout "<checkout>"
    Then the response status code should be <statusCode>
    And the response should <Result>

    Examples:
      | checkin     | checkout    | Result            | statusCode |
      | 2014-03-13  | 2014-05-21  | contain empty     | 200        |
      | 2020-01-01  | 2020-12-31  | contain empty     | 200        |
      | invalid     | 2020-01-01  | contain empty     | 500        |
      | 2020-01-01  | invalid     | contain empty     | 500        |
      | (/&         | $%^         | contain empty     | 500        |
