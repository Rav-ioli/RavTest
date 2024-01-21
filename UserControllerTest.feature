Feature: UserLogin
    As a user
    I want to be able to log in
    So that I can access my account

Scenario: Successful login
    Given I have entered a valid email and password
    When I press login
    Then I should be logged in