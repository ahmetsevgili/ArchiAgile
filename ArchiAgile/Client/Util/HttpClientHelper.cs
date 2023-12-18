using ArchiAgile.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ArchiAgile.Client.Util
{
    public class HttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _iJSRuntime;
        private readonly ModelValidator _modelValidator;
        private readonly NavigationManager _navigationManager;
        private readonly NotificationService _notificationService;
        public HttpClientHelper(HttpClient httpClient,
                                  IJSRuntime iJSRuntime,
                                  ModelValidator modelValidator,
                                  NavigationManager navigationManager,
                                  NotificationService notificationService)
        {
            _httpClient = httpClient;
            _iJSRuntime = iJSRuntime;
            _modelValidator = modelValidator;
            _navigationManager = navigationManager;
            _notificationService = notificationService;
        }
        private void ShowNotification(NotificationMessage message)
        {
            _notificationService.Notify(message);
        }
        public async Task<HttpClientHelperResult<TResponse>> Post<TResponse>(string requestUri)
        {
            return await Post<object, TResponse>(requestUri, "", true, true, true);
        }

        public async Task<HttpClientHelperResult<TResponse>> Post<TResponse>(string requestUri, bool showSuccessMessage)
        {
            return await Post<object, TResponse>(requestUri, "", true, showSuccessMessage, true);
        }

        public async Task<HttpClientHelperResult<TResponse>> Post<TRequest, TResponse>(string requestUri, TRequest request, bool showSuccessMessage)
        {
            return await Post<TRequest, TResponse>(requestUri, request, true, showSuccessMessage, true);
        }
        public async Task<HttpClientHelperResult<TResponse>> Post<TRequest, TResponse>(string requestUri, TRequest request)
        {
            return await Post<TRequest, TResponse>(requestUri, request, true, true, true);
        }

        public async Task<HttpClientHelperResult<TResponse>> Post<TResponse>(string requestUri, bool validate, bool showSuccessMessage, bool showErrorMessage)
        {
            return await Post<object, TResponse>(requestUri, "", validate, showSuccessMessage, showErrorMessage);
        }

        public async Task<HttpClientHelperResult<TResponse>> Post<TRequest, TResponse>(string requestUri, TRequest request, bool validate, bool showSuccessMessage, bool showErrorMessage)
        {
            if (validate)
            {
                List<ValidationResult> errors = new List<ValidationResult>();
                var isValid = _modelValidator.ValidateRecursive(request, out errors);
                if (!isValid)
                {
                    foreach (var item in errors)
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error! ", Detail = item.ErrorMessage, Duration = 4000 });
                    }
                    return new HttpClientHelperResult<TResponse> { IsSuccess = false, };
                }
            }

            try
            {
                var result = await _httpClient.PostAsJsonAsync(requestUri, request);
                if (result.IsSuccessStatusCode)
                {
                    var response = System.Text.Json.JsonSerializer.Deserialize<TResponse>(result.Content.ReadAsStringAsync().Result);
                    if (response is BaseResponse baseResponse)
                    {
                        if (!string.IsNullOrWhiteSpace(baseResponse.ResponseMessage))
                        {
                            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Warning! ", Detail = baseResponse.ResponseMessage, Duration = 4000 });
                            return new HttpClientHelperResult<TResponse> { IsSuccess = false, ErrorCode = baseResponse.ResponseCode };
                        }
                        else if (showSuccessMessage)
                        {
                            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success! ", Detail = "Your process is completed successfuly", Duration = 4000 });
                        }
                    }
                    else if (showSuccessMessage)
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Success! ", Detail = "Your process is completed successfuly", Duration = 4000 });
                    }
                    return new HttpClientHelperResult<TResponse> { IsSuccess = true, Response = response };
                }
                else
                {
                    if (showErrorMessage)
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error! ", Detail = "There is a problem with your process. Contact your administrator.", Duration = 4000 });
                    }
                    return new HttpClientHelperResult<TResponse> { IsSuccess = false, };
                }
            }
            catch (Exception ex)
            {
                if (showErrorMessage)
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error! ", Detail = "There is a problem with your process. Contact your administrator..", Duration = 4000 });
                }
                return new HttpClientHelperResult<TResponse> { IsSuccess = false, };
            }
        }
    }
}
