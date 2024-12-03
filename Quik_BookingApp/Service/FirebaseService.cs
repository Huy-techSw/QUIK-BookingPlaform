using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Modal;
using Quik_BookingApp.Repos.Interface;
using RestSharp;
using System.Net;

namespace Quik_BookingApp.Service
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseConfiguration _firebaseConfiguration;
        private readonly IConfiguration _configuration;
        public FirebaseService(IConfiguration configuration, IOptions<FirebaseConfiguration> firebaseConfiguration)
        {
            this._firebaseConfiguration = firebaseConfiguration.Value;
            this._configuration = configuration;
        }

        public async Task<IServiceResult> DeleteFileFromFirebase(string pathFileName)
        {
            var _result = new ServiceResult();
            try
            {
                var auth = new FirebaseAuthProvider (new FirebaseConfig(_firebaseConfiguration.ApiKey));
                var account = await auth.SignInWithEmailAndPasswordAsync(_firebaseConfiguration.AuthEmail, _firebaseConfiguration.AuthPassword);
                var storage = new FirebaseStorage(
             _firebaseConfiguration.Bucket,
             new FirebaseStorageOptions
             {
                 AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                 ThrowOnCancel = true
             });
                await storage
                    .Child(pathFileName)
                    .DeleteAsync();
                _result.Message = "Delete image successful";
                _result.Status = 200;
            }
            catch (FirebaseStorageException ex)
            {
                _result.Message = $"Error deleting image: {ex.Message}";
            }
            return _result;
        }

        public async Task<string> GetUrlImageFromFirebase(string pathFileName)
        {
            //var a = pathFileName.Split("/");
            var a = pathFileName.Split("/o/")[1];
            //pathFileName = $"{a[0]}%2F{a[1]}";
            var api = $"https://console.firebase.google.com/u/1/project/quik-1893b/storage/quik-1893b.appspot.com/files?hl={a}";
            if (string.IsNullOrEmpty(pathFileName))
            {
                return string.Empty;
            }

            var client = new RestClient();
            var request = new RestRequest(api);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jmessage = JObject.Parse(response.Content);
                var downloadToken = jmessage.GetValue("downloadTokens").ToString();
                return
                    $"https://console.firebase.google.com/u/1/project/quik-1893b/storage/{_configuration["quik-1893b.appspot.com"]}/files/{pathFileName}?alt=media&token={downloadToken}";
            }

            return string.Empty;
        }

      

        public async Task<IServiceResult> UploadFileToFirebase(IFormFile file, string pathFileName)
        {
            var _result = new ServiceResult();

            if (file == null || file.Length == 0)
            {
                _result.Message = "The file is empty";
                _result.Status = 400;
                return _result;
            }

            try
            {
                var apiKey = _firebaseConfiguration.ApiKey;
                var stream = file.OpenReadStream();
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var account = await auth.SignInWithEmailAndPasswordAsync(_firebaseConfiguration.AuthEmail, _firebaseConfiguration.AuthPassword);

                var encodedPathFileName = Uri.EscapeDataString(pathFileName);
                var task = new FirebaseStorage(
                    _firebaseConfiguration.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(encodedPathFileName)
                    .PutAsync(stream);

                var downloadUrl = await task;
                _result.Status = 200;
                _result.Message = "Success";
                _result.Data = downloadUrl;
            }
            catch (FirebaseStorageException ex)
            {
                _result.Status = 500;
                _result.Message = $"Upload failed: {ex.Message}";
            }

            return _result;
        }

        public async Task<IServiceResult> UploadFilesToFirebase(List<IFormFile> files, string basePath)
        {
            var _result = new ServiceResult();
            var uploadResults = new List<string>();

            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseConfiguration.ApiKey));
            var account = await auth.SignInWithEmailAndPasswordAsync(_firebaseConfiguration.AuthEmail, _firebaseConfiguration.AuthPassword);
            var storage = new FirebaseStorage(
                _firebaseConfiguration.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(account.FirebaseToken),
                    ThrowOnCancel = true
                });

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    _result.Message = $"One or more files are empty: {file?.FileName}";
                    continue;
                }

                try
                {
                    var stream = file.OpenReadStream();
                    string destinationPath = $"{basePath}/{file.FileName}";

                    var task = storage.Child(destinationPath).PutAsync(stream);
                    var downloadUrl = await task;

                    uploadResults.Add(downloadUrl);
                }
                catch (FirebaseStorageException ex)
                {
                    _result.Status = 500;
                    _result.Message = $"Upload failed for file: {file.FileName} - {ex.Message}";
                }
            }

            _result.Data = uploadResults;
            _result.Status = uploadResults.Count == files.Count ? 200 : 500;
            _result.Message = uploadResults.Count == files.Count
                ? "All files uploaded successfully"
                : "Some files failed to upload";

            return _result;
        }


        
    }
}
