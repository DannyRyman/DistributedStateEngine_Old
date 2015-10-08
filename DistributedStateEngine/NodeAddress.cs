namespace DistributedStateEngine
{
    using System;

    public class NodeAddress
    {
        private readonly Guid _id;
        private readonly string _address;

        public NodeAddress(Guid id, string address)
        {
            this._id = id;
            this._address = address;
        }

        public Guid Id
        {
            get { return this._id; }
        }

        public string Address
        {
            get { return this._address; }
        }
    }
}