using System.IO;
using NUnit.Framework;

namespace PdfExtract.Tests
{
    [TestFixture]
    public class TemporaryFileTests
    {
        [Test]
        public void File_is_deleted_after_use()
        {
            ////Arrange
            FileInfo fileInfo;

            ////Act
            using (var file = new TemporaryFile())
            using (var stream = file.Info.OpenWrite())
            {
                fileInfo = file.Info;
                stream.WriteByte(1);
            }

            ////Assert
            fileInfo.Refresh();
            Assert.That(!fileInfo.Exists);
        }

        [Test]
        public void File_can_be_initialized()
        {
            ////Arrange & Act
            using (var file = new TemporaryFile(new MemoryStream(new byte[] {1,2,3})))
            {
                ////Assert
                file.Info.Refresh();
                Assert.That(file.Info.Length, Is.EqualTo(3));
            }
        }

        [Test]
        public void Can_copy_to_stream()
        {
            ////Arrange
            Stream result = new MemoryStream();

            ////Act
            using (var file = new TemporaryFile())
            {
                using (var stream = file.Info.OpenWrite())
                    stream.WriteByte(1);

                file.CopyTo(result);
            }

            ////Assert
            Assert.That(result.Length, Is.EqualTo(1));
        }
    }
}