Feature: 011_Add, Remove and Move topics
Scenario Outline: As a user, I want to test Add and Remove topics feature

Scenario: TC055_Create a topic under main branch
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to remove browser specific topics    
    Then I should be able to delete topic named "Automation"    
    Then I should be able to create a child topic named "Automation" under root topic "Cars & Co"
    
Scenario: TC056_Create a child topic under a topic
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to delete topic named "ParentTopic"
    Then I should be able to create a child topic named "ParentTopic" under root topic "Cars & Co"
    Then I should be able to create a child topic named "ChildTopic" under topic "ParentTopic"
    Then I should be able to click on browser specific branch "ChildTopic"
    Then I should be able to collapse browser specific branch "ParentTopic"
    Then I should verify browser specific topic "ChildTopic" is not displayed in the topic tree

Scenario: TC057_Delete Child and parent Topic
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to create a child topic named "ParentTopic_Delete" under root topic "Cars & Co"
    Then I should be able to create a child topic named "ChildTopic_Delete" under topic "ParentTopic_Delete"
    Then I should be able to click on browser specific branch "ChildTopic_Delete"
    Then I should be able to delete topic named "ChildTopic_Delete"
    Then I should be able to delete topic named "ParentTopic_Delete"
    Then I should be able to expand branch "Recycle Bin"
    Then I should verify browser specific topic "ChildTopic_Delete" is displayed in the topic tree
    Then I should be able to collapse branch "Recycle Bin"

Scenario: TC058_Create Topic with type and Icon and navigate on panes
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to delete topic named "AutomationTest"
    Then I should be able to create a child topic named "AutomationTest" under root topic "Cars & Co"
    Then I should be able to create a child topic named "ChildTopic" of type "Standard Topic" and icon "Folder brown" under topic "AutomationTest"
    Given I Login to Improve application
    Given I wait for Home Button
    When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
    When I click on browser specific "AutomationTest" subtopic on right pane
    Then the Right split screen should display "AutomationTest"   
    When I click on browser specific "ChildTopic" subtopic on right pane
    Then the Right split screen should display "ChildTopic"
    Then I wait for right pane to load

Scenario: TC059_Created topic is found under Relationship popup
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to delete topic named "ParentTopic"
    Then I should be able to create a child topic named "ParentTopic" under root topic "Cars & Co"
    Then I should be able to click on browser specific branch "ParentTopic"
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Management"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should get Edit Page
    When I click on Add Relationship button
    Then I should be able to choose "With What" option in the relationship popup
    Then I collapse branch "Relationship Categories" under Relationship PopUp
    Then I should verify that the topic "ParentTopic" is available in relationship popup
    Then I should click topic "ParentTopic" in relationship popup
    When I click save on the edit page
    When I wait for the topic tree to load
    Then I should verify that the tree is loaded
    Then I wait for right pane to load
    Then I should verify that the following relationship Types are added to topics
    |Relationship Types|Topics|
    |With What|ParentTopic|
    
Scenario: TC060_Delete Topic Clean Up
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"    
    Then I should be able to delete topic named "Automation"
    Then I should be able to delete topic named "AutomationTest"
    Then I should be able to delete topic named "ParentTopic"

Scenario: TC061_Delete Topic from Recycle bin
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"  
    Then I should be able to expand branch "Recycle Bin"
    Then I should be able to remove browser specific topics

Scenario: TC062_Should not be able to create topic under Recycle Bin
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    When I click on create topic button for the topic "Recycle Bin"
    Then I should verify Create topic dropdown Items
    |Add topic|
    Then I verify there is no "Add child topic" while creating topic

Scenario: TC063_Should not be able to create topic under Created Versions and Imported Versions
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Created Versions"
    Then I should be able to collapse branch "Created Versions"
    Then I should be able to expand branch "Created Versions"
    Then I should be able to click on tree branch "Created Versions"
    Then I should verify that create topic button is not displayed for topic "Created Versions"
    Then I should be able to click on tree branch "Imported Versions"
    Then I should verify that create topic button is not displayed for topic "Imported Versions"
    Then I should be able to click on tree branch "Running some business 15051"
    When I click on create topic button for the topic "Running some business 15051"
    Then I should verify Create topic dropdown Items
    |Remove topic|
    Then I verify there is no "Add child topic" while creating topic
    Then I should be able to collapse branch "Created Versions"
    Then I should be able to expand branch "Imported Versions"
    Then I should be able to click on tree branch "Nasty 3001"
    When I click on create topic button for the topic "Nasty 3001"
    Then I should verify Create topic dropdown Items
    |Remove topic|
    Then I verify there is no "Add child topic" while creating topic
    Then I should be able to collapse branch "Imported Versions"

Scenario: TC064_Clean up
    Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to remove browser specific topics
    Then I should be able to logout of Improve    