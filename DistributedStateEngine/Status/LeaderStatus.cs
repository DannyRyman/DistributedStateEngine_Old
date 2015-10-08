namespace DistributedStateEngine.Status
{
    internal class LeaderStatus : Status
    {
        public LeaderStatus(Server server) : base(server)
        {
        }

        internal override StatusType StatusType
        {
            get { return StatusType.Leader; }
        }

        public override void EnterState()
        {
            
        }

        protected override void ElectionTimeout()
        {
            
        }
    }
}
