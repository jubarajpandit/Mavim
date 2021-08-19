Feature: 012_Move topics up, down, level up, down, top and bottom
Scenario Outline: As a user, I want to test move topics

Scenario: TC065_Create Data for Move topics
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to remove browser specific topics
    Then I should be able to create a child topic named "AutomationMoveTopic" under root topic "Cars & Co"
    Then I should be able to create a child topic named "Topic1" under topic "AutomationMoveTopic"
    Then I should be able to create a child topic named "Topic2" under topic "AutomationMoveTopic"
    Then I should be able to create a child topic named "Topic3" under topic "AutomationMoveTopic"

Scenario: TC066_Move topic Up and Down
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"    
    Then I should be able to expand browser specific branch "AutomationMoveTopic"
    Then I should be able to click on browser specific branch "Topic1"
    Then I should be able to move down topic "Topic1"
    Then I should be able to move up topic "Topic1"

Scenario: TC067_Move topic top and bottom
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"    
    Then I should be able to expand browser specific branch "AutomationMoveTopic"
    Then I should be able to click on browser specific branch "Topic1"
    Then I should be able to move to bottom topic "Topic1"
    Then I should be able to move to top topic "Topic1"

Scenario: TC068_Move Topic Level down and up
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"    
    Then I should be able to expand browser specific branch "AutomationMoveTopic"
    Then I should be able to click on browser specific branch "Topic2"
    Then I should be able to move level down topic "Topic2"
    Then I should be able to collapse browser specific branch "Topic1"
    Then I should verify browser specific topic "Topic2" is not displayed in the topic tree
    Then I should be able to expand browser specific branch "Topic1"
    Then I should be able to move level up topic "Topic2"
    Then I should verify browser specific topic "Topic2" is displayed in the topic tree

Scenario: TC069_Move Topic negative tests
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co"
    When I try to move up the topic "Cars & Co"
    Then I should get an error in the application stating "Cannot move given topic one position up in the branch"
    When I try to move down the topic "Cars & Co"
    Then I should get an error in the application stating "Cannot move given topic one position down in the branch"
    When I try to move the topic "Cars & Co" to the top
    Then I should get an error in the application stating "Cannot move given topic to the top of the branch"
    When I try to move the topic "Cars & Co" to bottom
    Then I should get an error in the application stating "Cannot move topic to the bottom of the branch"
    When I try to move the topic "Cars & Co" One level down
    Then I should get an error in the application stating "Cannot move given topic one level down"
    When I try to move the topic "Cars & Co" One level up
    Then I should get an error in the application stating "Cannot move given topic one level up"
    When I try to move up the topic "Recycle Bin"
    Then I should get an error in the application stating "Cannot move given topic one position up in the branch"
    When I try to move down the topic "Recycle Bin"
    Then I should get an error in the application stating "Cannot move given topic one position down in the branch"
    When I try to move the topic "Recycle Bin" to the top
    Then I should get an error in the application stating "Cannot move given topic to the top of the branch"
    When I try to move the topic "Recycle Bin" to bottom
    Then I should get an error in the application stating "Cannot move topic to the bottom of the branch"
    When I try to move the topic "Recycle Bin" One level down
    Then I should get an error in the application stating "Cannot move given topic one level down"
    When I try to move the topic "Recycle Bin" One level up
    Then I should get an error in the application stating "Cannot move given topic one level up"    
    When I try to move up the topic "Created Versions"
    Then I should get an error in the application stating "Cannot move given topic one position up in the branch"
    When I try to move down the topic "Created Versions"
    Then I should get an error in the application stating "Cannot move given topic one position down in the branch"
    When I try to move the topic "Created Versions" to the top
    Then I should get an error in the application stating "Cannot move given topic to the top of the branch"
    When I try to move the topic "Created Versions" to bottom
    Then I should get an error in the application stating "Cannot move topic to the bottom of the branch"
    When I try to move the topic "Created Versions" One level down
    Then I should get an error in the application stating "Cannot move given topic one level down"
    When I try to move the topic "Created Versions" One level up
    Then I should get an error in the application stating "Cannot move given topic one level up"
    When I try to move up the topic "Imported Versions"
    Then I should get an error in the application stating "Cannot move given topic one position up in the branch"
    When I try to move down the topic "Imported Versions"
    Then I should get an error in the application stating "Cannot move given topic one position down in the branch"
    When I try to move the topic "Imported Versions" to the top
    Then I should get an error in the application stating "Cannot move given topic to the top of the branch"
    When I try to move the topic "Imported Versions" to bottom
    Then I should get an error in the application stating "Cannot move topic to the bottom of the branch"
    When I try to move the topic "Imported Versions" One level down
    Then I should get an error in the application stating "Cannot move given topic one level down"
    When I try to move the topic "Imported Versions" One level up
    Then I should get an error in the application stating "Cannot move given topic one level up"
    When I try to move up the topic "Relationship Categories"
    Then I should get an error in the application stating "Cannot move given topic one position up in the branch"
    When I try to move down the topic "Relationship Categories"
    Then I should get an error in the application stating "Cannot move given topic one position down in the branch"
    When I try to move the topic "Relationship Categories" to the top
    Then I should get an error in the application stating "Cannot move given topic to the top of the branch"
    When I try to move the topic "Relationship Categories" to bottom
    Then I should get an error in the application stating "Cannot move topic to the bottom of the branch"
    When I try to move the topic "Relationship Categories" One level up
    Then I should get an error in the application stating "Cannot move given topic one level up"
    When I try to move the topic "Relationship Categories" One level down
    Then I should get an error in the application stating "Cannot move given topic one level down"

Scenario: TC070_Cannot Move to top or up if the topic is already at top
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"    
    Then I should be able to expand browser specific branch "AutomationMoveTopic"
    Then I should be able to click on browser specific branch "Topic1"
    When I try to move the topic "Topic1" to the top
    Then I should get an error in the application stating "Cannot move given topic to the top of the branch"
    When I try to move up the topic "Topic1"
    Then I should get an error in the application stating "Cannot move given topic one position up in the branch"

Scenario: TC071_Cannot Move to bottom or down if the topic is already at the bottom
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Cars & Co" 
    Then I should be able to collapse branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"    
    Then I should be able to expand browser specific branch "AutomationMoveTopic"
    Then I should be able to click on browser specific branch "Topic3"
    When I try to move the topic "Topic3" to bottom
    Then I should get an error in the application stating "Cannot move topic to the bottom of the branch"
    When I try to move down the topic "Topic3"
    Then I should get an error in the application stating "Cannot move given topic one position down in the branch"

Scenario: TC072_Should be not be able to move topics under Created and imported versions one level up or down
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to expand branch "Imported Versions"
    Then I should be able to click on tree branch "Nasty 3001"
    When I try to move the topic "Nasty 3001" One level down
    Then I should get an error in the application stating "Cannot move given topic one level down"
    When I try to move the topic "Nasty 3001" One level up
    Then I should get an error in the application stating "Cannot move given topic one level up"    
    Then I should be able to expand branch "Created Versions"
    Then I should be able to click on tree branch "Running some business 15051"
    When I try to move the topic "Running some business 15051" One level down
    Then I should get an error in the application stating "Cannot move given topic one level down"
    When I try to move the topic "Running some business 15051" One level up
    Then I should get an error in the application stating "Cannot move given topic one level up"

Scenario: TC073_Clean up
    Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I should be able to remove browser specific topics
    Then I should be able to logout of Improve    