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
    public class ProjectRepositoryTest
    {
        [Test]
        public void InsertAndSelect_ShouldEqualInserted()
        {
            // Arrange
            var dbModel = new ProjectModel()
            {
                ProjectName = "minima",
            };
            var expectedValue = new ProjectRepository(AppState.ConnectionString)
                .Insert(dbModel);

            // Act
            var actualValue = new ProjectRepository(AppState.ConnectionString)
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
            var listPoco = new List<ProjectModel>()
            {
                new ProjectModel()
                {
                    ProjectName = dummyString,
                },
                new ProjectModel()
                {
                    ProjectName = dummyString,
                }
            };

            // Act
            new ProjectRepository(AppState.ConnectionString).InsertBulk(listPoco);
            var actualValue = new ProjectRepository(AppState.ConnectionString)
                .SelectList()
                .Where(x => x.ProjectName.Equals(dummyString))
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
            var dbModel = new ProjectModel()
            {
                ProjectName = "quae",
            };

            // Act
            var newId = new ProjectRepository(AppState.ConnectionString).Insert(dbModel);
            new ProjectRepository(AppState.ConnectionString).Delete(newId);
            var actualValue = new ProjectRepository(AppState.ConnectionString)
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
            var dbModel = new ProjectModel()
            {
                ProjectName = dummyString,
            };

            // Act
            var newId = new ProjectRepository(AppState.ConnectionString)
                .Insert(dbModel);
                dummyString = Guid.NewGuid().ToString().Replace("-", "");
            var dbModel2 = new ProjectRepository(AppState.ConnectionString)
                .Select(newId);
            dbModel2.ProjectName = dummyString;

            new ProjectRepository(AppState.ConnectionString)
                .Update(dbModel2);
            var actualValue = new ProjectRepository(AppState.ConnectionString)
                .Select(newId)
                .ProjectName;

            // Assert
            Assert.AreEqual(dummyString, actualValue);
        }
    }
}
