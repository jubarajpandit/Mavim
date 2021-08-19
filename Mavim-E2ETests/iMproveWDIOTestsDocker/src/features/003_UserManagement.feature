Feature: 003_User Management
  Scenario Outline: As a user, I test user management feature

  Scenario: TC004_Navigate to User management page and check user
      Given I Login to Improve application
      And I wait for Home Button
      When I navigate to User Management Page
      Then I should verify user "userName" exists      

 Scenario: TC005_Change the language to english
      Given I Login to Improve application
      And I wait for Home Button
      Then I should be able to change the language to "English"

  Scenario: TC006_Add New User
      Given I Login to Improve application
      And I wait for Home Button
      When I navigate to User Management Page
      Then I should be able to add user "userName2" with role as "Subscriber"
      Then I should verify user "userName2" exists

    Scenario: TC007_Remove user
      Given I Login to Improve application
      And I wait for Home Button
      When I navigate to User Management Page
      Then I should be able to remove user "userName2"
      Then I should verify user "userName2" has been removed
      Then I should be able to logout of Improve