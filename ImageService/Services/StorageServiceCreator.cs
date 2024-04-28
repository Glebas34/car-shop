using Firebase.Storage;
using Firebase.Auth;
using ImageService.Interfaces;
namespace ImageService.Services
{
    public class StorageServiceCreator: IStorageServiceCreator
    {
        private static string ApiKey = "AIzaSyAWuwHs11juhaKYDJvlqC2xPwt_PvhXDTE";
        private static string Bucket = "car-shop-e3217.appspot.com";
        private static string AuthEmail = "gleb@mail.ru";
        private static string AuthPassword = "123456";
        public async Task<IStorageService> CreateStorageService()
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail,AuthPassword);

            var storage = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                });

            return new StorageService(storage);
        }
    }
}