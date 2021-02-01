# Australian DDR Events API

This service handles score submissions, DDR dancer information, ingredients. badges and etc.

## Getting started

1. Install Visual Studio

2. Make sure you have .NET Core 5 downloaded
   
   https://dotnet.microsoft.com/download/dotnet-core

3. Download Docker

   https://docs.docker.com/get-docker/

4. Clone this git repository
5. Open it in Visual Studio
6. Set up the enviroment variables used in `docker-compose.yml`
6. In a terminal, change directory to the git folder and run the docker-compose
   ```
   cd <PATH_TO>/aus-ddr-api
   docker-compose up
   ```
7. Run database migration in the terminal on Visual Studio
    ```
    dotnet ef database update
    ```

8. Once all that is done, you can navigate to localhost:1236 to see the pdAdmin portal. Log in as the details you put in the environment variables given to the docker-compose.
9. Run the below command to get the service IP address to connect to the database via pgAdmin
   ```
   docker inspect aus-ddr-events-db -f "{{json .NetworkSettings.Networks }}"
   ```
