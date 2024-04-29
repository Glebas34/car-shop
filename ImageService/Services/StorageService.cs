using Firebase.Storage;
using ImageService.Dtos;
using ImageService.Entities;
using ImageService.Interfaces;
namespace ImageService.Services
{
    public class StorageService: IStorageService
    {
        private readonly FirebaseStorage _storage;
        public StorageService(FirebaseStorage storage)
        {
            _storage = storage;
        }
        public async Task<Image> UploadAsync(CreateImage createImage, Guid carPageId, bool isMain)
        {
            var file = createImage.Image;
            var fileBytes = new byte[file.Length];
            file.OpenReadStream().Read(fileBytes, 0, int.Parse(file.Length.ToString()));
            var stream = new MemoryStream(fileBytes);
            string root;
            if(isMain)
            {
                root = $"CarImages/{carPageId}/MainImage/{file.FileName}";     
            }
            else
            {
                root = $"CarImages/{carPageId}/{file.FileName}"; 
            }

            FirebaseStorageReference reference = _storage.Child(root);

            var cancellation = new CancellationTokenSource();

            var task = await reference.PutAsync(stream, cancellation.Token);

            var image = new Image
            {
                Id = new Guid(),
                Url = task,
                CarPageId = createImage.CarPageId,
                Root = root,
                IsMain = isMain
            };

            return image;
        }
        public async Task DeleteAsync(string root)
        {
             await _storage.Child(root).DeleteAsync();
        }
    }
}