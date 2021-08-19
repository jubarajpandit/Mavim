Feature: 001_Login, Navigate and Edit Description
  Scenario Outline: As a user, I can log into the secure and navigate to different tree Items

    Scenario: TC001_Login iMprove and Edit description
      Given I Login to Improve application
      Given I wait for Home Button
      And I click on the Home Icon
      Then I should be able to click on tree branch "Cars & Co"
      When I click Edit Description in word and login
      Then I should be able to add description text "Automation Description"
      
    Scenario: TC002_Verify Edit Description
      Given I Login to Improve application
      Given I wait for Home Button
      And I click on the Home Icon
      Then I should be able to click on tree branch "Cars & Co"
      Then I click Edit Description in word
      Then I should verify that description text is available in the description pane
      Then I should be able to logout of Improve