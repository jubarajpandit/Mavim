Feature: 009_Tree Topics Feature
Scenario Outline: As a user, I want to test Topics feature Create and Move topics

Scenario: TC045_Check Create topics button click and hover
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I wait for 2 seconds    
    Then I should verify that create topic button gets enabled for topic "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    When I hover over topic "Running the business"
    Then I wait for 2 seconds
    Then I should verify that create topic button gets enabled for topic "Running the business"
    Then I should verify that create topic button is not displayed for topic "Cars & Co"
    When I click on create topic button for the topic "Running the business"
    Then I should verify Create topic dropdown Items
    |Add topic|
    |Add child topic|

Scenario: TC046_Check Move topics button click and hover
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I wait for 2 seconds    
    Then I should verify that move topic button gets enabled for topic "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    When I hover over topic "Running the business"
    Then I wait for 2 seconds
    Then I should verify that move topic button gets enabled for topic "Running the business"
    Then I should verify that move topic button is not displayed for topic "Cars & Co"
    Then I should be able to click on tree branch "Running the business"
    Then I wait for right pane to load
    When I click on move topic button for the topic "Running the business"
    Then I should verify Move topic dropdown Items
    |Move to top|
    |Move up|
    |Move down|
    |Move to bottom|
    |Move level up|
    |Move level down|
    Then I should be able to logout of Improve    

