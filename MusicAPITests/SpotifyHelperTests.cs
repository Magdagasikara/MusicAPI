using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Repositories;
using Moq;
using MusicAPI.Services;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace MusicAPITests
{
    [TestClass]
    public class SpotifyHelperTests
    {
        //Tests that methods throws exception if recieving a badrequest.
        [TestMethod]
        public void GetTop100MostPopularArtists_NotSuccessFullResponse()
        {   
            // Arrange
            var token = "token";

            var spotifyAccHelperMock = new Mock<ISpotifyAccountHelper>();
            var configurationMock = new Mock<IConfiguration>();
            var artistRepositoryMock = new Mock<IArtistRepository>();
            var httpClientMock = new Mock<HttpClient>();

            httpClientMock
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            var spotifyHelperMock = new SpotifyHelper(httpClientMock.Object, spotifyAccHelperMock.Object, configurationMock.Object, artistRepositoryMock.Object);

            spotifyAccHelperMock.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(token);

            configurationMock.Setup(x => x["Spotify:ClientId"]).Returns("clientId");
            configurationMock.Setup(x => x["Spotify:ClientSecret"]).Returns("clientSecret");

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);

            // Act & Assert
            Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await spotifyHelperMock.GetTop100MostPopularArtists());
        }

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
