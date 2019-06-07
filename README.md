# Epinova.Infrastructure

Real basic stuff for cross module sharing of code. We'll keep it simple in here.
Key features are extensions to EPiServer's ILogger interface for logging objects (not only strings). and a base class for making web api calls.

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Epinova.Infrastructure&metric=alert_status)](https://sonarcloud.io/dashboard?id=Epinova.Infrastructure)
[![Build status](https://ci.appveyor.com/api/projects/status/5gmhan996hx2aknm/branch/master?svg=true)](https://ci.appveyor.com/project/Epinova_AppVeyor_Team/epinova-infrastructure/branch/master)
![Tests](https://img.shields.io/appveyor/tests/Epinova_AppVeyor_Team/epinova-infrastructure.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Getting Started

### Epinova.Infrastructure.Logging

This namespace contains extension method to `EPiServer.Logging.ILogger` interface. Enables you to send in entire object instances for logging of coplex typs. The types will be serialized as JSON structures in the log message. This can be useful to avoid lots of string concatenation code in logger calls.

Use this:
```
var model = new { Foo = "foo", Bar = "bar" };
_log.Information(new { message = "some message", model });
```
Instead of this:
```
var model = new { Foo = "foo", Bar = "bar" };
_log.Information($"some message. Foo: {mdel.Foo}, Bar: {model.Bar}");
```

Select EPiServer properties will be excluded from being serialized in the log message. These include:
* XhtmlString
* XForm

Reference loops are ignored using built in loop handling form Newtonsoft.Json

If you have special needs for controlling how objects are serialized for logging purposes, you can decorate your model class being logged with the `ICustomLogMessage` contract. The implementation of `ICustomLogMessage.ToLoggableString()` will decide the log message.

```
public class Person : ICustomLogMessage
{
  public string Name { get; set; }
  public string SocialSecurityNumber { get; set; }

  public string ToLoggableString()
  {
    return $"Name: {Name}, SocialSecurityNumber: masked in logs";
  }
}
```

### Epinova.Infrastructure.RestServiceBase

Implement a service with a static read-only instance of System.Net.Http.HttpClient. Let it live – don't dispose it after each call. Inherit Epinova.Infrastructure.RestServiceBase, and use the CallAsync method to safely make API calls

```
public class AwesomeService : RestServiceBase
{
    private static readonly HttpClient Client = new HttpClient { BaseAddress = new Uri("https://some.api/address/") };
    private readonly ILogger _log;

    public AwesomeService(ILogger log) : base(log)
    {
        _log = log;
    }

    public async Task<Bar> CreateStuff(Foo payload)
    {
	    var responseMessage = await CallAsync(() => Client.PostAsync("api/method", new StringContent("serialized version og Foo")));
		//Even simpler, if using in combination with Microsoft.AspNet.WebApi.Client:
	    var responseMessage = await CallAsync(() => Client.PostAsync<Foo>("api/method", payload, new JsonMediaTypeFormatter()));

        if (responseMessage == null || !responseMessage.IsSuccessStatusCode)
        {
            _log.Information(new { message = "Unable to post payload", payload });
            return null;
        }

        Bar responseDto = await ParseJsonAsync<Bar>(responseMessage);

        if (responseDto.HasError)
        {
            _log.Information(new { message = "Unable to parse response data.", responseDto.ErrorMessage, payload });
            return null;
        }

        return responseDto;
    }
}
```

### Prerequisites

This module depends on EPiServer.Framework >= v11.1 for logging purposes. No setup or init code required.

### Installing

The module is published on nuget.org.

```
nuget install Epinova.Infrastructure
```

## Built With

* .Net Framework 4.6.2

## Authors

* **Tarjei Olsen** - *Initial work* - [apeneve](https://github.com/apeneve)

See also the list of [contributors](https://github.com/Epinova/Epinova.Infrastructure/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
