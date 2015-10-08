namespace DistributedStateEngine
{
    using System.Linq;

    public class ClusterConfiguration
    {
        public NodeAddress[] Nodes { get; private set; }

        public ClusterConfiguration(NodeAddress[] nodes)
        {
            this.Nodes = nodes;
        }

        public int QuorumSize
        {
            get
            {
                return this.Nodes.Count()/2;
            }
        }
    }
}