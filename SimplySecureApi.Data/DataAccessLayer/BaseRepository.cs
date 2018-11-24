using SimplySecureApi.Data.DataContext;

namespace SimplySecureApi.Data.DataAccessLayer
{
    public abstract class BaseRepository
    {
        protected SimplySecureDataContext DataContext;

        protected BaseRepository()
        {
        }
    }
}