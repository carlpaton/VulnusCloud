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
    public class ReportLinesRepositoryTest
    {
        [Test]
        public void InsertAndSelect_ShouldEqualInserted()
        {
            // Arrange
            var dbModel = new ReportLinesModel()
            {
                ReportId = 7,
                OssIndexId = 6,
            };
            var expectedValue = new ReportLinesRepository(AppState.ConnectionString)
                .Insert(dbModel);

            // Act
            var actualValue = new ReportLinesRepository(AppState.ConnectionString)
                .Select(expectedValue)
                .Id;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
        [Test]
        public void InsertBulkThenSelectList_ShouldEqualTwo()
        {
            // Arrange
            var dummyId = 7;
            var listPoco = new List<ReportLinesModel>()
            {
                new ReportLinesModel()
                {
                    ReportId = dummyId,
                    OssIndexId = 6,
                },
                new ReportLinesModel()
                {
                    ReportId = dummyId,
                    OssIndexId = 6,
                }
            };

            // Act
            new ReportLinesRepository(AppState.ConnectionString).InsertBulk(listPoco);
            var actualValue = new ReportLinesRepository(AppState.ConnectionString)
                .SelectList()
                .Where(x => x.ReportId.Equals(dummyId))
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
            var dbModel = new ReportLinesModel()
            {
                ReportId = 7,
                OssIndexId = 6,
            };

            // Act
            var newId = new ReportLinesRepository(AppState.ConnectionString).Insert(dbModel);
            new ReportLinesRepository(AppState.ConnectionString).Delete(newId);
            var actualValue = new ReportLinesRepository(AppState.ConnectionString)
                .Select(newId)
                .Id;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
        [Test]
        public void InsertThenUpdate_ShouldReflectChanges()
        {
            // Arrange
            var dummyId = 7;
            var dbModel = new ReportLinesModel()
            {
                ReportId = dummyId,
                OssIndexId = 6,
            };

            // Act
            var newId = new ReportLinesRepository(AppState.ConnectionString)
                .Insert(dbModel);
                dummyId = 8;
            var dbModel2 = new ReportLinesRepository(AppState.ConnectionString)
                .Select(newId);
            dbModel2.ReportId = dummyId;

            new ReportLinesRepository(AppState.ConnectionString)
                .Update(dbModel2);
            var actualValue = new ReportLinesRepository(AppState.ConnectionString)
                .Select(newId)
                .ReportId;

            // Assert
            Assert.AreEqual(dummyId, actualValue);
        }
    }
}
