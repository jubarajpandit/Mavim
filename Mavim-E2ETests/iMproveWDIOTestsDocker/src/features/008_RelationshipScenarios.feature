Feature: 008_Relationships feature
Scenario Outline: As a user, I want to test adding, removing relationships to a topic

Scenario: TC042_Add All relationship types to a topic
    Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Improvement"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should be able to add Relationship types to topics
    |Relationship Types|Topics|
    |With What|Work instructions|
    |Whereto|Go To Command without Destination|
    |Who|Staff|
    |Where|workshop|
    |When|Date|
    |Why|Company Guidelines|
    When I click save on the edit page
    When I wait for the topic tree to load
    Then I should verify that the following relationship Types are added to topics
    |Relationship Types|Topics|
    |With What|Work instructions|
    |Whereto|Go To Command without Destination|
    |Who|Staff|
    |Where|workshop|
    |When|Date|
    |Why|Company Guidelines|
    Then I should be able to logout of Improve

Scenario: TC043_Remove All relationship types from a topic
    Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Improvement"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I wait for 10 seconds
    Then I should be able to remove Relationship types to topics
    |Relationship Types|Topics|
    |With What|Work instructions|
    |Whereto|Go To Command without Destination|
    |Who|Staff|
    |Where|workshop|
    |When|Date|
    |Why|Company Guidelines|
    When I click save on the edit page    
    When I wait for the topic tree to load
    Then I should verify that the relationship are removed from the topic
    |Relationship Types|Topics|
    |With What|Work instructions|
    |Whereto|Go To Command without Destination|
    |Who|Staff|
    |Where|workshop|
    |When|Date|
    |Why|Company Guidelines|

Scenario: TC044_Add relationship and cancel
    Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Improvement"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I wait for 5 seconds
    Then I should be able to add Relationship types to topics
    |Relationship Types|Topics|
    |With What|Work instructions|
    |Whereto|Go To Command without Destination|
    |Who|Staff|
    |Where|workshop|
    |When|Date|
    |Why|Company Guidelines|
    Then I click cancel on the edit page
    When I wait for the topic tree to load
    Then I should verify that the relationship are removed from the topic
    |Relationship Types|Topics|
    |With What|Work instructions|
    |Whereto|Go To Command without Destination|
    |Who|Staff|
    |Where|workshop|
    |When|Date|
    |Why|Company Guidelines|
    Then I should be able to logout of Improve

Scenario: TC045_Add Relationship to a new topic
    Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to delete topic named "RelateTopic"
    Then I should be able to create a child topic named "RelateTopic" under root topic "Cars & Co"
    Then I should be able to click on browser specific branch "RelateTopic"
    When I click edit button on the right panel
    Then I should verify Add Relationship Button is displayed in Edit Page
    Then I should get a message "This topic does not contain relationships." in Relationship Table of the edit page
    Then I should be able to add Relationship types to topics
    |Relationship Types|Topics|
    |With What|Work instructions|
    Then the message "This topic does not contain relationships." is not displayed in Relationship Table of the edit page
    When I click save on the edit page    
    When I wait for the topic tree to load
    Then I should verify that the following relationship Types are added to topics
    |Relationship Types|Topics|
    |With What|Work instructions|