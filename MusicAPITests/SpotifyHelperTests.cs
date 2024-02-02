using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MusicAPI.Services;
using Castle.Core.Configuration;

namespace MusicAPITests
{
    [TestClass]
    public class SpotifyHelperTests
    {
        [TestMethod]
        public void AddArtistsGenresAndTracksFromSpotify_SuccessfullyAddsArtistAndTracks()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "ArtistsAndAlotOfTracks")
                .Options;

            ApplicationContext context = new ApplicationContext(dbContextOptions);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["Spotify:ClientId"]).Returns("yourClientId");
            configurationMock.Setup(x => x["Spotify:ClientSecret"]).Returns("yourClientSecret");

            var spotifyHelperMock = new Mock<ISpotifyHelper>();
            spotifyHelperMock.Setup(x => x.SaveArtistGenreAndTrackFromSpotifyToDb())

    }
}
