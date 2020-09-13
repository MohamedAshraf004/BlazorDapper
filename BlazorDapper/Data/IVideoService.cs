using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorDapper.Data
{
    public interface IVideoService
    {
        Task<bool> VideoInsert(Video video);
        Task<IEnumerable<Video>> GetAllVideos();
        Task<IEnumerable<Video>> GetAllVideosActive();
        Task<bool> UpdateVideo(Video video);
        Task<bool> DeleteVideoById(int videoId);
        Task<Video> GetVideoById(int videoId);
    }
}