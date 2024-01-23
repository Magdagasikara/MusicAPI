namespace MusicAPI.Services
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
}
