using Common.Models;
using DataAccessLayer;
using DbInteractionService;
using FluentAssertions;
using InteractionService;
using NUnit.Framework;

namespace Testing
{
    [TestFixture]
    public class CacheRealizationTests
    {
        [Test]
        public void InteractionService_CacheRealization_InputTest()
        {
            IDataModelInteraction dataModelInteraction = new FirstInteractionRealization(new DbInteractionRealize_Cache());

            List<InsertInputModel> data = new List<InsertInputModel>() { 
                new InsertInputModel(){ Code = "4", Value = "Val2" },
                new InsertInputModel(){ Code = "5", Value = "Val1" },
            };

            Assert.DoesNotThrowAsync(async () => await dataModelInteraction.ConvertAndSendToDB(data));
        }

        [Test]
        public void InteractionService_CacheRealization_InputTest_Exception()
        {
            IDataModelInteraction dataModelInteraction = new FirstInteractionRealization(new DbInteractionRealize_Cache());

            List<InsertInputModel> data = new List<InsertInputModel>() {
                new InsertInputModel(){ Code = "adf", Value = "Val2" },
                new InsertInputModel(){ Code = "5", Value = "Val1" },
            };

            var exception = Assert.CatchAsync(async () => await dataModelInteraction.ConvertAndSendToDB(data));
            // Исключение при Парсинге int
            exception.GetType().Should().Be(typeof(FormatException));
        }

        [Test]
        public async Task InteractionService_CacheRealization_GetTest()
        {
            IDataModelInteraction dataModelInteraction = new FirstInteractionRealization(new DbInteractionRealize_Cache());

            var data = await dataModelInteraction.GetFromDB(null, null, "");

            data.Count.Should().Be(0);
        }

        [Test]
        public async Task InteractionService_CacheRealization_InputAndGetTest_1()
        {
            IDataModelInteraction dataModelInteraction = new FirstInteractionRealization(new DbInteractionRealize_Cache());

            List<InsertInputModel> data = new List<InsertInputModel>() {
                new InsertInputModel(){ Code = "4", Value = "Val2" },
                new InsertInputModel(){ Code = "5", Value = "Val1" },
            };

            await dataModelInteraction.ConvertAndSendToDB(data);

            var dbInfo = await dataModelInteraction.GetFromDB(null, null, "");

            dbInfo.Count().Should().Be(2);
            dbInfo[0].Name.Should().Be("Val2");
            dbInfo[1].Name.Should().Be("Val1");
        }

        [Test]
        public async Task InteractionService_CacheRealization_InputAndGetTest_2()
        {
            IDataModelInteraction dataModelInteraction = new FirstInteractionRealization(new DbInteractionRealize_Cache());

            List<InsertInputModel> data = new List<InsertInputModel>() {
                new InsertInputModel(){ Code = "5", Value = "Val2" },
                new InsertInputModel(){ Code = "3", Value = "Val4" },
            };

            await dataModelInteraction.ConvertAndSendToDB(data);

            var dbInfo = await dataModelInteraction.GetFromDB(null, null, "");

            dbInfo.Count().Should().Be(2);
            dbInfo[0].Name.Should().Be("Val4");
            dbInfo[1].Name.Should().Be("Val2");
        }

        [Test]
        public async Task InteractionService_CacheRealization_InputAndGetTest_3()
        {
            IDataModelInteraction dataModelInteraction = new FirstInteractionRealization(new DbInteractionRealize_Cache());

            List<InsertInputModel> data = new List<InsertInputModel>() {
                new InsertInputModel(){ Code = "4", Value = "Val2" },
                new InsertInputModel(){ Code = "5", Value = "Val1" },
                new InsertInputModel(){ Code = "2", Value = "Val6" }
            };

            await dataModelInteraction.ConvertAndSendToDB(data);

            var dbInfo = await dataModelInteraction.GetFromDB(null, null, "");

            dbInfo.Count().Should().Be(3);

            dbInfo[0].Number.Should().Be(1);
            dbInfo[1].Number.Should().Be(2);
            dbInfo[2].Number.Should().Be(3);

            dbInfo[0].Name.Should().Be("Val6");
            dbInfo[1].Name.Should().Be("Val2");
            dbInfo[2].Name.Should().Be("Val1");
        }
    }
}
