using DistributedStateEngine.Entities;

namespace DistributedStateEngine.Status
{
    internal class CandidateStatus : Status
    {
        private int _votesCounted;

        public CandidateStatus(Server server) : base(server)
        {
        }

        internal override StatusType StatusType
        {
            get { return StatusType.Candidate; }
        }

        public override void EnterState()
        {
            StartElection();
        }

        private void StartElection()
        {
            ResetVotesCount();
            IncrementTerm();
            ResetElectionTimeout();
            VoteForSelf();
            RequestVotesFromOtherServers();
        }

        private void RequestVotesFromOtherServers()
        {
            this.Server.Broadcast(RequestVote.Create(this.Server));
        }

        private void VoteForSelf()
        {
            _votesCounted++;
        }

        private void ResetVotesCount()
        {
            _votesCounted = 0;
        }

        protected override void ElectionTimeout()
        {
            
        }
    }
}
