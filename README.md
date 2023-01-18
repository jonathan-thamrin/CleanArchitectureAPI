## Development Environment Setup
[![Build status](https://badge.buildkite.com/2033fd54d20609ead54dac78c03afa142d49c74cb6b9da9b22.svg)](https://buildkite.com/myob/jonathant-shoppinglistapi)

### Running the Database and Project
From the root directory, run the following command:
```shell
docker compose up --build
```

The `docker-compose.yaml` file can be found in the root directory. This command will start up the development version of the database and run the project.

The PostgreSQL database can be accessed locally at `localhost:5433` using pgAdmin, pgweb or similar. The credentials to access this database can be found in the `docker-compose.yaml` file as environment variables.

The API can be accessed locally at `http://localhost:8080/api/shoppinglist`.

The Swagger UI can be accessed at `http://localhost:8080/swagger/index.html`.

### Running the Project (Standalone)
This method should only be used if you have your own local database provisioned.

From the root directory, run the following command:
```shell
dotnet run --project ShoppingListApi.Presentation
```

The credentials of the PostgreSQL database can be found at `ShoppingListApi.Presentation/Properties/launchSettings.json`.

The API can be accessed locally at `https://localhost:7148/api/shoppinglist`.

The Swagger UI can be accessed at `https://localhost:7148/swagger/index.html`.
