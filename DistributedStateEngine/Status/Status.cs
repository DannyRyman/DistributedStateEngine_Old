using System;
using System.Threading;
using YourRootNamespace.Logging;

namespace DistributedStateEngine.Status
{
    internal abstract class Status : IDisposable
    {
        public long Term { get; set; }
        protected static readonly ILog Logger = LogProvider.For<Status>();
        protected readonly Server Server;
        private Timer _electionTimer;
        private const int _electionTimeoutMilliseconds = 2000;

        internal abstract StatusType StatusType { get; }

        protected Status(Server server)
        {
            Server = server;
        }

        public long CurrentTerm
        {
            get { return Server.CurrentTerm; }
        }

        public abstract void EnterState();

        protected void ResetElectionTimeout()
        {
            // todo get the electionTimeoutMilliseconds from configuration
            // todo randomise the election timeout
            _electionTimer = new Timer(ElectionTimeoutHandler, null, _electionTimeoutMilliseconds, Timeout.Infinite);
        }

        private void ElectionTimeoutHandler(object state)
        {
            ElectionTimeout();
        }

        protected abstract void ElectionTimeout();

        public virtual void Dispose()
        {
            if (_electionTimer != null)
            {
                _electionTimer.Dispose();
            }
        }

        public void IncrementTerm()
        {
            Server.IncrementTerm();
        }
    }
}
