# Onion analogy: Working from the inside out.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Sets the current working directory so commands that follow are run from here.
WORKDIR /ShoppingListApi
# Solution and Csproj need to be in their respective locations (must match reference made in .sln).
COPY *.sln .

# Restores the dependencies/tools of the project as distinct layers.
COPY ./ShoppingListApi.Application/*.csproj ShoppingListApi.Application/
COPY ./ShoppingListApi.Domain/*.csproj ShoppingListApi.Domain/
COPY ./ShoppingListApi.Infrastructure/*.csproj ShoppingListApi.Infrastructure/
COPY ./ShoppingListApi.Presentation/*.csproj ShoppingListApi.Presentation/
COPY ./ShoppingListApi.Test/*.csproj ShoppingListApi.Test/
RUN dotnet restore

COPY ./. ./

# ADD BACK BUILD MULTI-STAGE TEST: ALLOWS CACHING - BETTER FOR LOCAL DEV. PIPELINE HAS TO RESTORE ANYWAY SO COMPOSE DOESNT MATTER.
FROM build AS test
WORKDIR /ShoppingListApi/ShoppingListApi.Test
RUN dotnet test

# Copies everything else and builds
FROM build AS publish
WORKDIR /ShoppingListApi
RUN dotnet publish -c Release -o out

# Builds the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY --from=publish ShoppingListApi/out .
ENTRYPOINT ["dotnet", "ShoppingListApi.Presentation.dll"]
