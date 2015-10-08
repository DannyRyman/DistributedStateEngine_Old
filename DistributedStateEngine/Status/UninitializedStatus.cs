namespace DistributedStateEngine.Status
{
    internal class UninitializedStatus : Status
    {
        public UninitializedStatus(Server server) : base(server)
        {
        }

        internal override StatusType StatusType
        {
            get { return StatusType.Uninitialized; }
        }

        public override void EnterState()
        {            
        }

        protected override void ElectionTimeout()
        {
        }
    }
}
