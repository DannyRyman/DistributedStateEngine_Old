using System;
using DistributedStateEngine.Entities;
using DistributedStateEngine.Status;
using YourRootNamespace.Logging;

namespace DistributedStateEngine
{
    public class Server
    {
        private Status.Status _currentStatus;
        private readonly StatusFactory _statusFactory;
        private ServerState _serverState;
        private static readonly ILog Logger = LogProvider.For<Server>();
        private RpcChannel _rpcChannel;

        public Server(ClusterConfiguration clusterConfiguration)
        {
            _currentStatus = new UninitializedStatus(this);
            _statusFactory = new StatusFactory();
            // todo avoid hard-coded timespan
            _rpcChannel = new NetMqRpcChannel(clusterConfiguration, TimeSpan.FromMilliseconds(200));
        }

        internal long CurrentTerm {
            get
            {
                EnsureInitialized();
                return _serverState.Term;
            }
        }

        public Guid Id {
            get
            {
                EnsureInitialized();
                return _serverState.Id;
            }
        }

        public LogPosition LastLogPosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private void EnsureInitialized()
        {
            if (_currentStatus.StatusType == StatusType.Uninitialized)
            {
                throw new InvalidOperationException(
                    "Attempted to perform an operation that is not valid on an uninitialized server.  Make sure you call Start.");
            }
        }

        public void Start()
        {
            Logger.Debug("Starting server.");

            // todo load from persistant store
            _serverState = ServerState.Load();

            ChangeStatus(StatusType.Follower);
            
            Logger.Debug("Finished starting server.");
        }

        internal void ChangeStatus(StatusType statusType)
        {
            Logger.Debug(String.Format("Server attempting to transition from initial state: {0} to {1}", _currentStatus.StatusType, statusType));

            _currentStatus.Dispose();
            _currentStatus = _statusFactory.Create(statusType, this);
            _currentStatus.EnterState();

            Logger.Debug(String.Format("Server transitioned to state {0}", statusType));
        }

        public void IncrementTerm()
        {
            _serverState.IncrementTerm();
        }

        public void Broadcast(RequestVote requestVote)
        {
            
        }
    }
}
