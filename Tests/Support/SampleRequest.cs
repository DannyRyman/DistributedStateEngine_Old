namespace Tests.Support
{
    public class SampleRequest
    {
        public SampleRequest(string requestString)
        {
            RequestString = requestString;
        }

        public string RequestString { get; private set; }

        protected bool Equals(SampleRequest other)
        {
            return string.Equals(RequestString, other.RequestString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SampleRequest) obj);
        }

        public override int GetHashCode()
        {
            return (RequestString != null ? RequestString.GetHashCode() : 0);
        }
    }
}