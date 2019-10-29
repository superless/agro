
using Moq;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqUploadImage
    {
        private readonly Mock<IUploadImage> mockUpload;
        public MoqUploadImage()
        {
            mockUpload = new Mock<IUploadImage>();
            mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync(FakeGenerator.UploadImageBase64());
        }

        public Mock<IUploadImage> GetUploadImage => mockUpload;
    }
}
