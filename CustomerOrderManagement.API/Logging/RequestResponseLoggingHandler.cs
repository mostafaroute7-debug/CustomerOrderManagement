using CustomerOrderManagement.Application.Exceptions;
using Serilog;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrderManagement.API.Logging
{
    public class RequestResponseLoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var method = request.Method.Method;
            var endpoint = request.RequestUri?.PathAndQuery;

            var requestBody = string.Empty;

            if (request.Content != null)
            {
                requestBody = await request.Content.ReadAsStringAsync();
            }

            var username = RequestContextUser(request);

            Log.Information(
                "HTTP Request | Method: {Method} | Endpoint: {Endpoint} | User: {User} | Request: {Request}",
                method,
                endpoint,
                username,
                requestBody);

            try
            {
                var response = await base.SendAsync(
                    request,
                    cancellationToken);

                stopwatch.Stop();

                var responseBody = string.Empty;

                if (response.Content != null)
                {
                    responseBody =
                        await response.Content.ReadAsStringAsync();
                }

                var statusCode = (int)response.StatusCode;

                if (statusCode >= 500)
                {
                    Log.Error(
                        "HTTP Response | Method: {Method} | Endpoint: {Endpoint} | StatusCode: {StatusCode} | Duration: {Duration}ms | User: {User} | Response: {Response}",
                        method,
                        endpoint,
                        statusCode,
                        stopwatch.ElapsedMilliseconds,
                        username,
                        responseBody);
                }
                else if (statusCode >= 400)
                {
                    Log.Warning(
                        "HTTP Response | Method: {Method} | Endpoint: {Endpoint} | StatusCode: {StatusCode} | Duration: {Duration}ms | User: {User} | Response: {Response}",
                        method,
                        endpoint,
                        statusCode,
                        stopwatch.ElapsedMilliseconds,
                        username,
                        responseBody);
                }
                else
                {
                    Log.Information(
                        "HTTP Response | Method: {Method} | Endpoint: {Endpoint} | StatusCode: {StatusCode} | Duration: {Duration}ms | User: {User} | Response: {Response}",
                        method,
                        endpoint,
                        statusCode,
                        stopwatch.ElapsedMilliseconds,
                        username,
                        responseBody);
                }

                return response;
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                if (IsWarningException(exception))
                {
                    Log.Warning(
                        exception,
                        "HTTP Exception | Method: {Method} | Endpoint: {Endpoint} | Duration: {Duration}ms | User: {User}",
                        method,
                        endpoint,
                        stopwatch.ElapsedMilliseconds,
                        username);
                }
                else
                {
                    Log.Error(
                        exception,
                        "HTTP Exception | Method: {Method} | Endpoint: {Endpoint} | Duration: {Duration}ms | User: {User}",
                        method,
                        endpoint,
                        stopwatch.ElapsedMilliseconds,
                        username);
                }

                throw;
            }
        }

        private bool IsWarningException(Exception exception)
        {
            return exception is NotFoundException
                || exception is UnauthorizedException
                || exception is BusinessException
                || exception is ValidationException;
        }

        private string RequestContextUser(
            HttpRequestMessage request)
        {
            var principal =
                request.GetRequestContext()
                    ?.Principal;

            return principal?.Identity?.IsAuthenticated == true
                ? principal.Identity.Name
                : "Anonymous";
        }
    }
}