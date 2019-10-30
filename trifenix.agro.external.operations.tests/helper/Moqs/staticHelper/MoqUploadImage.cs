
using Moq;
using System;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.tests.helper.staticHelper
{
    public static class MoqUploadImage
    {
        

        public static Mock<IUploadImage> GetUploadImageReturnResult() {
            var mockUpload = new Mock<IUploadImage>();
            mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync(FakeGenerator.UploadImageBase64());
            return mockUpload;
        }

        public static Mock<IUploadImage> GetUploadImageReturnNull()
        {
            var mockUpload = new Mock<IUploadImage>();
            mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync((string)null);
            return mockUpload;
        }

        public static Mock<IUploadImage> GetUploadImageReturnEmptyResult()
        {
            var mockUpload = new Mock<IUploadImage>();
            mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync(string.Empty);
            return mockUpload;
        }

        public static Mock<IUploadImage> GetUploadImageThrowException()
        {
            var mockUpload = new Mock<IUploadImage>();
            mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).Throws(new Exception("simulated test error"));
            return mockUpload;
        }
    }
}
