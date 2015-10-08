using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using NetMQ;

namespace DistributedStateEngine
{
    public class NetMqRpcChannel : RpcChannel
    {
        public NetMqRpcChannel(
            ClusterConfiguration clusterConfiguration, TimeSpan maximumWaitTime)
            : base(clusterConfiguration, maximumWaitTime)
        {
        }

        public override void Broadcast<TRequestType, TResponseType>(TRequestType request, Action<TResponseType> onResponseReceived)
        {
            foreach (var node in ClusterConfiguration.Nodes)
            {
                Task.Factory.StartNew(() => Unicast<TRequestType, TResponseType>(node.Id, request))
                    .ContinueWith(x =>
                    {
                        if (x.Result != null)
                        {
                            onResponseReceived(x.Result);
                        }
                    });
            }
        }

        public override TResponseType Unicast<TRequestType, TResponseType>(Guid destinationServerId, TRequestType request)
        {
            var node = ClusterConfiguration.Nodes.SingleOrDefault(x => x.Id == destinationServerId);
            TResponseType response = null;

            if (node == null)
            {
                throw new InvalidOperationException(String.Format("Node not found: {0}", destinationServerId));
            }

            using (var context = NetMQContext.Create())
            {
                using (var requester = context.CreateRequestSocket())
                {
                    requester.Connect(node.Address);
                    requester.Options.Linger = TimeSpan.Zero;
                    requester.ReceiveReady += (sender, args) =>
                    {
                        var responseString = requester.ReceiveString();
                        response = (TResponseType) JsonConvert.DeserializeObject(responseString, typeof(TResponseType));
                    };

                    requester.Send(JsonConvert.SerializeObject(request));
                    requester.Poll(this.MaximumWaitTime);
                }    
            }
            
            return response;
        }
    }
}