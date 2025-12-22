using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ExampleCS;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var systemInfo = $"AwsRequestId: {context.AwsRequestId} | TenantId: {context.TenantId}";
        context.Logger.LogInformation(systemInfo);

        string? input = null;
        request.QueryStringParameters?.TryGetValue("input", out input);
        if (string.IsNullOrWhiteSpace(input))
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = "Query string parameter 'input' is required.",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var upperValue = input.ToUpperInvariant();
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = upperValue,
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };
    }
}
