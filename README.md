# angularmusicstore

The MVC Music Store API

# Introduction Video

https://www.youtube.com/watch?v=OzqnTM4xXPs&feature=youtu.be

# How to start

# Set up Visual Studio 2015
```bash
1. Get Visual Studio 2015
2. Install ASP.NET 5 for windows here https://docs.asp.net/en/latest/getting-started/installing-on-windows.html

```

```bash
git clone https://github.com/SSWConsulting/musicstore-api-aspnet5
cd src/SSW.musicstore.API
run the command `dnu restore` for each project
build the solution
```

## Configurations

Music Store API uses few third party tools, namely:

1. [Seq](https://getseq.net/) - used for structured logging throughout the application.
2. [Raygun](https://raygun.com/) - used for app performance and error tracking
3. [ApplicationInsights](https://azure.microsoft.com/en-us/services/application-insights/) - used for app performance monitoring
4. [Auth0](https://auth0.com/) - used for user authentication

In order to run API you need to create configuration file for all these tools. In *Solution Explorer* go to *SSW.MusicStore.API* project and add new *privatesettings.json* file and paste the following:

```json
{
  "Seq": {
    "Url": "http://log.ssw.com:5341",
    "Key": "dev"
  },
  "Auth0": {
    "ClientId": "dev",
    "Domain": "ssw.auth0.com"
  },
  "RaygunSettings": {
    "ApiKey": "dev"
  },
  "ApplicationInsights": {
    "InstrumentationKey": "dev"
  }
}
```

This will ensure that API can run. As you can see all keys have been set as *dev*. If you are already using any of the third party tools listed above, then just populate correct settings. Otherwise, you can either leave it as it is or register using trial accounts (all of these tools give you a trial option).



