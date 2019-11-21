using Moq;
using System;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.external.operations.tests.helper.Moqs;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.tests.helper.Instances
{
    public static class UploadImageInstances {
        public static Mock<IUploadImage> GetInstance(Results result) {

            var mockUpload = new Mock<IUploadImage>();
            
            

            switch (result)
            {
                case Results.Nullables:
                    mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync((string)null);
                    break;
                case Results.Empty:
                    mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync(string.Empty);
                    break;
                case Results.Errors:
                    mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).Throws(new Exception("simulated test error"));
                    break;
                case Results.Values:
                    mockUpload.Setup(s => s.UploadImageBase64(It.IsAny<string>())).ReturnsAsync(FakeGenerator.CreateString());
                    break;
                default:
                    break;
            }
            return mockUpload;
        }

    }
}
