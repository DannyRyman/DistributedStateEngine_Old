namespace DistributedStateEngine.Entities
{
    public class LogPosition
    {
        private readonly long _index;
        private readonly long _term;

        public LogPosition(long index, long term)
        {
            _index = index;
            _term = term;
        }

        public long Index
        {
            get { return _index; }
        }

        public long Term
        {
            get { return _term; }
        }
    }
}