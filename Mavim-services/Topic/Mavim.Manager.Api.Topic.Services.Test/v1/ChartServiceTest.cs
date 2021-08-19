using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Api.Topic.Business.v1.Models;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.v1;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.Test.v1
{
    public class ChartServiceTest
    {
        [Theory, MemberData(nameof(DcvIdAndChartValues))]
        [Trait("Category", "ChartService")]
        public async Task GetTopicCharts_ValidArguments_Charts(string dcvId, IEnumerable<IBusiness.IChart> requested, IEnumerable<IChart> expected)
        {
            //Arrange
            var chartBusinessMock = new Mock<IBusiness.IChartBusiness>();
            var loggerMock = new Mock<ILogger<TopicService>>();
            var business = new ChartService(chartBusinessMock.Object, loggerMock.Object);
            chartBusinessMock.Setup(x => x.GetTopicCharts(It.IsAny<string>())).ReturnsAsync(requested);

            //Act
            var result = await business.GetTopicCharts(dcvId);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IChart>>(result);
            var resultString = JsonConvert.SerializeObject(result);
            var expectedString = JsonConvert.SerializeObject(expected);
            Assert.Equal(expectedString, resultString);
        }

        [Theory, MemberData(nameof(InvalidDcvIdValues))]
        [Trait("Category", "RelationshipService")]
        public async Task GetTopicCharts_InvalidArguments_BadRequestException(string dcvId)
        {
            //Arrange
            var chartBusinessMock = new Mock<IBusiness.IChartBusiness>();
            var loggerMock = new Mock<ILogger<TopicService>>();
            var business = new ChartService(chartBusinessMock.Object, loggerMock.Object);

            //Act
            var result = await Record.ExceptionAsync(async () => await business.GetTopicCharts(dcvId));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestException>(result);
        }

        public static IEnumerable<object[]> DcvIdAndChartValues
        {
            get
            {
                yield return new object[] { "d0c2v0",
                    new List<IBusiness.IChart> { new Chart() {
                            Dcv = "d12950883c414v0",
                            Name = "TestChart"
                        }
                    },
                     new List<IChart> {
                            new Services.v1.Models.Chart() {
                                Dcv = "d12950883c414v0",
                                Name = "TestChart"
                            }
                    }
                };
                yield return new object[] { "d12950883c414v0",
                    new List<IBusiness.IChart> { new Chart() {
                            Dcv = "d0c2v0",
                            Name = "TestChart2"
                        }
                    },
                     new List<IChart> {
                            new Services.v1.Models.Chart() {
                                Dcv = "d0c2v0",
                                Name = "TestChart2"
                            }
                    }
                };
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
