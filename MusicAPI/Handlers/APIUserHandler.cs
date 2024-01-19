﻿using MusicAPI.Data;
using MusicAPI.Models.Dtos;
using MusicAPI.Services;
using System.Net;
using System.Text;

namespace MusicAPI.Handlers
{
    public static class APIUserHandler
    {
        public static IResult GetAllUsers(IUserHelper userHelper)
        {
            var users = userHelper.GetAllUsers();
            return Results.Json(users);
        }
        public static IResult GetUser(int userId, IUserHelper userHelper)
        {
            var user = userHelper.GetUser(userId);
            return Results.Json(user);
        }
        public static IResult AddUser(UserDto user, IUserHelper userHelper)
        {

            if (user.Name == null)
            {
                return Results.BadRequest(new { Message = "User needs to have a name" });
            }

            userHelper.AddUser(user);
            
            return Results.StatusCode((int)HttpStatusCode.Created);

        }
        public static IResult ConnectSongToUser(int userId, int songId, IUserHelper userHelper, IArtistHelper artistHelper)
        {
            if (!userHelper.CheckIfUserIdExists(userId))
            {
                return Results.NotFound($"user {userId} not found");
            }
            if (!artistHelper.CheckIfSongIdExists(songId))
            {
                return Results.NotFound($"user {songId} not found");
            }
            userHelper.ConnectSongToUser(userId, songId);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ConnectArtistToUser(int userId, int artistId, IUserHelper userHelper)
        {
            userHelper.ConnectArtistToUser(userId, artistId);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ConnectGenreToUser(int userId, int genreId, IUserHelper userHelper)
        {
            userHelper.ConnectGenreToUser(userId, genreId);
            return Results.StatusCode((int)HttpStatusCode.Created);

        }
    }
}
