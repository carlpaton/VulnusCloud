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
    public class ComponentRepositoryTest
    {
        [Test]
        public void InsertAndSelect_ShouldEqualInserted()
        {
            // Arrange
            var dbModel = new ComponentModel()
            {
                Name = "quae"
            };
            var expectedValue = new ComponentRepository(AppState.ConnectionString)
                .Insert(dbModel);

            // Act
            var actualValue = new ComponentRepository(AppState.ConnectionString)
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
            var listPoco = new List<ComponentModel>()
            {
                new ComponentModel()
                {
                    Name = dummyString
                },
                new ComponentModel()
                {
                    Name = dummyString
                }
            };

            // Act
            new ComponentRepository(AppState.ConnectionString).InsertBulk(listPoco);
            var actualValue = new ComponentRepository(AppState.ConnectionString)
                .SelectList()
                .Where(x => x.Name.Equals(dummyString))
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
            var dbModel = new ComponentModel()
            {
                Name = "suscipit"
            };

            // Act
            var newId = new ComponentRepository(AppState.ConnectionString).Insert(dbModel);
            new ComponentRepository(AppState.ConnectionString).Delete(newId);
            var actualValue = new ComponentRepository(AppState.ConnectionString)
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
            var dbModel = new ComponentModel()
            {
                Name = dummyString
            };

            // Act
            var newId = new ComponentRepository(AppState.ConnectionString)
                .Insert(dbModel);
                dummyString = Guid.NewGuid().ToString().Replace("-", "");
            var dbModel2 = new ComponentRepository(AppState.ConnectionString)
                .Select(newId);
            dbModel2.Name = dummyString;

            new ComponentRepository(AppState.ConnectionString)
                .Update(dbModel2);
            var actualValue = new ComponentRepository(AppState.ConnectionString)
                .Select(newId)
                .Name;

            // Assert
            Assert.AreEqual(dummyString, actualValue);
        }
    }
}
