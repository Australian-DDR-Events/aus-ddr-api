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
6. 
  - Set your launch configuration to `Api Local`

OR 
  - From the `solution root` run `docker-compose --env-file ./.local.env up -d`
7. Once all that is done, you can navigate to localhost:5001/graphql to see the graphql page.
9. Run the below command to get the service IP address to connect to the database via pgAdmin
   ```
   docker inspect aus-ddr-events-db -f "{{json .NetworkSettings.Networks }}"
   ```
