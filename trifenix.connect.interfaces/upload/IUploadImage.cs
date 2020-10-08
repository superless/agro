using System.Threading.Tasks;

namespace trifenix.connect.interfaces.upload
{
    public interface IUploadImage
    {
        Task<string> UploadImageBase64(string base64);

    }
}
