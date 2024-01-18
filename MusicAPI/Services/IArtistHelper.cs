﻿using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;

namespace MusicAPI.Services
{
    public interface IArtistHelper
    {
        public void AddArtist(ArtistDto artistDto, int genreId);
        public void AddSong(SongDto songDto, int artistId);
        public void AddGenre(GenreDto genreDto);
        public List<ArtistsViewModel> GetArtists(int userId);
        public List<GenresViewModel> GetGenres(int userId);
        public List<SongsViewModel> GetSongs(int userId);
    }

    public class ArtistHelper : IArtistHelper
    {
        private ApplicationContext _context;
        public ArtistHelper(ApplicationContext context)
        {
            _context = context;
        }

        public void AddGenre(GenreDto genreDto)
        {
            throw new NotImplementedException();
        }

        public void AddArtist(ArtistDto artistDto, int genreId)
        {
            throw new NotImplementedException();
        }

        public void AddSong(SongDto songDto, int artistId)
        {
            throw new NotImplementedException();
        }

        public List<ArtistsViewModel> GetArtists(int userId)
        {
            throw new NotImplementedException();
        }

        public List<GenresViewModel> GetGenres(int userId)
        {
            throw new NotImplementedException();
        }

        public List<SongsViewModel> GetSongs(int userId)
        {
            throw new NotImplementedException();
        }
    }
}