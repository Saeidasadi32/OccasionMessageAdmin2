using OccasionMessageAdmin.Shared.Interfaces;

namespace OccasionMessageAdmin.Shared.Services;

public class AuthHttpMessageHandler(ITokenStorageService tokenStorage, AuthClientService authClient) : DelegatingHandler
{
    private readonly ITokenStorageService _tokenStorage = tokenStorage;
    private readonly AuthClientService _authClient = authClient;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var (token, _) = await _tokenStorage.GetTokensAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // اگر 401 گرفت، ریفرش کن و مجدد تست
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var refreshResult = await _authClient.RefreshAsync();

            if (refreshResult?.IsSuccess == true)
            {
                // توکن جدید رو ست کن
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", refreshResult.Token);

                // درخواست مجدد
                response.Dispose(); // درخواست قبلی رو آزاد کن
                response = await base.SendAsync(request, cancellationToken);
            }
        }

        return response;
    }
}

