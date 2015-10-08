using System;

namespace DistributedStateEngine
{
    internal class ServerState
    {
        private readonly Guid _id;
        private long _term;
        private readonly Guid? _votedFor;
        private readonly LogCollection _logs;

        internal ServerState(
            Guid id,
            long term,
            Guid? votedFor,
            LogCollection logs)
        {
            _id = id;
            _term = term;
            _votedFor = votedFor;
            _logs = logs;
        }

        internal static ServerState Load()
        {
            // todo load from persistance
            return new ServerState(Guid.NewGuid(), 0, null, new LogCollection());
        }

        public Guid Id
        {
            get { return _id; }
        }

        public long Term
        {
            get { return _term; }
        }

        public Guid? VotedFor
        {
            get { return _votedFor; }
        }

        public LogCollection Logs
        {
            get { return _logs; }
        }

        public void IncrementTerm()
        {
            _term++;
            PersistState();
        }

        private void PersistState()
        {
            // todo persist the state
        }
    }
}