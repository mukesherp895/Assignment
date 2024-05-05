#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5224


FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Assignment.WebApp/Assignment.WebApp.csproj", "Assignment.WebApp/"]
COPY ["Assignment.Common/Assignment.Common.csproj", "Assignment.Common/"]
COPY ["Assignment.Model/Assignment.Model.csproj", "Assignment.Model/"]
RUN dotnet restore "./Assignment.WebApp/Assignment.WebApp.csproj"
COPY . .
WORKDIR "/src/Assignment.WebApp"
RUN dotnet build "./Assignment.WebApp.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Assignment.WebApp.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assignment.WebApp.dll"]