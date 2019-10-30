using Moq;
using System;
using trifenix.agro.common.tests.interfaces;
using trifenix.agro.external.operations.tests.helper.staticHelper;
using trifenix.agro.storage.interfaces;

namespace trifenix.agro.external.operations.tests.helper
{
    public class MoqUploadImageRepo : IMoqRepo<IUploadImage>
    {
        public Mock<IUploadImage> GetMoqRepo(Results result)
        {
            switch (result)
            {
                case Results.Nullables:
                    return MoqUploadImage.GetUploadImageReturnNull();
                case Results.Empty:
                    return MoqUploadImage.GetUploadImageReturnEmptyResult();
                case Results.Errors:
                    return MoqUploadImage.GetUploadImageThrowException();
                case Results.Values:
                    return MoqUploadImage.GetUploadImageReturnResult();
            }
            throw new Exception("bad parameters");
        }
    }
}