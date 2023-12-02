using CarShop.Models;

namespace CarShop.Services.Interfaces
{
    public interface IPostsService
    {
        PostModel Create(PostModel model);

        PostModel Update(PostModel model);

        PostModel Get(int id);

        List<PostModel> GetAll();

        void Delete(int id);
    }
}
