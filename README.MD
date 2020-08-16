# .NET Jira

A .NET wrapper for the Jira REST API.

Note: This library currently targets Jira Server, not Jira Cloud. Server uses OAuth1.0a. Jira Cloud uses [OAuth 2.0](https://developer.atlassian.com/cloud/jira/platform/oauth-2-authorization-code-grants-3lo-for-apps/).

I put this together because I couldn't find a .NET API client library which wrapped around the Jira API and getting OAuth to work requires a little bit of research. I hope to save some other developer hours getting setup.

Follow the steps [here](https://developer.atlassian.com/server/jira/platform/oauth/) to generate an OAuth token. This library expects you'll be using a PFX file. It's easiest to use [Keystore Explorer](https://keystore-explorer.org/) to combine the public and private key generated in the previous link into a PFX file. Afterwards you should have all the necessary files needed to authenticate via OAuth. Simply make a new class which inherits from the JiraRest class and you'll be off to the races.

I'll work on building out the different method calls over time and some generator code so you can easily code against your custom fields.