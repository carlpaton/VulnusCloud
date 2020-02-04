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
    public class ReportRepositoryTest
    {
        [Test]
        public void InsertAndSelect_ShouldEqualInserted()
        {
            // Arrange
            var dbModel = new ReportModel()
            {
                ProjectId = 2,
                InsertDate = DateTime.Now,
            };
            var expectedValue = new ReportRepository(AppState.ConnectionString)
                .Insert(dbModel);

            // Act
            var actualValue = new ReportRepository(AppState.ConnectionString)
                .Select(expectedValue)
                .Id;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
        [Test]
        public void InsertBulkThenSelectList_ShouldEqualTwo()
        {
            // Arrange
            var dummyId = 2;
            var listPoco = new List<ReportModel>()
            {
                new ReportModel()
                {
                    ProjectId = dummyId,
                    InsertDate = DateTime.Now,
                },
                new ReportModel()
                {
                    ProjectId = dummyId,
                    InsertDate = DateTime.Now,
                }
            };

            // Act
            new ReportRepository(AppState.ConnectionString).InsertBulk(listPoco);
            var actualValue = new ReportRepository(AppState.ConnectionString)
                .SelectList()
                .Where(x => x.ProjectId.Equals(dummyId))
                .ToList()
                .Count;

            // Assert
            Assert.IsTrue(actualValue >= 2);
        }
        [Test]
        public void InsertAndDelete_ShouldNoLongerExistAfterDelete()
        {
            // Arrange
            var expectedValue = 0;
            var dbModel = new ReportModel()
            {
                ProjectId = 2,
                InsertDate = DateTime.Now,
            };

            // Act
            var newId = new ReportRepository(AppState.ConnectionString).Insert(dbModel);
            new ReportRepository(AppState.ConnectionString).Delete(newId);
            var actualValue = new ReportRepository(AppState.ConnectionString)
                .Select(newId)
                .Id;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
        [Test]
        public void InsertThenUpdate_ShouldReflectChanges()
        {
            // Arrange
            var dummyId = 2;
            var dbModel = new ReportModel()
            {
                ProjectId = dummyId,
                InsertDate = DateTime.Now,
            };

            // Act
            var newId = new ReportRepository(AppState.ConnectionString)
                .Insert(dbModel);
                dummyId = 3;
            var dbModel2 = new ReportRepository(AppState.ConnectionString)
                .Select(newId);
            dbModel2.ProjectId = dummyId;

            new ReportRepository(AppState.ConnectionString)
                .Update(dbModel2);
            var actualValue = new ReportRepository(AppState.ConnectionString)
                .Select(newId)
                .ProjectId;

            // Assert
            Assert.AreEqual(dummyId, actualValue);
        }
    }
}
