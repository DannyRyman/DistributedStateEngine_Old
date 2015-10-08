namespace Tests.Support
{
    public class SampleResponse
    {
        public string ResponseString { get; private set; }

        public SampleResponse(string responseString)
        {
            ResponseString = responseString;
        }

        protected bool Equals(SampleResponse other)
        {
            return string.Equals(ResponseString, other.ResponseString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SampleResponse) obj);
        }

        public override int GetHashCode()
        {
            return (ResponseString != null ? ResponseString.GetHashCode() : 0);
        }
    }
}