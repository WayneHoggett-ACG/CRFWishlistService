FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:b56053d0a8f4627047740941396e76cd9e7a9421c83b1d81b68f10e5019862d7 AS build
WORKDIR /App
COPY . ./
RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0.16@sha256:c149fe7e2be3baccf3cc91e9e6cdcca0ce70f7ca30d5f90796d983ff4f27bd9a
WORKDIR /App
COPY --from=build /App/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "CRFWishlistService.dll"]