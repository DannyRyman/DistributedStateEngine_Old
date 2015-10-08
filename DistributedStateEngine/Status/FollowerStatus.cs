using YourRootNamespace.Logging;

namespace DistributedStateEngine.Status
{
    internal class FollowerStatus : Status
    {
        public FollowerStatus(Server server) : base(server)
        {
        }

        internal override StatusType StatusType
        {
            get { return StatusType.Follower; }
        }

        public override void EnterState()
        {
            ResetElectionTimeout();
        }

        protected override void ElectionTimeout()
        {
            Logger.Debug("Election timeout initiated.");
            
            Server.ChangeStatus(StatusType.Candidate);

            Logger.Debug("Election timeout complete.");
        }
    }
}
