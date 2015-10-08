using System;

namespace DistributedStateEngine.Entities
{
    public class RequestVote
    {
        private readonly long _term;
        private readonly Guid _serverId;
        private readonly LogPosition _logPosition;

        private RequestVote(
            long term,
            Guid serverId,
            LogPosition logPosition
            )
        {
            _term = term;
            _serverId = serverId;
            _logPosition = logPosition;
        }

        public long Term
        {
            get { return _term; }
        }

        public Guid ServerId
        {
            get { return _serverId; }
        }

        public LogPosition LogPosition
        {
            get { return _logPosition; }
        }

        public static RequestVote Create(Server server)
        {
            return new RequestVote(server.CurrentTerm, server.Id, server.LastLogPosition);
        }
    }
}
