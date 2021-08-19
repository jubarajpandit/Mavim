using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;
using ITopicLogic = Mavim.Manager.Api.Topic.Business.Interfaces.v1;
using TopicLogic = Mavim.Manager.Api.Topic.Business.v1.Models;
using ITopicService = Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using TopicService = Mavim.Manager.Api.Topic.Services.v1.Models;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Services.v1.Mappers;
using Moq;

namespace Mavim.Manager.Api.Topic.Services.Test.v1.Mappers
{
    public class TopicMapperTest
    {
        [Fact]
        [Trait("Category", "TopicMapper")]
        public void Map_ValidArguments_ValidServicesTopic()
        {
            //Arrange
            var businessTopic = this.BusinessTopic;
            var servicesTopic = this.ServiceTopic;

            //Act
            var result = TopicMapper.Map(businessTopic);

            //Assert
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(servicesTopic);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "TopicMapper")]
        public void Map_ValidArguments_ValidServicesTopicWithResources()
        {
            //Arrange
            var businessTopic = this.BusinessTopic;
            var servicesTopic = this.ServiceTopicWithResources;

            //Act
            var result = TopicMapper.MapTopicWithResource(businessTopic);

            //Assert
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(servicesTopic);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "TopicMapper")]
        public void Map_ValidArguments_ValidBusinessSaveTopic()
        {
            //Arrange
            var businessSaveTopic = new TopicLogic.SaveTopic() { Name = "test" };
            var serviceSaveTopic = new TopicService.SaveTopic() { Name = "test" };

            //Act
            var result = TopicMapper.MapSaveTopic(serviceSaveTopic);

            //Assert
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(businessSaveTopic);
            Assert.Equal(expected, resultstring);
        }

        [Fact]
        [Trait("Category", "TopicMapper")]
        public void Map_ValidArguments_ValidServicesTopicPath()
        {
            //Arrange
            var businessTopicPath = new TopicLogic.TopicPath() {
                Data = new List<ITopicLogic.ITopic>() { this.BusinessTopic },
                Path = new List<ITopicLogic.IPathItem> { new TopicLogic.PathItem() { DcvId = "DcvId", Order = 1 } }
            };

            var servicesTopicPath = new TopicService.TopicPath()
            {
                Data = new List<ITopicService.ITopic>() { this.ServiceTopic },
                Path = new List<ITopicService.IPathItem> { new TopicService.PathItem() { DcvId = "DcvId", Order = 1 } }
            };

            //Act
            var result = TopicMapper.MapTopicPath(businessTopicPath);

            //Assert
            var resultstring = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(servicesTopicPath);
            Assert.Equal(expected, resultstring);
        }

        private ITopicLogic.ITopic BusinessTopic { 
            get {
                return
                new TopicLogic.Topic()
                {
                    Dcv = "Dcv",
                    Parent = "Parent",
                    Name = "Name",
                    Code = "Code",
                    ElementType = ElementType.Virtual,
                    Icon = "Icon",
                    IsChart = true,
                    IsReadOnly = true,
                    OrderNumber = 1,
                    HasChildren = true,
                    CanDelete = true,
                    CanCreateChildTopic = true,
                    CanCreateTopicAfter = true,
                    IsInRecycleBin = true,
                    Resource = new List<ITopicLogic.enums.TopicResource>() {
                        ITopicLogic.enums.TopicResource.Chart,
                        ITopicLogic.enums.TopicResource.Description,
                        ITopicLogic.enums.TopicResource.Fields,
                        ITopicLogic.enums.TopicResource.Relations,
                        ITopicLogic.enums.TopicResource.SubTopics
                    }
                };
            }
        }

        private ITopicService.ITopic ServiceTopic
        {
            get
            {
                return
                new TopicService.Topic()
                {
                    Dcv = "Dcv",
                    Parent = "Parent",
                    HasChildren = true,
                    Name = "Name",
                    Code = "Code",
                    Icon = "Icon",
                    OrderNumber = 1,
                    TypeCategory = TopicType.Virtual,
                    IsInRecycleBin = true,
                    Business = new TopicService.TopicBusiness()
                    {
                        IsReadOnly = true,
                        CanDelete = true,
                        CanCreateChildTopic = true,
                        CanCreateTopicAfter = true
                    }
                };
            }
        }

        private ITopicService.ITopic ServiceTopicWithResources
        {
            get
            {
                return
                new TopicService.Topic()
                {
                    Dcv = "Dcv",
                    Parent = "Parent",
                    HasChildren = true,
                    Name = "Name",
                    Code = "Code",
                    Icon = "Icon",
                    OrderNumber = 1,
                    TypeCategory = TopicType.Virtual,
                    IsInRecycleBin = true,
                    Business = new TopicService.TopicBusiness()
                    {
                        IsReadOnly = true,
                        CanDelete = true,
                        CanCreateChildTopic = true,
                        CanCreateTopicAfter = true
                    },
                    Resources = new List<ITopicService.enums.TopicResource>() {
                        ITopicService.enums.TopicResource.Chart,
                        ITopicService.enums.TopicResource.Description,
                        ITopicService.enums.TopicResource.Fields,
                        ITopicService.enums.TopicResource.Relations,
                        ITopicService.enums.TopicResource.SubTopics
                    }
                };
            }
        }
    }
}
