Feature: 005_Navigation Tree Feature
Scenario Outline: As a user, I want to test user navigation in the tree

Scenario: TC013_Navigate main topics and Verify Session Storeage
	Given I Login to Improve application
    Given I wait for Home Button
	Then I should verify that the session storeage contains the value for "Mavim Database"	
 	When I click on topic "Cars & Co" on the left pane
	Then I should verify that the session storeage contains the value for "Cars & Co"	
 	When I click on topic "Created Versions" on the left pane
	Then I should verify that the session storeage contains the value for "Created Versions"	
	
Scenario: TC014_Navigation to topics and verify session value and right pane
	Given I Login to Improve application
    Given I wait for Home Button	
	Then I should verify that the session storeage contains the value for "Mavim Database"
	When I click on topic "Cars & Co" on the left pane
 	Then the Right split screen should display "Cars & Co"	 	
	Then I should verify that the session storeage contains the value for "Cars & Co"
 	Then I should verify that the url contains the session value for "Cars & Co"
	Then I should verify that the session storeage contains the value for "Mavim Database"
	When I click on topic "Created Versions" on the left pane
 	Then the Right split screen should display "Created Versions"
 	Then I should verify that the session storeage contains the value for "Created Versions"
 	Then I should verify that the session storeage does not contain the value for "Cars & Co"
 	Then I should verify that the url contains the session value for "Created Versions"
	When I click on topic "Imported Versions" on the left pane	
 	Then the Right split screen should display "Imported Versions"
 	Then I should verify that the session storeage contains the value for "Imported Versions"
 	Then I should verify that the session storeage does not contain the value for "Cars & Co"
 	Then I should verify that the session storeage does not contain the value for "Created Versions"
 	Then I should verify that the url contains the session value for "Imported Versions"	

Scenario: TC015_Navigation to Main topics Navigate Back and forward
	Given I Login to Improve application	
	Given I wait for Home Button
	Then I should verify that the session storeage contains the value for "Mavim Database"
	When I click on topic "Cars & Co" on the left pane
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Cars & Co"
	When I click on topic "Created Versions" on the left pane	
	Then the Right split screen should display "Created Versions"
	Then I should verify that the session storeage contains the value for "Created Versions"
	When I click on topic "Imported Versions" on the left pane	
	Then the Right split screen should display "Imported Versions"
	Then I should verify that the session storeage contains the value for "Imported Versions"
	Then I should verify that the url contains the session value for "Imported Versions"
	When I click back button on the browser
	Then the Right split screen should display "Created Versions"
	Then I should verify that the session storeage contains the value for "Mavim Database"
	Then I should verify that the session storeage does not contain the value for "Imported Versions"
	When I click back button on the browser
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Mavim Database"
	Then I should verify that the session storeage contains the value for "Cars & Co"
	Then I should verify that the session storeage does not contain the value for "Imported Versions"
	Then I should verify that the session storeage does not contain the value for "Created Versions"
	When I click back button on the browser
	Then I should verify that the session storeage contains the value for "Mavim Database"
	Then I should verify that the session storeage does not contain the value for "Imported Versions"
	Then I should verify that the session storeage does not contain the value for "Created Versions"
	Then I should verify that the session storeage does not contain the value for "Cars & Co"	
	When I should click forward button on the browser
	Then the Right split screen should display "Cars & Co"
	Then I should verify that the session storeage contains the value for "Mavim Database"
	Then I should verify that the session storeage contains the value for "Cars & Co"
	When I should click forward button on the browser
	Then the Right split screen should display "Created Versions"
	Then I should verify that the session storeage contains the value for "Created Versions"
	Then I should verify that the session storeage does not contain the value for "Imported Versions"
	Then I should verify that the session storeage does not contain the value for "Cars & Co"
	When I should click forward button on the browser
	Then the Right split screen should display "Imported Versions"
	Then I should verify that the session storeage contains the value for "Imported Versions"
	Then I should verify that the session storeage does not contain the value for "Cars & Co"
	Then I should verify that the session storeage does not contain the value for "Created Versions"

Scenario: TC016_Login iMprove and navigate Main tree branches
	Given I Login to Improve application
	Given I wait for Home Button
	And I click on the Home Icon
	Then I should be able to click on tree branch "Cars & Co"
	Then I should be able to expand branch "Cars & Co"
	Then I should verify "Running the business" branch is available
	Then I should be able to click on tree branch "Running the business"
	Then I should be able to expand branch "Running the business"
	Then I should be able to collapse branch "Running the business"
	Then I should be able to click on tree branch "Created Versions"	

Scenario: TC017_Navigate Tree and check vertical side bar
	Given I Login to Improve application
	Given I wait for Home Button
	And I click on the Home Icon
	Then I should be able to click on tree branch "Cars & Co"
	Then the vertical sider bar should display "Mavim Database"
	Then I should be able to expand branch "Cars & Co"
	Then I should be able to click on tree branch "Running the business"
	#Then the Right split screen should display "Running the business"
	Then the vertical sider bar should display "Cars & Co"
	Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Management"
	Then the vertical sider bar should display "Running the business"
	#Then the Right split screen should display "Management"
	When I click on the vertical side bar
	#Then the Right split screen should display "Running the business"
	Then the vertical sider bar should display "Cars & Co"
	When I click on the vertical side bar
	#Then the Right split screen should display "Cars & Co"
	Then the vertical sider bar should display "Mavim Database"
	When I click on the vertical side bar
	Then I should verify that the side bar is not displayed
	Then I should be able to click on tree branch "Management"
	Then the vertical sider bar should display "Mavim Database"

Scenario: TC018_Navigate to user management and check session values
	Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
	When I navigate to User Management Page
	When I click on add user button
	Then I should verify that the session storeage count should not be null
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click back button on the browser
	When I click back button on the browser
    Then I should verify that the session storeage contains the value for "Mavim Database"

Scenario: TC019_Navigate to user management and check session values with loaded right and left panels
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Mavim Database"
    Then I should verify that the session storeage contains the value for "Cars & Co"
	When I navigate to User Management Page
	When I click on add user button
	Then I should verify that the session storeage count should not be null
    Then I should verify that the session storeage contains the value for "Mavim Database"
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click back button on the browser
    When I click back button on the browser
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Mavim Database"
    Then I should verify that the session storeage contains the value for "Cars & Co"

Scenario: TC020_Navigate to user management and check session values with Tree
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I wait for right pane to load
    Then I should be able to click on tree branch "Running the business"
    Then I wait for right pane to load
    Then I should verify that the session storeage contains the value for "Running the business"    
    Then I should verify that the session storeage contains the value for "Mavim Database"    
	When I navigate to User Management Page
	When I click on add user button
	Then I should verify that the session storeage count should not be null
    Then I should verify that the session storeage contains the value for "Mavim Database"
    Then I should verify that the session storeage contains the value for "Running the business"    
    When I click back button on the browser
    When I click back button on the browser
    When I wait for the topic tree to load
    Then I should verify that the tree is loaded
    Then I should verify that the session storeage contains the value for "Running the business"
    Then I should verify that the session storeage contains the value for "Mavim Database"
	Then I should be able to logout of Improve