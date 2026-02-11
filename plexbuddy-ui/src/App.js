import React, { useState } from 'react';
import './App.css';

const API_BASE = "http://localhost:5000/api";

function App() {
    const [query, setQuery] = useState('');
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(false);

    // 1. Logic to search TMDB via your .NET API
    const searchMovies = async (e) => {
        if (e) e.preventDefault();
        if (!query) return;

        setLoading(true);
        try {
            const response = await fetch(`${API_BASE}/search/${query}`);
            const data = await response.json();
            setMovies(data);
        } catch (err) {
            console.error("Search failed:", err);
        } finally {
            setLoading(false);
        }
    };

    // 2. Logic to POST a request to your WishlistController
    const addToWishlist = async (movie) => {
        const newItem = {
            plexUserId: "Guest_User", // We will replace this with real Plex Auth later
            title: movie.title,
            tmdbId: movie.id.toString(),
            posterUrl: movie.posterUrl,
            year: movie.releaseDate,
            status: 0 // 0 matches our 'Wishlist' Enum in C#
        };

        try {
            const res = await fetch(`${API_BASE}/wishlist`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newItem)
            });

            if (res.ok) {
                alert(`🚀 ${movie.title} added to wishlist!`);
            } else {
                const errorText = await res.text();
                alert(errorText || "Already in wishlist!");
            }
        } catch (err) {
            alert("Backend is offline!");
        }
    };

    return (
        <div className="app-container">
            <nav className="navbar">
                <h1 className="logo">Plex<span>Buddy</span></h1>
                <form onSubmit={searchMovies} className="search-form">
                    <input
                        type="text"
                        placeholder="Search for movies or shows..."
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                    />
                    <button type="submit">{loading ? '...' : 'Search'}</button>
                </form>
            </nav>

            <main className="results-grid">
                {movies.map(movie => (
                    <div key={movie.id} className="movie-card">
                        <div className="poster-wrapper">
                            <img src={movie.posterUrl} alt={movie.title} />
                            <div className="overlay">
                                <button onClick={() => addToWishlist(movie)}>Add to Wishlist</button>
                            </div>
                        </div>
                        <div className="movie-meta">
                            <h3>{movie.title}</h3>
                            <span>{movie.releaseDate}</span>
                        </div>
                    </div>
                ))}
            </main>
        </div>
    );
}

export default App;