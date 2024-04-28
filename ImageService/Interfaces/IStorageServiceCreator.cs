namespace ImageService.Interfaces
{
    public interface IStorageServiceCreator
    {
        Task<IStorageService> CreateStorageService();
    }
}