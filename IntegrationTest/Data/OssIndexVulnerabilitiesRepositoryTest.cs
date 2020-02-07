using Data.MsSQL;
using Data.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using IntegrationTest;

namespace Data.IntegrationTest
{
    [TestFixture]
    public class OssIndexVulnerabilitiesRepositoryTest
    {
        private int _OssIndexId = 5;

        [Test]
        public void InsertAndSelect_ShouldEqualInserted()
        {
            // Arrange
            var dbModel = new OssIndexVulnerabilitiesModel()
            {
                OssIndexId = _OssIndexId,
                InsertDate = DateTime.Now,
                OssId = "expedita",
                Title = "dolor",
                Description = "cumque",
                CvssScore = 89,
                CvssVector = "maxime",
                Cve = "ut",
                Reference = "temporibus",
            };
            var expectedValue = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .Insert(dbModel);

            // Act
            var actualValue = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .Select(expectedValue)
                .Id;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
        [Test]
        public void InsertBulkThenSelectList_ShouldEqualTwo()
        {
            // Arrange
            var expectedValue = 2;
            var dummyString = Guid.NewGuid().ToString().Replace("-", "");
            var listPoco = new List<OssIndexVulnerabilitiesModel>()
            {
                new OssIndexVulnerabilitiesModel()
                {
                    OssIndexId = _OssIndexId,
                    InsertDate = DateTime.Now,
                    OssId = dummyString,
                    Title = "dolor",
                    Description = "doloribus",
                    CvssScore = 7036,
                    CvssVector = "aperiam",
                    Cve = "iste",
                    Reference = "beatae",
                },
                new OssIndexVulnerabilitiesModel()
                {
                    OssIndexId = _OssIndexId,
                    InsertDate = DateTime.Now,
                    OssId = dummyString,
                    Title = "totam",
                    Description = "doloribus",
                    CvssScore = 7036,
                    CvssVector = "aperiam",
                    Cve = "iste",
                    Reference = "beatae",
                }
            };

            // Act
            new OssIndexVulnerabilitiesRepository(AppState.ConnectionString).InsertBulk(listPoco);
            var actualValue = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .SelectList()
                .Where(x => x.OssId.Equals(dummyString))
                .ToList()
                .Count;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
        [Test]
        public void InsertAndDelete_ShouldNoLongerExistAfterDelete()
        {
            // Arrange
            var expectedValue = 0;
            var dbModel = new OssIndexVulnerabilitiesModel()
            {
                OssIndexId = _OssIndexId,
                InsertDate = DateTime.Now,
                OssId = "earum",
                Title = "et",
                Description = "voluptatem",
                CvssScore = 3983,
                CvssVector = "quasi",
                Cve = "deserunt",
                Reference = "fugit",
            };

            // Act
            var newId = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString).Insert(dbModel);
            new OssIndexVulnerabilitiesRepository(AppState.ConnectionString).Delete(newId);
            var actualValue = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .Select(newId)
                .Id;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
        [Test]
        public void InsertThenUpdate_ShouldReflectChanges()
        {
            // Arrange
            var dummyString = Guid.NewGuid().ToString().Replace("-", "");
            var dbModel = new OssIndexVulnerabilitiesModel()
            {
                OssIndexId = _OssIndexId,
                InsertDate = DateTime.Now,
                OssId = dummyString,
                Title = "et",
                Description = "voluptatem",
                CvssScore = 3983,
                CvssVector = "quasi",
                Cve = "deserunt",
                Reference = "fugit",
            };

            // Act
            var newId = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .Insert(dbModel);
                dummyString = Guid.NewGuid().ToString().Replace("-", "");
            var dbModel2 = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .Select(newId);
            dbModel2.OssId = dummyString;

            new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .Update(dbModel2);
            var actualValue = new OssIndexVulnerabilitiesRepository(AppState.ConnectionString)
                .Select(newId)
                .OssId;

            // Assert
            Assert.AreEqual(dummyString, actualValue);
        }
    }
}
