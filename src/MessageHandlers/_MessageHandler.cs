namespace Microsoft.Azure.SpaceFx.HostServices.Position;

public partial class MessageHandler<T> : Microsoft.Azure.SpaceFx.Core.IMessageHandler<T> where T : notnull {
    private readonly ILogger<MessageHandler<T>> _logger;
    private readonly Utils.PluginDelegates _pluginDelegates;
    private readonly Microsoft.Azure.SpaceFx.Core.Services.PluginLoader _pluginLoader;
    private readonly IServiceProvider _serviceProvider;
    private readonly Core.Client _client;
    private readonly Models.APP_CONFIG _appConfig;

    public MessageHandler(ILogger<MessageHandler<T>> logger, Utils.PluginDelegates pluginDelegates, Microsoft.Azure.SpaceFx.Core.Services.PluginLoader pluginLoader, IServiceProvider serviceProvider, Core.Client client) {
        _logger = logger;
        _pluginDelegates = pluginDelegates;
        _pluginLoader = pluginLoader;
        _serviceProvider = serviceProvider;
        _appConfig = new Models.APP_CONFIG();
        _client = client;
    }

    public void MessageReceived(T message, MessageFormats.Common.DirectToApp fullMessage) {
        using (var scope = _serviceProvider.CreateScope()) {

            if (message == null || EqualityComparer<T>.Default.Equals(message, default)) {
                _logger.LogInformation("Received empty message '{messageType}' from '{appId}'.  Discarding message.", typeof(T).Name, fullMessage.SourceAppId);
                return;
            }

            switch (typeof(T).Name) {
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Position.PositionRequest).Name, StringComparison.CurrentCultureIgnoreCase):
                    PositionRequestHandler(message: message as MessageFormats.HostServices.Position.PositionRequest, fullMessage: fullMessage);
                    break;
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Position.PositionUpdateRequest).Name, StringComparison.CurrentCultureIgnoreCase):
                    PositionUpdateRequestHandler(message: message as MessageFormats.HostServices.Position.PositionUpdateRequest, fullMessage: fullMessage);
                    break;
            }
        }
    }
}
