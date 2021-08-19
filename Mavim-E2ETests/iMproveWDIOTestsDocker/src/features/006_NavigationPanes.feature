Feature: 006_Navigation Panes Feature
Scenario Outline: As a user, I want to test user navigation on the right and left pane

Scenario: TC021_Navigate without tree, forward and back using side bar, check session values
	Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
	When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should click on the extreme left vertical bar
    Then I should verify that the session storeage does not contain the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    Then the Right split screen should display "Cars & Co"
    Then the Left pane should display "Mavim Database"

Scenario: TC022_Navigate without tree, forward and back using side bar and back native button, check session values
	Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
	When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should click on the extreme left vertical bar
    Then I should verify that the session storeage does not contain the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    Then the Right split screen should display "Cars & Co"
    Then the Left pane should display "Mavim Database"
	When I click back button on the browser
	Then I should verify that the session storeage contains the value for "Running the business"
	Then I should verify that the session storeage contains the value for "Cars & Co"
    Then the Right split screen should display "Running the business"
    When I click back button on the browser 
    Then I should verify that the session storeage contains the value for "Cars & Co"
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage does not contain the value for "Running the business"
    When I click back button on the browser
    Then I should check Right pane is not visible
    Then I should verify that the session storeage does not contain the value for "Cars & Co"   

Scenario: TC023_Navigate without tree, full screen on Left Pane and navigate to different topic
	Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
	When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I should verify that the session storeage contains the value for "Running the business"
    When I click on "Management" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Management"
    When I click full screen on the left pane
    Then I wait for 1 seconds
    Then I should check Right pane is not visible
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Management"    
    When I click on "Management" subtopic on right pane
    Then I should check Right pane is not visible
    When I click on topic "Support" on the left pane    
    Then I wait for right pane to load
    Then I should check Right pane is visible    
    Then I should verify that the session storeage does not contain the value for "Management"
    Then I should verify that the session storeage contains the value for "Support"

Scenario: TC024_Navigate without tree, full screen and exit full screen on Left Pane
	Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I should verify that the session storeage contains the value for "Running the business"
    When I click full screen on the left pane
    Then I wait for 1 seconds
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should check Right pane is not visible
    When I disable full screen on the left pane
    Then I wait for right pane to load
    Then I should check Right pane is visible
    Then the Right split screen should display "Running the business"
    
Scenario:  TC025_Navigate without tree, full screen on Right Pane
	Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    When I click full screen on the right pane
    Then I wait for 1 seconds
    Then I should verify left pane is not visible
    Then the left vertical bar displays text "Cars & Co"
    When I click on the extreme left vertical bar
    Then I should verify left pane is not visible
    Then I should verify that the session storeage does not contain the value for "Running the business"
    Then the left vertical bar displays text "Mavim Database"
    When I click on the extreme left vertical bar
    Then I should verify that the session storeage does not contain the value for "Cars & Co"    
    
Scenario:  TC026_Navigate without tree, full screen and exit on Right Pane
	Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    When I click full screen on the right pane
    Then I wait for 1 seconds
    Then I should verify left pane is not visible
    Then I should verify that the session storeage contains the value for "Running the business"
    Then the left vertical bar displays text "Cars & Co"
    When I disable full screen on the right pane
    Then I wait for right pane to load
    Then I should verify left pane is visible
    Then I should verify that the session storeage contains the value for "Running the business"
    Then the left vertical bar displays text "Mavim Database"
    When I click on the extreme left vertical bar
    Then I should verify that the session storeage does not contain the value for "Running the business"

Scenario: TC027_Navigate without tree, full screen Right Pane and navigate forward and back
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    When I click full screen on the right pane
    Then I wait for 1 seconds
    Then I should verify left pane is not visible
    When I click on "Management" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify left pane is not visible
    Then the left vertical bar displays text "Running the business"
    Then I should verify that the session storeage contains the value for "Management"
    When I click on the extreme left vertical bar
    Then I should verify that the session storeage does not contain the value for "Management"
    Then I should verify that the session storeage contains the value for "Running the business"
    Then the left vertical bar displays text "Cars & Co"
    When I click on the extreme left vertical bar
    Then I should verify that the session storeage does not contain the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    Then the left vertical bar displays text "Mavim Database"
    When I click on the extreme left vertical bar
    Then I should verify that the session storeage does not contain the value for "Cars & Co"
    Then I should verify that the session storeage contains the value for "Mavim Database"    

Scenario: TC028_Navigate without tree, full screen Right Pane and Left Pane
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    When I click full screen on the right pane
    Then I wait for 1 seconds
    Then I should verify left pane is not visible
    When I disable full screen on the right pane
    Then I wait for right pane to load
    Then I should verify left pane is visible
    When I click full screen on the left pane
    Then I wait for 1 seconds
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should check Right pane is not visible
    When I disable full screen on the left pane
    Then I wait for right pane to load
    Then I should check Right pane is visible    

