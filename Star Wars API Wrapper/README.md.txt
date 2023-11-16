The API has been deployed to a cloud service provider and can be accessed using the following link: http://starwarsapiwrapper.azurewebsites.net/

Alternatively, the 'Star Wars API Wrapper.sln' can be opened in Visual Studio and can be built and deployed from there.


Description of the architecture:

1. Models - c# classes, like the 'people' class, that represent the structure of the data that is being sent or received in API requests and responses.
2. Service Layer - the 'filmService' class contains only the logic that is required to interact with the external API in order to keep the code maintainable.
3. Controller - handles the incoming HTTP requests and contain methods that execute the logic for those requests. 'getAllFilmsController' is one of the controllers that handle requests related to the external API.

Explanation of design decisions:

1. Caching - All the controllers use '_memoryCache' to store and retrieve data for requests. This will improve performance since redundant calls to the API will be avoided. 
2. Error Handling - All methods include try-catch blocks to handle exceptions. This is to ensure that unexpected errors are caught and handled. If an exception occurs, a 500 internal server error is returned as well as a meaningful message.
3. Response Formatting - The list of films are formatted by concatenating the episode_id and title so that meaningful information is returned. 
4. Response Formatting - The list of films, characters in a film and starships in a film are formatted to return only the names of the films, characters and starships respectively. This ensures that meaningful, readable information is returned. 
5. Response Codes - Appropriate HTTP status codes are returned in the responses. If data is not found then a 404 Not Found status is returned but if there is an internal server error then a 500 Internal Server Error status is returned. This aids in finding the error
6. Timeout for Cache - Any and all data stored in the cache has a timeout of 5 minutes to ensure that any updates or changes in the data is reflected accordingly.






