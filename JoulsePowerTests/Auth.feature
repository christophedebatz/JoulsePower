
Feature: Auth

		I want to check that all user can log in themself 
		as an API user, with roles suppport

@Auth
Scenario: Log in user
        Given login API URI is <uri>
		When I request for login with username is <username> and password is <password>
		Then response have 200 status code