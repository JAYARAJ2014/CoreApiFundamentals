# CoreApiFundamentals

## Use Models instead of Entities

Payload is a contrract with users
Likely want to filter data for security
Use surrogate keys


## Automapper feature

Automapper maps by the name by default. If you have a related object such as Location  with the followng property

`        public string VenueName { get; set; }`

you can include this in the Dto with the following name so that automapper will automatically map it. If you dont like this method then you have to explicitly map them in the profile.

`        public string LocationVenueName { get; set; }`


## Model Binding

` [FromBody] ` or ` [ApiController] ` can be used. APicontroller from ASp.net core 2.1 onwards only


APIController attribute will apply default behavior to validations . You can turn this off and do ModelState.IsValid if required.


### API Versioning 

Specifiying Version in Uri path or query string
Versioning with Headers
Accept Header  : applicatino/json ;version=v2
Content-Type versioning 

Add the nuget package dotnet add package Microsoft.AspNetCore.Mvc.Versioning
in Startup.cs include services.AddApiVersioning();
`    services.AddMvc(opt=>opt.EnableEndpointRouting=true)`
