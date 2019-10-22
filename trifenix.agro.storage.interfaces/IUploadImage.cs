using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace trifenix.agro.storage.interfaces
{
    public interface IUploadImage
    {
        Task<string> UploadImageBase64(string base64);

    }
}
