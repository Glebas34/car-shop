using Firebase.Storage;
using Firebase.Auth;
using ImageService.Interfaces;
using ImageService.Options;
using Microsoft.Extensions.Options;

namespace ImageService.Services
{
    public class StorageServiceCreator: IStorageServiceCreator
    {
        private string ApiKey;
        private string Bucket;
        private string AuthEmail;
        private string AuthPassword;

        public StorageServiceCreator(IOptions<StorageOptions> options)
        {
            ApiKey = options.Value.ApiKey;
            Bucket = options.Value.Bucket;
            AuthEmail = options.Value.AuthEmail;
            AuthPassword = options.Value.AuthPassword;
        }

        public async Task<IStorageService> CreateStorageServiceAsync()
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail,AuthPassword);

            var storage = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true 
                });

            return new StorageService(storage);
        }
    }
}