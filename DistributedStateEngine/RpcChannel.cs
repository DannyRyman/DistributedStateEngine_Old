using System;

namespace DistributedStateEngine
{
    public abstract class RpcChannel
    {
        protected readonly ClusterConfiguration ClusterConfiguration;

        protected readonly TimeSpan MaximumWaitTime;

        protected RpcChannel(
            ClusterConfiguration clusterConfiguration,
            TimeSpan maximumWaitTime)
        {
            this.ClusterConfiguration = clusterConfiguration;
            this.MaximumWaitTime = maximumWaitTime;
        }

        public abstract void Broadcast<TRequestType, TResponseType>(TRequestType request, Action<TResponseType> onResponseReceived) where TResponseType : class;

        public abstract TResponseType Unicast<TRequestType, TResponseType>(Guid destinationServerId, TRequestType request) where TResponseType : class;
    }

    
}