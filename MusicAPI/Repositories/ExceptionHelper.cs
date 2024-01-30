namespace MusicAPI.Repositories
{
    public class UserNotFoundException: Exception
    {
        public override string Message => "User not found";
    }

    public class ArtistNotFoundException : Exception
    {
        public override string Message => "Artist not found";
    }

    public class SongNotFoundException : Exception
    {
        public override string Message => "Song not found";
    }

    public class GenreNotFoundException : Exception
    {
        public override string Message => "Genre not found";
    }

    public class SpotifyGenreNotFoundException : Exception
    {
        public override string Message => "No genre found for spotify searchquery";
    }

    public class SpotifyArtistNotFoundException : Exception
    {
        public override string Message => "No artist found for spotify searchquery";
    }
}
