using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.enums;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.v1;
using Mavim.Manager.Model;
using Mavim.Manager.Resources;
using Mavim.Manager.Server;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Api.Topic.Repository.Test.v1
{
    public class RelationshipRepositoryTest
    {
        private const string BASE_ELEMENT_DCVID = "d0c0v0";
        private const string FROM_ELEMENT_DCVID = "d0c1v0";
        private const string TO_ELEMENT_DCVID = "d0c2v0";
        private const string UNKNOWN_ELEMENT_DCVID = "d0c3v0";
        private const string WRONG_ELEMENT_DCVID = "5c5cb999-819c-4604-9a4b-38937e280995";
        private const string RELATION_DCVID = "d0c4v0";
        private const string RELATION2_DCVID = "d0c5v0";

        [Fact]
        [Trait("Category", "RelationshipRepository")]
        public async Task GetRelationships_TopicId_IRelationshipList()
        {
            IDcvId baseElementDcv = DcvId.FromDcvKey(BASE_ELEMENT_DCVID);
            IDcvId fromElementDcv = DcvId.FromDcvKey(FROM_ELEMENT_DCVID);
            IDcvId toElementDcv = DcvId.FromDcvKey(TO_ELEMENT_DCVID);
            IDcvId relationDcv = DcvId.FromDcvKey(RELATION_DCVID);
            IDcvId relation2Dcv = DcvId.FromDcvKey(RELATION2_DCVID);

            Mock<IElement> fromElementMock = GetMockElement(fromElementDcv, "from", TreeIconID.MiscellaneousIconID_MvMin);
            Mock<IElement> toElementMock = GetMockElement(toElementDcv, "to", TreeIconID.MiscellaneousIconID_MvPlus);
            Mock<IRelation> relation1 = GetMockRelation(relationDcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Where);
            Mock<IRelation> relation2 = GetMockRelation(relationDcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Unknown, true);
            Mock<IRelation> relation3 = GetMockRelation(relation2Dcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_HypTo, false, true);

            List<IRelation> relationsMock = new List<IRelation>();
            relationsMock.Add(relation1.Object);
            relationsMock.Add(relation2.Object);
            relationsMock.Add(relation3.Object);

            Mock<ILogger<RelationshipRepository>> loggerMock = GetMockLogger();

            Mock<IElement> baseElementMock = GetMockElement(baseElementDcv);
            baseElementMock = AddRelationsToMockElement(baseElementMock, relationsMock);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(baseElementMock);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);

            Mock<IDataLanguage> dataLanguageMock = GetDataLanguageMock();
            RelationshipRepository relationshipRepository = new RelationshipRepository(dataAccessMock.Object, loggerMock.Object, dataLanguageMock.Object);
            Mock<IRelationship> relationship = GetRelationshipMock();

            Func<IRelationship, IRelation, bool> validate = (repoRelation, mavimRelation) =>
            {
                Assert.Equal(mavimRelation.DcvId.ToString(), repoRelation.Dcv);
                Assert.Equal((int)repoRelation.RelationshipType, (int)mavimRelation.Type.Type);
                Assert.Equal(repoRelation.IsAttributeRelation, mavimRelation.IsAttributeRelation);
                Assert.Equal(repoRelation.WithElement.Dcv, mavimRelation.ToElement.DcvID.ToString());
                Assert.Equal(repoRelation.WithElement.Name, mavimRelation.ToElement.Name?.ToString());
                Assert.Equal(repoRelation.WithElement.Icon, mavimRelation.ToElement.Visual?.IconResourceID.ToString("G"));
                Assert.Equal(repoRelation.IsDestroyed, mavimRelation.IsDestroyed);
                return true;
            };

            //Act
            IEnumerable<IRelationship> relations = await relationshipRepository.GetRelationships(BASE_ELEMENT_DCVID);

            //Assert
            Assert.NotNull(relations);
            Assert.NotEmpty(relations);
            Assert.Collection(relations, rel => validate(rel, relation1.Object), rel => validate(rel, relation2.Object), rel => validate(rel, relation3.Object));
        }

        [Fact]
        [Trait("Category", "RelationshipRepository")]
        public async Task GetRelationships_UnknownDcvId_IRelationship()
        {
            IDcvId baseElementDcv = DcvId.FromDcvKey(BASE_ELEMENT_DCVID);
            IDcvId fromElementDcv = DcvId.FromDcvKey(FROM_ELEMENT_DCVID);
            IDcvId toElementDcv = DcvId.FromDcvKey(TO_ELEMENT_DCVID);
            IDcvId relationDcv = DcvId.FromDcvKey(RELATION_DCVID);
            IDcvId relation2Dcv = DcvId.FromDcvKey(RELATION2_DCVID);

            Mock<IElement> fromElementMock = GetMockElement(fromElementDcv);
            Mock<IElement> toElementMock = GetMockElement(toElementDcv);
            Mock<IRelation> relation1 = GetMockRelation(relationDcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Unknown);
            Mock<IRelation> relation2 = GetMockRelation(relation2Dcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_HypTo);


            List<IRelation> relationsMock = new List<IRelation>();
            relationsMock.Add(relation1.Object);
            relationsMock.Add(relation2.Object);

            Mock<ILogger<RelationshipRepository>> loggerMock = GetMockLogger();

            Mock<IElement> baseElementMock = GetMockElement(baseElementDcv);
            baseElementMock = AddRelationsToMockElement(baseElementMock, relationsMock);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(baseElementMock);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);

            Mock<IDataLanguage> dataLanguageMock = GetDataLanguageMock();
            RelationshipRepository relationshipRepository = new RelationshipRepository(dataAccessMock.Object, loggerMock.Object, dataLanguageMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await relationshipRepository.GetRelationships(UNKNOWN_ELEMENT_DCVID));

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions.RequestNotFoundException>(result);
        }

        [Theory, MemberData(nameof(InvalidAgumentDcvIdValues))]
        [Trait("Category", "RelationshipRepository")]
        public async Task GetRelationships_InvalidArgumentDcvIdValue_IRelationship(string dcvId)
        {
            IDcvId baseElementDcv = DcvId.FromDcvKey(BASE_ELEMENT_DCVID);
            IDcvId fromElementDcv = DcvId.FromDcvKey(FROM_ELEMENT_DCVID);
            IDcvId toElementDcv = DcvId.FromDcvKey(TO_ELEMENT_DCVID);
            IDcvId relationDcv = DcvId.FromDcvKey(RELATION_DCVID);
            IDcvId relation2Dcv = DcvId.FromDcvKey(RELATION2_DCVID);

            Mock<IElement> fromElementMock = GetMockElement(fromElementDcv);
            Mock<IElement> toElementMock = GetMockElement(toElementDcv);
            Mock<IRelation> relation1 = GetMockRelation(relationDcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Unknown);
            Mock<IRelation> relation2 = GetMockRelation(relation2Dcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_HypTo);

            List<IRelation> relationsMock = new List<IRelation>();
            relationsMock.Add(relation1.Object);
            relationsMock.Add(relation2.Object);

            Mock<ILogger<RelationshipRepository>> loggerMock = GetMockLogger();

            Mock<IElement> baseElementMock = GetMockElement(baseElementDcv);
            baseElementMock = AddRelationsToMockElement(baseElementMock, relationsMock);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(baseElementMock);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);

            Mock<IDataLanguage> dataLanguageMock = GetDataLanguageMock();
            RelationshipRepository relationshipRepository = new RelationshipRepository(dataAccessMock.Object, loggerMock.Object, dataLanguageMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await relationshipRepository.GetRelationships(dcvId));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<System.ArgumentNullException>(result);
        }

        [Fact]
        [Trait("Category", "RelationshipRepository")]
        public async Task GetRelationships_WrongFormatDcvId_IRelationship()
        {
            IDcvId baseElementDcv = DcvId.FromDcvKey(BASE_ELEMENT_DCVID);
            IDcvId fromElementDcv = DcvId.FromDcvKey(FROM_ELEMENT_DCVID);
            IDcvId toElementDcv = DcvId.FromDcvKey(TO_ELEMENT_DCVID);
            IDcvId relationDcv = DcvId.FromDcvKey(RELATION_DCVID);
            IDcvId relation2Dcv = DcvId.FromDcvKey(RELATION2_DCVID);

            Mock<IElement> fromElementMock = GetMockElement(fromElementDcv);
            Mock<IElement> toElementMock = GetMockElement(toElementDcv);
            Mock<IRelation> relation1 = GetMockRelation(relationDcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_Unknown);
            Mock<IRelation> relation2 = GetMockRelation(relation2Dcv, fromElementMock, toElementMock, RELBuffer.MvmSrv_CATtpe.MvmSRV_CATtpe_HypTo);

            List<IRelation> relationsMock = new List<IRelation>();
            relationsMock.Add(relation1.Object);
            relationsMock.Add(relation2.Object);

            Mock<ILogger<RelationshipRepository>> loggerMock = GetMockLogger();

            Mock<IElement> baseElementMock = GetMockElement(baseElementDcv);
            baseElementMock = AddRelationsToMockElement(baseElementMock, relationsMock);

            Mock<IMavimDatabaseModel> mavimDataModelMock = GetMockDataModel(baseElementMock);

            Mock<IMavimDbDataAccess> dataAccessMock = new Mock<IMavimDbDataAccess>();
            dataAccessMock.Setup(x => x.DatabaseModel).Returns(mavimDataModelMock.Object);

            Mock<IDataLanguage> dataLanguageMock = GetDataLanguageMock();
            RelationshipRepository relationshipRepository = new RelationshipRepository(dataAccessMock.Object, loggerMock.Object, dataLanguageMock.Object);

            // Act
            var result = await Record.ExceptionAsync(async () => await relationshipRepository.GetRelationships(WRONG_ELEMENT_DCVID));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<System.ArgumentException>(result);
        }

        private Mock<ILogger<RelationshipRepository>> GetMockLogger()
        {
            return new Mock<ILogger<RelationshipRepository>>();
        }

        private Mock<IElement> GetMockElement(IDcvId elementDcv, string name = "test", TreeIconID iconId = TreeIconID.MiscellaneousIconID_Fillin)
        {
            Mock<IElement> elementMock = new Mock<IElement>();
            elementMock.Setup(topic => topic.DcvID).Returns(elementDcv);
            elementMock.Setup(topic => topic.Name).Returns(name);
            elementMock.Setup(topic => topic.Visual.IconResourceID).Returns(iconId);

            return elementMock;
        }

        private Mock<IElement> AddRelationsToMockElement(Mock<IElement> elementMock, IEnumerable<IRelation> relationsMock)
        {
            elementMock.Setup(element => element.Relations).Returns(relationsMock);

            return elementMock;
        }

        private static Mock<IMavimDatabaseModel> GetMockDataModel(Mock<IElement> elementMock)
        {
            Mock<IMavimDatabaseModel> mavimDatabaseModel = new Mock<IMavimDatabaseModel>();

            mavimDatabaseModel
                .Setup(model => model.ElementRepository
                    .GetElement(It.Is<IDcvId>(d => d.Ver == elementMock.Object.DcvID.Ver
                                                   && d.Cde == elementMock.Object.DcvID.Cde
                                                   && d.Dbs == elementMock.Object.DcvID.Dbs)))
                .Returns(elementMock.Object);

            return mavimDatabaseModel;
        }

        private Mock<IRelation> GetMockRelation(IDcvId dcvId, Mock<IElement> fromElementMock, Mock<IElement> toElementMock, RELBuffer.MvmSrv_CATtpe categoryType, bool isAttributeRelation = false, bool isDestroyed = false)
        {
            Mock<IRelationCategory> relationCategory = new Mock<IRelationCategory>();
            relationCategory.SetupGet(category => category.Type).Returns(categoryType);

            Mock<IRelationType> relationType = new Mock<IRelationType>();
            relationType.Setup(type => type.Type).Returns(RELBuffer.MvmSrv_RELtpe.MvmSRV_RELtpe_who);

            Mock<IRelation> relationMock = new Mock<IRelation>();
            relationMock.Setup(relation => relation.GetCategory(It.IsAny<IElement>())).Returns(relationCategory.Object);
            relationMock.Setup(relation => relation.DcvId).Returns(dcvId);
            relationMock.Setup(relation => relation.Type).Returns(relationType.Object);
            relationMock.Setup(relation => relation.ToElement).Returns(toElementMock.Object);
            relationMock.Setup(relation => relation.FromElement).Returns(fromElementMock.Object);
            relationMock.Setup(relation => relation.IsAttributeRelation).Returns(isAttributeRelation);
            relationMock.Setup(relation => relation.IsDestroyed).Returns(isDestroyed);

            return relationMock;
        }

        private Mock<IDataLanguage> GetDataLanguageMock()
        {
            var mock = new Mock<IDataLanguage>();
            mock.Setup(x => x.Type).Returns(DataLanguageType.English);

            return mock;
        }

        private static Mock<IRelationship> GetRelationshipMock()
        {
            Mock<IRelationshipElement> relationshipTopic = new Mock<IRelationshipElement>();
            relationshipTopic.Setup(t => t.Dcv).Returns(TO_ELEMENT_DCVID);
            Mock<IRelationship> relationship = new Mock<IRelationship>();
            relationship.Setup(r => r.CategoryType).Returns(CategoryType.Who);
            relationship.Setup(r => r.Dcv).Returns(RELATION2_DCVID);
            relationship.Setup(r => r.WithElement).Returns(relationshipTopic.Object);
            relationship.Setup(r => r.Icon).Returns("0");
            return relationship;
        }

        public static IEnumerable<object[]> InvalidAgumentDcvIdValues
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { "" };
            }
        }
    }
}