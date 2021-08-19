Feature: 010_Edit Fields feature
Scenario Outline: As a user, I want to test Edit fields feature

Scenario: TC047_Navigate to edit page and cancel without editing
    Given I Login to Improve application
    Given I wait for Home Button
    And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should get Edit Page
    When I cancel without editing
    When I wait for the topic tree to load
    Then I should verify that the tree is loaded

Scenario: TC048_Cancel Topic Name change
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
	Then I should be able to click on tree branch "Management"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should get Edit Page
    Then I edit the topic name to "Management Edited"
    When I click cancel on the edit page
    When I wait for the topic tree to load
    Then I should verify topic "Management Edited" is not displayed in the topic tree

Scenario: TC049_Edit Fields scenario : Edit Text Field
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to expand branch "Fields examples"
	Then I should be able to click on tree branch "1 Example Field Set (filled)"
    Then I wait for right pane to load
    Then I should be able to edit text field named "Status" using text value "Automation Field Edit"    
    When I wait for the topic tree to load
    Then I should be able to click on tree branch "1 Example Field Set (filled)"
    Then I should verify that the edited text field for field "Status" is updated in the right pane

Scenario: TC050_Edit Fields scenario : Edit All Fields
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to expand branch "Fields examples"
	Then I should be able to click on tree branch "1 Example Field Set (filled)"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should be able to change field values for the following
    |Field Name|Field Value|
    |Status|AutomationTesting|
    |1Text|TestAutomation!@#$%^&*90124|
    |1Number|1234567809|
    |1Decimal Number|12345.67809|    
    |1Date|01/01/2021|
    |1Text as Hyperlink|https://www.mavim.com/|
    Then I should be able to change boolean field "1Yes/No" to "false"    
    Then I should be able to select list item field "1List" to "List item 3"
    Then I should be able to select Relationship List item "1Relation-list" to "workshop"
    Then I should be able to select Relationship Field "1Relationship" to "Arjan de Zwart"
    Then I should be able to save the Edit Page
    When I wait for the topic tree to load
    Then I should be able to verify the field values on the Right pane
    |Field Name|Field Value|
    |Status|AutomationTesting|
    |1Text|TestAutomation!@#$%^&*90124|
    |1Number|1234567809|
    |1Decimal Number|12345.67809|    
    |1Date|1 januari 2021|
    |1Text as Hyperlink|https://www.mavim.com/|
    |1Yes/No|No|
    |1List|List item 3|
    |1Relationship|Arjan de Zwart|
    |1Relation-list|workshop|

Scenario: TC051_Edit Fields scenario : Edit multiline and multi value fields
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to expand branch "Fields examples"
	Then I should be able to click on tree branch "2 Example Field Set (empty)"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should be able to change multi line field values for the following
    |Field Name|Field Value|
    |Status|AutomationTesting|
    |2Text MultiLine|Line1,Line2,Line3|
    |2Text MultiLine_MultiValue|Line1,Line2,Line3|
    |2Text MultiLine_MultiValue_MultiLanguage|Line1,Lien2,#$#$%^,Test4,样品|
    Then I should be able to save the Edit Page
    When I wait for the topic tree to load
    Then I should be able to verify the multiline field values on the Right pane
    |Field Name|Field Value|
    |Status|AutomationTesting|
    |2Text MultiLine|Line1,Line2,Line3|
    |2Text MultiLine_MultiValue|Line1,Line2,Line3|
    |2Text MultiLine_MultiValue_MultiLanguage|Line1,Lien2,#$#$%^,Test4,样品|
  
