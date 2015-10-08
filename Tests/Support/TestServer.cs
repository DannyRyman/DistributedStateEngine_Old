using System.Threading.Tasks;
using DistributedStateEngine;
using NetMQ;
using Newtonsoft.Json;

namespace Tests.Support
{
    public class TestServer
    {
        private readonly NodeAddress _serverAddress;
        private readonly SampleResponse _serverResponse;
        private string _lastReceivedMessage;

        public TestServer(NodeAddress serverAddress, SampleResponse serverResponse)
        {
            _serverAddress = serverAddress;
            _serverResponse = serverResponse;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                using (var context = NetMQContext.Create())
                {
                    using (var responseSocket = context.CreateResponseSocket())
                    {
                        responseSocket.Bind(_serverAddress.Address);
                        _lastReceivedMessage = responseSocket.ReceiveString();
                        responseSocket.Send(JsonConvert.SerializeObject(_serverResponse));
                    }
                }
            });
        }

        public string GetLastReceivedMessageString()
        {
            return _lastReceivedMessage;
        }

        public T GetLastReceivedMessageObject<T>()
        {
            return (T) JsonConvert.DeserializeObject(_lastReceivedMessage, typeof (T));
        }
    }
}