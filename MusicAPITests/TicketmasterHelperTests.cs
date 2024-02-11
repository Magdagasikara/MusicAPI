using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Repositories;
using MusicAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MusicAPITests
{
    [TestClass]
    public class TicketmasterHelperTests
    {
        [TestMethod]
        public void FindArtistAsync_ReturnsArtistCorrectly()
        {
            // Arrange
            TicketmasterRootAttractionDto artist = new TicketmasterRootAttractionDto
            {
                Embedded = new TicketmasterEmbeddedDto
                {
                    AttractionList = new List<TicketmasterAttractionDto>
                    {
                        new TicketmasterAttractionDto{Id="test-1",Name="test-artist"}
                    }
                }
            };
            string responseString = JsonSerializer.Serialize(artist);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>
                ("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseString)
                })
                ;
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            TicketmasterHelper helper = new TicketmasterHelper(mockClient, "test-key-1");

            // Act
            var result = helper.FindArtistAsync("test-artist");

            // Assert
            Assert.AreEqual(1, result.Result.Embedded.AttractionList.Count);
            Assert.AreEqual("test-1", result.Result.Embedded.AttractionList.First().Id);
            Assert.AreEqual("test-artist", result.Result.Embedded.AttractionList.First().Name);
        }


        // HELP! big, big problems writing this test :(
        // inside of ReturnConcertsForArtistAsync() I have two methods
        // the first one, FindArtistAsync(), should return artists from Ticketmaster - if the artist exists there
        // if the result is no artist the main method, ReturnConcertsForArtistAsync(), should throw an ArtistNotFoundException
        // i.e. this is the inner method that send an Http-request and the outer method that controls the result and throws exception
        // I don't succeed testing the exception
        // How can I tell the test to give null-result from Http but still make the outer method analyze it further?
        // Do I write this test incorrectly or was it incorrect to throw exception in the outer method?
        // /Magda
        [TestMethod]
        [ExpectedException(typeof(ArtistNotFoundException))]
        public void ReturnConcertsForArtistAsync_GivesExceptionIfArtistNotFound()
        {
            // Arrange
            TicketmasterRootAttractionDto artist = new TicketmasterRootAttractionDto
            {
                Embedded = null
            };
            string responseString = JsonSerializer.Serialize(artist);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>
                ("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseString)
                })
                ;
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            TicketmasterHelper helper = new TicketmasterHelper(mockClient, "test-key-1");

            // Act
            var concerts = helper.ReturnConcertsForArtistAsync("artist-should-not-be-found");

        }

        [TestMethod]
        public void FindConcertsByArtistAsync_ReturnsConcertsCorrectly()
        {
            // Arrange
            TicketmasterRootEventDto concerts = new TicketmasterRootEventDto
            {
                Embedded = new TicketmasterEmbeddedDto
                {
                    Events = new List<TicketmasterEventDto>
                    {
                        new TicketmasterEventDto
                        {
                            Name="test-concert",
                            Url="test-link",
                            Dates =new TicketmasterDatesDto
                                    {
                                        Start=new TicketmasterStartDto
                                        {
                                            LocalDate="01-01-test"
                                        }

                            },
                            EmbeddedEvent=new TicketmasterEmbeddedDto
                            {
                                Venues=new List<TicketmasterVenueDto>
                                {
                                    new TicketmasterVenueDto
                                    {
                                        City=new TicketmasterCityDto
                                        {
                                            Name="test-city"
                                        },
                                        Country=new TicketmasterCountryDto
                                        {
                                            Name="test-country"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            string responseString = JsonSerializer.Serialize(concerts);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>
                ("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseString)
                })
                ;
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            TicketmasterHelper helper = new TicketmasterHelper(mockClient, "test-key-1");

            // Act
            var result = helper.FindConcertsByArtistAsync("test-1");

            // Assert
            Assert.AreEqual(1, result.Result.Embedded.Events.Count);
            Assert.AreEqual("01-01-test", result.Result.Embedded.Events.First().Dates.Start.LocalDate);
            Assert.AreEqual("test-link", result.Result.Embedded.Events.First().Url);
            Assert.AreEqual(1, result.Result.Embedded.Events.First().EmbeddedEvent.Venues.Count);
            Assert.AreEqual("test-city", result.Result.Embedded.Events.First().EmbeddedEvent.Venues.First().City.Name);
            Assert.AreEqual("test-country", result.Result.Embedded.Events.First().EmbeddedEvent.Venues.First().Country.Name);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReturnConcertsForArtistAsync_GivesExceptionIfConcertsNotFound()
        {
            // exactly the same problem as for test:
            // ReturnConcertsForArtistAsync_GivesExceptionIfArtistNotFound()

        }
    }
}
