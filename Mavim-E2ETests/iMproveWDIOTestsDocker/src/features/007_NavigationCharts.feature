Feature: 007_Navigation Charts Feature
Scenario Outline: As a user, I want to test user to navigate to charts and verify shapes

Scenario: TC040_Navigate to network chart and verify shapes
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should click on the chart "Network Chart"    
    Then I should verify that the chart contains topic "Running the business"
    Then I should be able to verify the charts have the following shapes    
    |Running the business|
    |Management|
    |Support|
    |Improvement|
    |Operations|

Scenario: TC041_Navigate to Idef0 chart and verify shapes
    Given I Login to Improve application
    Given I wait for Home Button
    Then I should verify that the session storeage contains the value for "Mavim Database"
    When I click on topic "Cars & Co" on the left pane
    Then the Right split screen should display "Cars & Co"
    Then I should verify that the session storeage contains the value for "Cars & Co"
    When I click on "Running the business" subtopic on right pane
    Then I wait for right pane to load
    Then I should click on the chart "IDEF0 Chart"    
    Then I should verify that the chart contains topic "Running the business"
    Then I should be able to verify the charts have the following shapes    
    |Running the business|
    |Management|
    |Support|
    |Improvement|
    |Operations|
    |Statistics|
    |Files examples|
    |Fields examples|
    |8 chart types on a topic|
    |Flowchart with Subprocess shape|
    |BPMN 2.0 Model|
    |ArchiMate Model|
    |IDEF0 with multiple layers|
    |Flowcharts point to each other|
    |Matrix collection|
    |Custom icons|
    |Field examples with inheritance|
    |Fields with dates|
    |Here a topic without a name|
    |Topic with a long name for a matrix and a report|
    |User- en Dispatch instruction examples|
    |Hierarchical chart|
    |Flowchart without title and topic names|
    |Different hyperlinks|
    |Example of multiple hyperlinks to the same topic|
    |Connected with Dynamics AX|
    |Connected with Infor M3|
    |Connected with SAP|
    |Nasty 6048|
    |Overview of suppliers|
    |Daily planning|
    |Nasty 6048 with descriptions|
    |Descriptions growing large|
    |Description test 22345|
    |Connected with SAP, Dynamics AX and M3|
    |Nasty 6048 with descriptions|
    |Taxonomy test topics 5003|
    Then I should be able to logout of Improve