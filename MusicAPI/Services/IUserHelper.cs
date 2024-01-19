using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;

namespace MusicAPI.Services
{
    public interface IUserHelper
    {
        public List<User> GetAllUsers();
        public User GetUser(int userId);
        public void AddUser(UserDto userDto);
        public void ConnectSongToUser(int userId, int songId);
        public void ConnectArtistToUser(int userId, int artistId);
        public void ConnectGenreToUser(int userId, int genreId);
        public bool CheckIfUserIdExists(int userId);
    }

    public class UserHelper : IUserHelper
    {
        private ApplicationContext _context;
        public UserHelper(ApplicationContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            List<User> viewUsers = _context.Users.ToList();

            return viewUsers;
        }

        public User GetUser(int userId)
        {
            throw new NotImplementedException();
        }
        public void AddUser(UserDto user)
        {
            _context.Users
                .Add(new User()
                {
                    Name = user.Name,
                });

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Unable to save user to database");
            }

        }


        public void ConnectSongToUser(int userId, int songId)
        {



            throw new NotImplementedException();
        }

        public void ConnectArtistToUser(int userId, int artistId)
        {
            throw new NotImplementedException();
        }

        public void ConnectGenreToUser(int userId, int genreId)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfUserIdExists(int userId)
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == userId);
            
            if (user == null)
                return false;
            
            return true;

        }
    }
}
