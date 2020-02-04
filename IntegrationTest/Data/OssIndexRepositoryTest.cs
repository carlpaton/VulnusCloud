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
    public class OssIndexRepositoryTest
    {
        [Test]
        public void InsertAndSelect_ShouldEqualInserted()
        {
            // Arrange
            var dbModel = new OssIndexModel()
            {
                ComponentId = 2,
                Version = 9125,
                Coordinates = "vel",
                Description = "ut",
                Reference = "praesentium",
                ExpireDate = DateTime.Now,
                HttpStatus = 6077,
            };
            var expectedValue = new OssIndexRepository(AppState.ConnectionString)
                .Insert(dbModel);

            // Act
            var actualValue = new OssIndexRepository(AppState.ConnectionString)
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
            var listPoco = new List<OssIndexModel>()
            {
                new OssIndexModel()
                {
                    ComponentId = 2,
                    Version = 6072,
                    Coordinates = dummyString,
                    Description = "itaque",
                    Reference = "praesentium",
                    ExpireDate = DateTime.Now,
                    HttpStatus = 6077,
                },
                new OssIndexModel()
                {
                    ComponentId = 2,
                    Version = 6072,
                    Coordinates = dummyString,
                    Description = "itaque",
                    Reference = "praesentium",
                    ExpireDate = DateTime.Now,
                    HttpStatus = 6077,
                }
            };

            // Act
            new OssIndexRepository(AppState.ConnectionString).InsertBulk(listPoco);
            var actualValue = new OssIndexRepository(AppState.ConnectionString)
                .SelectList()
                .Where(x => x.Coordinates.Equals(dummyString))
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
            var dbModel = new OssIndexModel()
            {
                ComponentId = 2,
                Version = 6072,
                Coordinates = "esse",
                Description = "itaque",
                Reference = "cumque",
                ExpireDate = DateTime.Now,
                HttpStatus = 90,
            };

            // Act
            var newId = new OssIndexRepository(AppState.ConnectionString).Insert(dbModel);
            new OssIndexRepository(AppState.ConnectionString).Delete(newId);
            var actualValue = new OssIndexRepository(AppState.ConnectionString)
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
            var dbModel = new OssIndexModel()
            {
                ComponentId = 2,
                Version = 85,
                Coordinates = dummyString,
                Description = "dolor",
                Reference = "cumque",
                ExpireDate = DateTime.Now,
                HttpStatus = 90,
            };

            // Act
            var newId = new OssIndexRepository(AppState.ConnectionString)
                .Insert(dbModel);
                dummyString = Guid.NewGuid().ToString().Replace("-", "");
            var dbModel2 = new OssIndexRepository(AppState.ConnectionString)
                .Select(newId);
            dbModel2.Coordinates = dummyString;

            new OssIndexRepository(AppState.ConnectionString)
                .Update(dbModel2);
            var actualValue = new OssIndexRepository(AppState.ConnectionString)
                .Select(newId)
                .Coordinates;

            // Assert
            Assert.AreEqual(dummyString, actualValue);
        }
    }
}
