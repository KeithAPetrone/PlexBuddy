# 📽️ PlexBuddy

A full-stack media management tool to search for movies and manage a personal wishlist. Built with a .NET 8 API and a React frontend.

## 🚀 Features
- **TMDB Search**: Live movie search using the TMDB API.
- **Wishlist Management**: Add movies to a persistent SQLite database.
- **Admin Controls**: Approve requests or remove items from the list.
- **Responsive Design**: Dark-themed, Plex-inspired UI.

## 🛠️ Tech Stack
- **Frontend**: React (Hooks, CSS-in-JS)
- **Backend**: ASP.NET Core Web API
- **Database**: Entity Framework Core + SQLite
- **API**: TMDB (The Movie Database)

## 📦 Setup & Installation

### Backend
1. Navigate to the `Api` folder.
2. Update your `appsettings.json` with your TMDB API Key.
3. Run `dotnet run` or start the project in Rider/Visual Studio.

### Frontend
1. Navigate to the `client` folder.
2. Run `npm install`.
3. Run `npm start`.