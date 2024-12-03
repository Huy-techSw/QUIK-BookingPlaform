using Quik_BookingApp.Helper;

namespace Quik_BookingApp.Repos.Interface
{
    public interface IFirebaseService
    {
        Task<IServiceResult> UploadFileToFirebase(IFormFile file, string pathFileName);
        Task<IServiceResult> UploadFilesToFirebase(List<IFormFile> files, string basePath);
        public Task<string> GetUrlImageFromFirebase(string pathFileName);
        public Task<IServiceResult> DeleteFileFromFirebase(string pathFileName);
    }
}
