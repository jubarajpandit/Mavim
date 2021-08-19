Feature: 004_Editing topic feature. Rename, add and remove fields and relationships
Scenario Outline: As a user, I want to test topic name edit, add and remove fields

Scenario: TC008_Edit/Rename topic title
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon    
    Then I should be able to expand branch "Cars & Co"   
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to create a child topic named "Automation" under root topic "Cars & Co"
    Then I should be able to click on browser specific branch "Automation"
    Then I should be able to rename browser specific topic to "AutomationEdited"
    When I wait for the topic tree to load
    Then I should verify browser specific topic "AutomationEdited" is displayed in the topic tree
    Then I should be able to click on browser specific branch "AutomationEdited"
    Then I should be able to rename browser specific topic to "Automation"
    When I wait for the topic tree to load
    Then I should verify browser specific topic "AutomationEdited" is not displayed in the topic tree
    

Scenario: TC009_Add Relationship to a topic
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Management"
    Then I wait for right pane to load
    Then I should be able to add Relationship "When" with "Periodically"
    When I wait for the topic tree to load
 	Then I should be able to click on tree branch "Management"
    Then I should verify relationship "When" with "Periodically" appears in the right pane    

Scenario: TC010_Remove Relationship from topic
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Management"
    Then I wait for right pane to load
    Then I should be able to remove relationship for type "When" with "Periodically"
    When I click save on the edit page
    When I wait for the topic tree to load
    Then I should verify that the relationship are removed from the topic
    |Relationship Types|Topics|
    |When|Periodically|    

Scenario: TC011_Edit Text Field for a topic
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Management"
    Then I wait for right pane to load
    Then I should be able to edit text field named "Status" using text value "Automation Field Edit"    
    When I wait for the topic tree to load
    Then I should be able to click on tree branch "Management"
    Then I should verify that the edited text field for field "Status" is updated in the right pane

Scenario: TC012_Clean up
    Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to remove browser specific topics
    Then I should be able to logout of Improve