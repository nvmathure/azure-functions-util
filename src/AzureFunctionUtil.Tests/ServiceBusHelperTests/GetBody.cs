using CloudNDevOps.AzureFunctionUtil.Tests.TestArtifiacts;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CloudNDevOps.AzureFunctionUtil.Tests
{
    [TestClass]
    public class GetBody
    {
        [TestMethod]
        public void GetBodyWithNullMessage()
        {
            // Arrange 
            var action = new Action(() => ServiceBusHelper.GetBody<Payload>(null, null, null));

            // Act & Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("message");
        }

        [TestMethod]
        public void GetBodyWithNullEncodingFunction()
        {
            // Arrange 
            var message = new Message();
            var action = new Action(() => ServiceBusHelper.GetBody<Payload>(message, null, null));

            // Act & Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("encodingFunction");
        }
    }
}
