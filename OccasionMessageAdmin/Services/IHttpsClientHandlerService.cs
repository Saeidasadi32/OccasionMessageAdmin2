namespace OccasionMessageAdmin.Services;

public interface IHttpsClientHandlerService
	{
    HttpMessageHandler GetPlatformMessageHandler();
}

