namespace MyMusicApp.Repositories
{
    public interface IAlbumRepository
    {
        Task<List<Album>> GetAllAsync();
        Task<Album> GetByIdAsync(int id);
        Task AddAsync(Album album);
        Task UpdateAsync(Album album);
        Task<bool> DeleteAsync(int id);
        Task InitializeDataAsync();
        Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId);  // Nuevo método
    }
}
