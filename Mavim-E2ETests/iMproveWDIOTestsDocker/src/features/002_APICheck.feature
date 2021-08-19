  Feature: 002_API get request check
  Scenario Outline: As a user, I can check api calls when I expand branch

  Scenario: TC003_Login iMprove and verify API value for expanding a branch
      Given I Login to Improve application
      Given I wait for Home Button
      And I click on the Home Icon
      Then I should be able to expand branch "Cars & Co"
      Then I should verify Intercept service for the branch "Running the business"
      Then I should be able to logout of Improve