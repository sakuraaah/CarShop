using CarShop.Models;

namespace CarShop.Data
{
    public class PostsDataContext
    {
        public List<PostModel> Posts { get; set; }

        public PostsDataContext()
        {
            Posts = new List<PostModel>();
        }
    }
}
