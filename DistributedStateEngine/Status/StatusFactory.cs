using System;

namespace DistributedStateEngine.Status
{
    public class StatusFactory
    {
        internal DistributedStateEngine.Status.Status Create(StatusType statusType, Server context)
        {
            switch (statusType)
            {
                case StatusType.Follower:
                    return new FollowerStatus(context);
                case StatusType.Candidate:
                    return new CandidateStatus(context);
                case StatusType.Leader:
                    return new LeaderStatus(context);
            }

            throw new InvalidOperationException("Unexpected state.");
        }
    }
}
