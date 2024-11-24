using LabOrderProcessor.API.Services;

namespace LabOrderProcessor.Tests
{
    [TestClass]
    public sealed class Test1
    {

        [TestMethod]
        public void TestRedactPhi()
        {
            // Arrange
            var service = new PhiDetectionService();
            var input = "Patient Name: John Doe\nDate of Birth: 01/15/1990\nSocial Security Number: 123-45-6789\nAddress: 123 Main St\nPhone Number: (123) 456-7890\nEmail: john.doe@example.com\nMedical Record Number: MRN-1234567";
            var expectedOutput = "Patient Name: [REDACTED]\nDate of Birth: [REDACTED]\nSocial Security Number: [REDACTED]\nAddress: [REDACTED]\nPhone Number: [REDACTED]\nEmail: [REDACTED]\nMedical Record Number: [REDACTED]";

            // Act
            var result = service.RedactPhi(input);

            // Assert
            Assert.AreEqual(expectedOutput, result);
        }
    }
}
