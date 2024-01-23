using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicAPI.Services
{
    public interface IUserHelper
    {
        public List<User> GetAllUsers();
        public UsersViewModel GetUser(int userId);
        public void AddUser(UserDto userDto);
        public void ConnectSongToUser(int userId, int songId);
        public void ConnectArtistToUser(int userId, int artistId);
        public void ConnectGenreToUser(int userId, int genreId);
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

            if (viewUsers == null)
                throw new ArgumentNullException($"No users found");

            return viewUsers;
        }

        public UsersViewModel GetUser(int userId)
        {
            var user = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UsersViewModel
                {
                    Name = u.Name,
                })
                .FirstOrDefault();

            if (user == null)
                throw new ArgumentNullException($"No user with id:{userId} found");

            return user;
        }
        public void AddUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public void ConnectArtistToUser(int userId, int artistId)
        {
            throw new NotImplementedException();
        }

        public void ConnectSongToUser(int userId, int songId)
        {
            throw new NotImplementedException();
        }

        public void ConnectGenreToUser(int userId, int genreId)
        {
            User? user = _context.Users
                .Include(u => u.Genres)
                .SingleOrDefault(u => u.Id == userId);

            if (user == null)
                throw new ArgumentNullException($"User{userId} not found");

            Genre? genre = _context.Genres
                .SingleOrDefault(g => g.Id == genreId);

            if (genre == null)
                throw new ArgumentNullException($"Genre{genreId} not found");

            try
            {
                user.Genres.Add(genre);
                _context.SaveChanges();
            }

            catch 
            {
                throw new Exception($"Unable to connect UserId:{userId} with GenreId{genreId}");
            }
        }
    }
}
