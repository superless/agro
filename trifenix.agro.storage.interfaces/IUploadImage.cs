using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace trifenix.agro.storage.interfaces
{
    public interface IUploadImage
    {
        Task<string> UploadImageBase64(string base64);

    }
}
