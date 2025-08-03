# Created with Openapi Generator

<a id="cli"></a>
## Creating the library
Create a config.yaml file similar to what is below, then run the following powershell command to generate the library `java -jar "<path>/openapi-generator/modules/openapi-generator-cli/target/openapi-generator-cli.jar" generate -c config.yaml`

```yaml
generatorName: csharp
inputSpec: openapi.json
outputDir: out

# https://openapi-generator.tech/docs/generators/csharp
additionalProperties:
  packageGuid: '{ABE730EA-8983-4AD2-84FD-8C2AB7177029}'

# https://openapi-generator.tech/docs/integrations/#github-integration
# gitHost:
# gitUserId:
# gitRepoId:

# https://openapi-generator.tech/docs/globals
# globalProperties:

# https://openapi-generator.tech/docs/customization/#inline-schema-naming
# inlineSchemaOptions:

# https://openapi-generator.tech/docs/customization/#name-mapping
# modelNameMappings:
# nameMappings:

# https://openapi-generator.tech/docs/customization/#openapi-normalizer
# openapiNormalizer:

# templateDir: https://openapi-generator.tech/docs/templating/#modifying-templates

# releaseNote:
```

<a id="usage"></a>
## Using the library in your project

```cs
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;
using Org.OpenAPITools.Extensions;

namespace YourProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var api = host.Services.GetRequiredService<IActivityApi>();
            IGetActivityLogsApiV1ActivitiesGetApiResponse apiResponse = await api.GetActivityLogsApiV1ActivitiesGetAsync("todo");
            PaginatedListActivityEvents? model = apiResponse.Ok();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
          .ConfigureApi((context, services, options) =>
          {
              options.ConfigureJsonOptions((jsonOptions) =>
              {
                  // your custom converters if any
              });

              options.AddApiHttpClients(client =>
              {
                  // client configuration
              }, builder =>
              {
                  builder
                      .AddRetryPolicy(2)
                      .AddTimeoutPolicy(TimeSpan.FromSeconds(5))
                      .AddCircuitBreakerPolicy(10, TimeSpan.FromSeconds(30));
                      // add whatever middleware you prefer
                  }
              );
          });
    }
}
```
<a id="questions"></a>
## Questions

- What about HttpRequest failures and retries?
  Configure Polly in the IHttpClientBuilder
- How are tokens used?
  Tokens are provided by a TokenProvider class. The default is RateLimitProvider which will perform client side rate limiting.
  Other providers can be used with the UseProvider method.
- Does an HttpRequest throw an error when the server response is not Ok?
  It depends how you made the request. If the return type is ApiResponse<T> no error will be thrown, though the Content property will be null.
  StatusCode and ReasonPhrase will contain information about the error.
  If the return type is T, then it will throw. If the return type is TOrDefault, it will return null.
- How do I validate requests and process responses?
  Use the provided On and After partial methods in the api classes.

## Api Information
- appName: Todoist API
- appVersion: 1
- appDescription: # Developing with Todoist  Thanks for your interest in developing apps with Todoist. In this section we will provide an overview of the API we offer and cover some common topics for application development using them.  You can use our API for free, but depending on your Todoist account plan (or that of the authenticated user), some features may be restricted.  Please consider subscribing to the [Todoist API mailing list](https://groups.google.com/a/doist.com/g/todoist-api) to get important updates.  ## Our API  Our API uses an approach that should be familiar to anyone with experience calling [RESTful](https://en.wikipedia.org/wiki/Representational_state_transfer) APIs.  We also have a special endpoint called &#x60;/sync&#x60;, which is used by our first-party clients to keep the data updated locally without having to make many separate requests to the API. Anyone can use it, and some actions will only be available via &#x60;/sync&#x60;. The format is unconventional compared to current API standards, but it is our main driver for first-party apps.  ## Our SDKs  Our Python and JavaScript SDKs streamline working with the Todoist API, and can be installed from the main package registries for each ecosystem.  For instructions, examples, and reference documentation, visit their pages:  - [Todoist Python SDK](https://doist.github.io/todoist-api-python/) - [Todoist TypeScript SDK](https://doist.github.io/todoist-api-typescript/)  ## Pagination  Paginated endpoints are marked by having the &#x60;next_cursor&#x60; attribute in the response.  When a response comes back with &#x60;next_cursor: null&#x60;, it means the endpoint is paginated but there are no more pages to request data from.  If the cursor is &#x60;non-null&#x60;, there are more objects to return, and a new request is necessary to get the next page.  The &#x60;next_cursor&#x60; contains an _opaque_ string that shouldn&#39;t be modified in any way. It should be sent **as-is** in the &#x60;cursor&#x60; parameter along with the same parameters used in the previous request.  ## Integrations  Integrations can be created and updated [here](https://app.todoist.com/app/settings/integrations/app-management).  Once done, they can also be submitted for evaluation and inclusion in [our official integrations list](https://www.todoist.com/integrations). This not only serves as an opportunity to market your integration to our audience, but will also serve as a way to help users get set up and familiar with your app quickly.  To get your integration evaluated, please submit it via [this page](https://doist.typeform.com/to/Vvq7kNcl?typeform-source&#x3D;todoist.com/api/v1/docs).  Lost? Reach out to us at [submissions@doist.com](mailto:submissions@doist.com) anytime. 

## Build
This C# SDK is automatically generated by the [OpenAPI Generator](https://openapi-generator.tech) project.

- SDK version: 1.0.0
- Generator version: 7.14.0
- Build package: org.openapitools.codegen.languages.CSharpClientCodegen
