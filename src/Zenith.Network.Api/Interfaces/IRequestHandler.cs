using Zenith.Network.Api;
using Zenith.Network.Api.Messages;

namespace Zenith.Network.Api
{
    public interface IRequestHandler
    {
        NetworkNodeResponseBase Execute(string requestChannelId, NetworkNodeRequestBase request);
    }

    public interface IHubRequestHandler
    {
        RequestHandlerResult Execute<T>(string callerId, T request) where T : class;
    }
}
