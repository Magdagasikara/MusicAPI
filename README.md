# MusicAPI

# Instructions
* Create a new database
  * Paste your connection string into appsettings.Development.json
  * Go to Package Manager Console and run Update-Database
* To access all functionality you need a Client ID and a Client secret from Spotify:
  * Go to https://developer.spotify.com/
  * Log in
  * Click at your profile name and choose Dashboard
  * Create a new Web API with Website: https://localhost:7181/ and Redirect URL: https://localhost:7181/
  * Go to your new app, choose Settings and copy your Client ID and Client secret
  * Paste them into appsettings.Development.json 
* For Ticketmaster, feel free to use wGQCTHlO5PUQmB0pmowa8ezLAWlAUwTm in your appsettings

# Rest-API requests
- Get all users of the system:   
GET https://localhost:7181/user/
      
- Get all genres connected to a specific user:  
GET https://localhost:7181/user/magda/genre/
      
- Get all artists connected to a specific user:  
GET https://localhost:7181/user/magda/artist/
      
- Get all songs connected to a specific user:  
GET https://localhost:7181/user/magda/song/
      
- Connect a user with a new genre:  
POST https://localhost:7181/user/magda/genre/2
      
- Connect a user with a new artist:  
POST https://localhost:7181/user/magda/artist/10
      
- Connect a user with a new song:  
POST https://localhost:7181/user/magda/song/2

# Endpoints for external APIs
- Spotify: get and save in Db top 10 songs for artists on Spotify's top 100 list of all time  
POST https://localhost:7181/spotify/Top100sTop10

- Spotify: get and save 50 songs for a chosen artist + add a description of the artist:  
POST [https://localhost:7181/spotify/Top50Songs/rammstein/desc/Konstigt tyskt band](url)

- Ticketmaster: show upcoming concerts for a chosen artist, if found at Ticketmaster:  
GET https://localhost:7181/ticketmaster/behemoth  
GET [https://localhost:7181/ticketmaster/iron maiden](url)

**Extra challenges (optional)**
- Return a hierarchical JSON for all user's favrourite artists, genres and songs:  
GET https://localhost:7181/user/magda

- Let user filter results, for exempel by typing "to" to get "tomas" and "totoro"
- Use pagination. Let user see certain amount of artists/genres/songs per page:  
GET https://localhost:7181/artist/?name=Sol&pageNumber=1&amountPerPage=10  
GET https://localhost:7181/song/?name=love&pageNumber=1&amountPerPage=10  
GET https://localhost:7181/genre/?name=polish&pageNumber=1&amountPerPage=66  
  
