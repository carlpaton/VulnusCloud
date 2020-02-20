using Business;
using Business.Exceptions;
using Business.Model;
using NUnit.Framework;

namespace UnitTests.Business
{
    [TestFixture]
    public class CoordinatesServiceTests
    {
        [Test]
        public void GetCoordinates_only_required_parameters_supplied_returns_valid_package_url()
        {
            // Arrange
            var classUnderTest = new CoordinatesService();
            var expected = "pkg:nuget/System.Net.Http@4.3.1";
            var coordinatePartsModel = new CoordinatePartsModel()
            {
                Type = "nuget",
                Name = "System.Net.Http",
                Version = "4.3.1"
            };

            // Act
            var actual = classUnderTest.GetCoordinates(coordinatePartsModel);
            
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCoordinates_all_parameters_supplied_returns_valid_package_url()
        {
            // Arrange
            var classUnderTest = new CoordinatesService();
            var expected = "pkg:nuget/hoe/System.Net.Http@4.3.1";
            var coordinatePartsModel = new CoordinatePartsModel()
            {
                Type = "nuget",
                Name = "System.Net.Http",
                Version = "4.3.1",
                Namespace = "hoe"
            };

            // Act
            var actual = classUnderTest.GetCoordinates(coordinatePartsModel);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("", "Name", "Version")]
        [TestCase("Type", "", "Version")]
        [TestCase("Type", "Name", "")]

        [TestCase(null, "Name", "Version")]
        [TestCase("Type", null, "Version")]
        [TestCase("Type", "Name", null)]
        public void GetCoordinates_required_parameters_not_supplied_throws_CoordinateNotFoundException(string type, string name, string version)
        {
            // Arrange
            var classUnderTest = new CoordinatesService();
            var coordinatePartsModel = new CoordinatePartsModel() 
            {
                Type = type,
                Name = name,
                Version = version
            };

            // Act
            // Assert
            Assert.Throws<CoordinateNotFoundException>(() => classUnderTest.GetCoordinates(coordinatePartsModel));
        }
    }
}
