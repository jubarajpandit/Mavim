using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.Test.v1
{
    public class TopicServiceTest
    {
        private readonly Guid _databaseId = new Guid("d9e80be6-c4e0-417a-bb75-62994669dcb0");

        [Fact]
        [Trait("Category", "TopicService")]
        public async Task GetRootTopic_ValidArguments_Topic()
        {
            //Arrange
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            var topicMock = new Mock<IBusiness.ITopic>();
            topicMock.SetupAllProperties();
            topicMock.Object.Resource = new List<IBusiness.enums.TopicResource> {
                    IBusiness.enums.TopicResource.Chart,
                    IBusiness.enums.TopicResource.Description,
                    IBusiness.enums.TopicResource.Fields,
                    IBusiness.enums.TopicResource.Relations,
                    IBusiness.enums.TopicResource.SubTopics
                };

            topicBusinessMock.Setup(x => x.GetRootTopic()).ReturnsAsync(topicMock.Object);


            Mock<ILogger<TopicService>> loggerMock = new Mock<ILogger<TopicService>>();
            var fieldServiceMock = new Mock<IFieldService>();
            var service = new TopicService(topicBusinessMock.Object, loggerMock.Object);

            //Act
            ITopic result = await service.GetRootTopic(_databaseId);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITopic>(result);
            Assert.Contains(TopicResource.SubTopics, result.Resources);
            Assert.Contains(TopicResource.Fields, result.Resources);
            Assert.Contains(TopicResource.Chart, result.Resources);
            Assert.Contains(TopicResource.Relations, result.Resources);
            Assert.Contains(TopicResource.Description, result.Resources);
        }

        [Theory, MemberData(nameof(DcvIdValues))]
        [Trait("Category", "TopicService")]
        public async Task GetPathToRoot_ValidArguments_TopicPath(string dcvId)
        {
            //Arrange
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            var topicMock = new Mock<IBusiness.ITopic>();
            topicMock.Setup(p => p.HasChildren).Returns(true);
            topicBusinessMock.Setup(x => x.GetRootTopic()).ReturnsAsync(topicMock.Object);
            Mock<ILogger<TopicService>> loggerMock = new Mock<ILogger<TopicService>>();

            var topicPathMock = new Mock<IBusiness.ITopicPath>();
            topicPathMock.Setup(p => p.Data).Returns(new List<IBusiness.ITopic>());
            topicPathMock.Setup(p => p.Path).Returns(new List<IBusiness.IPathItem>());

            topicBusinessMock.Setup(business => business.GetPathToRoot(dcvId)).ReturnsAsync(topicPathMock.Object);
            var service = new TopicService(topicBusinessMock.Object, loggerMock.Object);

            //Act
            var result = await service.GetPathToRoot(dcvId);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITopicPath>(result);
        }

        [Theory, MemberData(nameof(InvalidDcvIdValues))]
        [Trait("Category", "TopicService")]
        public async Task GetPathToRoot_InvalidArguments_BadRequestException(string dcvId)
        {
            //Arrange
            Mock<IBusiness.ITopicBusiness> topicBusinessMock = new Mock<IBusiness.ITopicBusiness>();
            var topicMock = new Mock<IBusiness.ITopic>();
            topicMock.Setup(p => p.HasChildren).Returns(true);
            topicBusinessMock.Setup(x => x.GetRootTopic()).ReturnsAsync(topicMock.Object);
            Mock<ILogger<TopicService>> loggerMock = new Mock<ILogger<TopicService>>();
            var business = new TopicService(topicBusinessMock.Object, loggerMock.Object);
            var topicPathMock = new Mock<IBusiness.ITopicPath>();
            topicBusinessMock.Setup(repository => repository.GetPathToRoot(dcvId)).ReturnsAsync(topicPathMock.Object);

            //Act
            var result = await Record.ExceptionAsync(async () => await business.GetPathToRoot(dcvId));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestException>(result);
        }

        public static IEnumerable<object[]> DcvIdValues
        {
            get
            {
                yield return new object[] { "d0c2v0" };
                yield return new object[] { "d12950883c414v0" };
            }
        }

        public static IEnumerable<object[]> InvalidDcvIdValues
        {
            get
            {
                yield return new object[] { "test" };
                yield return new object[] { "d12950abc883c414v0" };
            }
        }
    }
}
