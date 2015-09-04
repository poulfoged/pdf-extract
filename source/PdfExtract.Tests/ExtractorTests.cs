using NUnit.Framework;

namespace PdfExtract.Tests
{
    [TestFixture]
    public class ExtractorTests
    {
        [Test]
        public void Can_extract_text()
        {
            ////Arrange
            var pdfStream = GetType().Assembly.GetManifestResourceStream(typeof (ExtractorTests), "sample.pdf");
            string result;

            ////Act
            using (var extractor = new Extractor())
                result = extractor.ExtractToString(pdfStream);

            ////Assert
            Assert.That(result.Trim(), Is.EqualTo("hello world"));
        }
    }
}