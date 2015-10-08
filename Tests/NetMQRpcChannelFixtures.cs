using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DistributedStateEngine;
using NUnit.Framework;
using Tests.Support;

namespace Tests
{
    [TestFixture]
    public class NetMqRpcChannelFixtures
    {
        [Test]
        public void ShouldBeAbleToUnicastAMessageAndReceiveAResponse()
        {
            var sampleClusterConfiguration = BuildSampleClusterConfiguration();

            // Start a test server to listen for requests
            SampleResponse serverResponse = new SampleResponse("test response");
            var testServer = new TestServer(sampleClusterConfiguration.Nodes[0], serverResponse);
            testServer.Start();

            var sut = new NetMqRpcChannel(sampleClusterConfiguration, TimeSpan.FromMilliseconds(200));

            var sampleRequest = new SampleRequest("test");

            var responseReturned = sut.Unicast<SampleRequest, SampleResponse>(sampleClusterConfiguration.Nodes[0].Id, sampleRequest);

            Assert.IsNotNull(responseReturned);
            Assert.AreEqual(serverResponse, responseReturned);
            Assert.AreEqual(sampleRequest, testServer.GetLastReceivedMessageObject<SampleRequest>());
        }

        [Test]
        public void UnicastShouldReturnNullIfServerDoesNotRespond()
        {
            var sampleClusterConfiguration = BuildSampleClusterConfiguration();

            var sut = new NetMqRpcChannel(sampleClusterConfiguration, TimeSpan.FromMilliseconds(200));

            var sampleRequest = new SampleRequest("test");

            var responseReturned = sut.Unicast<SampleRequest, SampleResponse>(sampleClusterConfiguration.Nodes[0].Id, sampleRequest);

            Assert.IsNull(responseReturned);
        }

        [Test]
        public void ShouldBeAbleToBroadcastMessagesAndReceiveResponses()
        {
            var sampleClusterConfiguration = BuildSampleClusterConfiguration();

            var testServer1 = SetupTestServerForNode(sampleClusterConfiguration.Nodes[0]);
            testServer1.Start();

            var testServer2 = SetupTestServerForNode(sampleClusterConfiguration.Nodes[1]);
            testServer2.Start();

            var sut = new NetMqRpcChannel(sampleClusterConfiguration, TimeSpan.FromMilliseconds(200));

            var sampleRequest = new SampleRequest("test");

            var autoResetEvent = new AutoResetEvent(false);

            int expectedNumberOfResponses = sampleClusterConfiguration.Nodes.Length;
            var responses = new List<SampleResponse>();

            sut.Broadcast<SampleRequest, SampleResponse>(sampleRequest, response =>
            {
                responses.Add(response);

                if (responses.Count >= expectedNumberOfResponses)
                {
                    autoResetEvent.Set();
                }
            });

            autoResetEvent.WaitOne(TimeSpan.FromSeconds(1));

            Assert.AreEqual(expectedNumberOfResponses, responses.Count);
            Assert.IsNotNull(responses.SingleOrDefault(x => x.ResponseString == sampleClusterConfiguration.Nodes[0].Id.ToString()));
            Assert.IsNotNull(responses.SingleOrDefault(x => x.ResponseString == sampleClusterConfiguration.Nodes[1].Id.ToString()));
        }

        [Test]
        public void ShouldBeAbleToReceiveResponseFromANodeWhenOtherNodesAreDown()
        {
            var sampleClusterConfiguration = BuildSampleClusterConfiguration();

            var testServer1 = SetupTestServerForNode(sampleClusterConfiguration.Nodes[0]);
            testServer1.Start();

            var sut = new NetMqRpcChannel(sampleClusterConfiguration, TimeSpan.FromMilliseconds(200));

            var sampleRequest = new SampleRequest("test");

            var autoResetEvent = new AutoResetEvent(false);

            int expectedNumberOfResponses = sampleClusterConfiguration.Nodes.Length;
            var responses = new List<SampleResponse>();

            sut.Broadcast<SampleRequest, SampleResponse>(sampleRequest, response =>
            {
                responses.Add(response);

                if (responses.Count >= expectedNumberOfResponses)
                {
                    autoResetEvent.Set();
                }
            });

            autoResetEvent.WaitOne(TimeSpan.FromSeconds(1));

            Assert.AreEqual(1, responses.Count);
            Assert.IsNotNull(responses.SingleOrDefault(x => x.ResponseString == sampleClusterConfiguration.Nodes[0].Id.ToString()));
        }

        private static TestServer SetupTestServerForNode(NodeAddress serverNode)
        {
            SampleResponse serverResponse = new SampleResponse(serverNode.Id.ToString());
            var testServer = new TestServer(serverNode, serverResponse);
            return testServer;
        }

        private ClusterConfiguration BuildSampleClusterConfiguration()
        {
            return new ClusterConfiguration(new []
                                                {
                                                    new NodeAddress(new Guid("B75D602B-9C62-4397-B993-321ECF46E7E6"), "tcp://127.0.0.1:5555"),
                                                    new NodeAddress(new Guid("10D03DEF-1614-4C46-AE55-7FC8B3A098A0"), "tcp://127.0.0.1:5556")
                                                });
        }
    }
}