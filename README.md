# MusicAPI

# Instruktioner
* du behöver en Client ID och Client secret från Spotify för att kunna använda all fuktionalitet:
  * Gå till https://developer.spotify.com/
  * Logga in
  * Klicka på ditt profilnamn och välj Dashboard
  * Skapa en ny Web API med Website: https://localhost:7181/ och Redirect URL: https://localhost:7181/
  * Gå till din nya app, välj Settings och hämta din Client ID och Client secret
  * Klistra in dem i appsettings.Development.json i delen som gäller Spotify

# Rest-API anrop:

- Hämta alla personer i systemet  
GET https://localhost:7181/user/
      
- Hämta alla genrer som är kopplade till en specifik person  
GET https://localhost:7181/user/magda/genre/
      
- Hämta alla artister som är kopplade till en specifik person  
GET https://localhost:7181/user/magda/artist/
      
- Hämta alla låtar som är kopplade till en specifik person  
GET https://localhost:7181/user/magda/song/
      
- Koppla en person till en genre  
POST https://localhost:7181/user/magda/genre/2
      
- Koppla en person till en artist  
POST https://localhost:7181/user/magda/artist/10
      
- Koppla en person till en låt  
POST https://localhost:7181/user/magda/song/2

Skapa passande endpoint för funktionalitet ni lagt till ovanför t.ex. för att hämta rekommenderade artister baserat på en användares gillade artister  
- Spotify: hämta och spara i Db top 10 låtar för artister på Spotify:s top 100 lista genom tiderna  
POST https://localhost:7181/spotify/Top100sTop10

- Spotify: hämta och spara 50 låtar för en vald artist + lägg till artistens beskrivning  
POST [https://localhost:7181//spotify/Top50Songs/rammstein/desc/Konstigt tyskt band](url)
  

**Extra utmaning (gör om du vill)**

- Ge möjlighet till den som anropar APIet och efterfrågar en person att direkt få ut alla genres och alla artister för den personen direkt i en hierarkisk JSON-fil  
GET https://localhost:7181/user/magda

- Ge möjlighet för den som anropar APIet att filtrera vad den får ut, som en sökning. Exempelvis som jag skickar med “to” till hämtning av alla personer vill jag ha de som har ett “to” i namnet så som “tobias” eller “tomas”. Detta kan du sen skapa för alla anropen om du vill.  
- skapa paginering av anropen. När jag anropar exempelvis låtar får jag kanske de första 100 låtarna och får sen anropa ytterligare gånger för att få fler. Här kan det också vara snyggt att anropet avgör hur många personer jag får i ett anrop så jag kan välja att få säg 10st om jag bara vill ha det.  
GET https://localhost:7181/artist/?name=Sol&pageNumber=1&amountPerPage=10  
GET https://localhost:7181/song/?name=love&pageNumber=1&amountPerPage=10  
GET https://localhost:7181/genre/?name=polish&pageNumber=1&amountPerPage=66  
  