Scenario: TC029_Navigate without tree, navigate to a link two topics
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon    
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"    
    Then I should be able to click on tree branch "Management"
    Then I wait for right pane to load
    When I navigate to link "navigateTopics"
    Then I wait for right pane to load
    Then I wait for the left pane to load
    Then the Right split screen should display "Running the business"
    Then the Left pane should display "Cars & Co"    
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click back button on the browser
    Then I wait for right pane to load    
    Then I should verify that the session storeage does not contain the value for "Running the business"
    Then I should verify that the session storeage does not contain the value for "Cars & Co"               
    Then the Right split screen should display "Management"
    Then the Left pane should display "Mavim Database"

Scenario: TC030_Navigate without tree, navigate to a link Single topic    
    Given I Login to Improve application
    Given I wait for Home Button
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    When I click on topic "Running the business" on the left pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Mavim Database"    
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I navigate to link "navigateToCars&Co"
    Then I wait for the left pane to load
    Then I should verify that the session storeage does not contain the value for "Running the business"
    Then I should verify that the session storeage does not contain the value for "Mavim database"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on topic "Running the business" on the left pane
    Then I wait for the left pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage does not contain the value for "Mavim database"    
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click back button on the browser
    Then the Left pane should display "Cars & Co"
    Then I should verify that the session storeage does not contain the value for "Running the business"
    Then I should verify that the session storeage does not contain the value for "Mavim database"
    Then I should verify that the session storeage contains the value for "Cars & Co"     
    
Scenario: TC031_Navigate without tree, navigate to a link Single topic and open tree through link   
    Given I Login to Improve application
    Given I wait for Home Button
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    When I click on topic "Running the business" on the left pane
    Then I wait for right pane to load
    When I navigate to link "navigateToCars&Co"
	Then I wait for the left pane to load
	When I navigate to link "navigateToCars&CoWithTree"
	Then I wait for the left pane to load
	Then I should verify that the tree is loaded
    Then I should verify that the url contains the session value for "Cars & Co"
    Then I should verify that the url contains the value "treeOpen=1"
	When I click on topic "Running the business" on the left pane
	Then I wait for right pane to load
	Then I should verify that the url contains the session value for "Cars & Co"
	Then I should verify that the url contains the session value for "Running the business"
    Then I should verify that the tree is loaded
    Then I should verify url does not contain the value "treeOpen=1"
	When I close the tree pane
	Then I wait for 2 seconds
	Then I should verify that the tree is closed

Scenario: TC032_Navigate without tree, Where am I on the Right pane
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I wait for right pane to load
    When I click on "Management" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Management"
    When I click where am I button on the right pane
    When I wait for the topic tree to load
    Then I should verify that the tree is loaded
    Then I should verify tree item "Management" is selected	
    
 Scenario: TC033_Navigate without tree, Where am I on the Left pane
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I wait for right pane to load
    When I click on "Management" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Management"
    When I click where am I button on the left pane
    When I wait for the topic tree to load
    Then I should verify that the tree is loaded
    Then I should verify tree item "Running the business" is selected

Scenario: TC034_Navigate without tree Open and close tree 
    Given I Login to Improve application
    Given I wait for Home Button
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    When I click on topic "Running the business" on the left pane
    Then I wait for right pane to load
    And I click on the Home Icon
    Then I wait for right pane to load
    Then I should verify that the tree is loaded
    When I click on "Management" subtopic on right pane    
    Then the vertical sider bar should display "Running the business"
    When I close the tree pane
	Then I wait for 2 seconds
	Then I should verify that the tree is closed
    Then the Left pane should display "Running the business"    	    

Scenario: TC035_Navigate to same branch and check session storeage count
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then I wait for right pane to load
    Then the Right split screen should display "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage count should be 3
    When I click on topic "Running the business" on the left pane    
    Then I should verify that the session storeage count should be 3

Scenario: TC036_Navigate without tree, Where am I on the Right pane and click full screen and collapse
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I wait for right pane to load
    When I click on "Management" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Management"
    When I click where am I button on the right pane
    When I wait for the topic tree to load
    Then I should verify that the tree is loaded
    When I click full screen on the right pane
    Then I wait for 1 seconds
    Then I should verify that the tree is closed
    When I disable full screen on the right pane
    Then I wait for right pane to load
    Then I should verify left pane is visible
    Then I should verify that the tree is closed

Scenario: TC037_Navigate to a link and enable tree
    Given I Login to Improve application
    Given I wait for Home Button
    When I navigate to link "navigateTopics"
    Then I wait for right pane to load
    Then I wait for the left pane to load
    Then the Right split screen should display "Running the business"
    Then the Left pane should display "Cars & Co"
    When I click on the Home Icon
    When I wait for the topic tree to load
    Then I should verify that the tree is loaded
    Then I should verify tree item "Running the business" is selected

Scenario: TC038_Click on link under Fields table
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to expand branch "Fields examples"
	Then I should be able to click on tree branch "3 Field example with fieldrelations"
    When I click on Fields link "With What" on right pane
    Then the Right split screen should display "With What"

Scenario: TC039_Click on link under Relationship table
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to click on tree branch "Management"
    When I click on Relationship link "Running the business" on right pane
    Then the Right split screen should display "Running the business"
    Then I should be able to logout of Improve        