Scenario: TC052_Edit Fields scenario : Edit Field validation for decimal, number 
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to expand branch "Fields examples"
	Then I should be able to click on tree branch "1 Example Field Set (filled)"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should be able to change field values for the following
    |Field Name|Field Value|
    |Status|AutomationTesting|
    |1Text|TestAutomation!@#$%^&*90124 fdfd|
    |1Number|12345sdads67809|
    |1Decimal Number|12345.6sdsd.ssd7809|    
    Then I should be able to save the Edit Page
    When I wait for the topic tree to load
    Then I should be able to verify the field values on the Right pane
    |Field Name|Field Value|
    |Status|AutomationTesting|
    |1Text|TestAutomation!@#$%^&*90124 fdfd|
    |1Number|1234567809|
    |1Decimal Number|12345.67809|    

Scenario: TC053_Edit Fields scenario : Edit Field validation URL
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to expand branch "Fields examples"
	Then I should be able to click on tree branch "1 Example Field Set (filled)"
    Then I wait for right pane to load
    Then I should be able to edit text field named "1Text as Hyperlink" using text value "test invalid URL"    
    Then I should get a validation error in Edit Page
    Then I should be able to close the notifiction message
    When I click cancel on the edit page
    When I wait for the topic tree to load
    Then I should be able to verify the field values on the Right pane
    |Field Name|Field Value|
    |1Text as Hyperlink|https://www.mavim.com/|

Scenario: TC054_Edit Fields scenario : Edit multiline and multi value fields
	Given I Login to Improve application
    Given I wait for Home Button
	And I click on the Home Icon
    Then I should be able to click on tree branch "Cars & Co"
    Then I should be able to expand branch "Cars & Co"
	Then I should be able to expand branch "Running the business"
    Then I should be able to expand branch "Fields examples"
	Then I should be able to click on tree branch "2 Example Field Set (filled)"
    Then I wait for right pane to load
    When I click edit button on the right panel
    Then I should be able to change multi line, multi field values for the following
    |Field Name|Field Value 1|Field Value 2|
    |2Text MultiLine_MultiValue|Value 1 Line1, Value 1 Line2, Value 1 Line3|Value 2 Line1, Value 2 Line2, Value 2 Line3|
    |2Text MultiLine_MultiValue_MultiLanguage|Value 1 Line1,Value 1 Line2,Value 1 #$#$%^,Value 1 Test4, Value 1 样品|Value 2 Line1,Value 2 Line2,Value 2 #$#$%^,Value 2 Test4, Value 2 样品|
    |2Text Multivalue|https://www.google.nl|https://www.mavim.com|
    |2Decimal Number_MultiValue|1234.4|4567.8|
    |2Date_MultiValue|01/01/2035|03/12/2023|
    |2Number_MultiValue|1947|11225524|
    Then I should be able to change Multi value Relationship field values for the following
    |Field Name|Field Value1|Field Value2|
    |2Relationship_MultiValue|Forms|Standard Word documents|
    Then I should be able to save the Edit Page
    When I wait for the topic tree to load
    Then I should be able to verify the multiline field values on the Right pane
    |Field Name|Field Value|
    |2Text MultiLine_MultiValue|Value 1 Line1, Value 1 Line2, Value 1 Line3|
    |2Text MultiLine_MultiValue|Value 2 Line1, Value 2 Line2, Value 2 Line3|
    |2Text MultiLine_MultiValue_MultiLanguage|Value 1 Line1,Value 1 Line2,Value 1 #$#$%^,Value 1 Test4, Value 1 样品|
    |2Text MultiLine_MultiValue_MultiLanguage|Value 2 Line1,Value 2 Line2,Value 2 #$#$%^,Value 2 Test4, Value 2 样品|
    |2Text Multivalue|https://www.google.nl|
    |2Text Multivalue|https://www.mavim.com|
    |2Decimal Number_MultiValue|1234.4|
    |2Decimal Number_MultiValue|4567.8|
    |2Number_MultiValue|11225524|
    |2Number_MultiValue|1947|
    |2Date_MultiValue|1 januari 2035|
    |2Date_MultiValue|12 maart 2023|
    |2Relationship_MultiValue|Forms|
    |2Relationship_MultiValue|Standard Word documents|
    Then I should be able to logout of Improve