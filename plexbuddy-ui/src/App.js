import React, { useState } from 'react';

function App() {
    const [query, setQuery] = useState('');
    const [movies, setMovies] = useState([]);
    const [view, setView] = useState('search');
    const [wishlist, setWishlist] = useState([]);

    // Logic for search
    const handleSearch = async (e) => {
        e.preventDefault();
        if (!query) return;
        try {
            const response = await fetch(`http://localhost:5000/api/Search/${query}`);
            const data = await response.json();
            setMovies(data);
            setView('search');
        } catch (err) { console.error("Search failed", err); }
    };

    // Logic for loading wishlist
    const loadWishlist = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/Wishlist/all');
            const data = await response.json();
            setWishlist(data);
            setView('wishlist');
        } catch (err) { console.error("Wishlist load failed", err); }
    };

    // Logic for adding to DB
    const addToWishlist = async (movie) => {
        const payload = {
            Title: movie.Title || movie.title,
            TmdbId: (movie.Id || movie.id).toString(),
            PosterUrl: movie.PosterUrl || movie.posterUrl,
            Year: (movie.ReleaseDate || movie.releaseDate || "").split('-')[0],
            PlexUserId: "Admin",
            Status: 0
        };

        try {
            const response = await fetch('http://localhost:5000/api/Wishlist', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });
            if (response.ok) alert("Added to Wishlist!");
        } catch (err) { console.error("Add failed", err); }
    };

    // Logic for removing from DB
    const removeFromWishlist = async (id) => {
        try {
            const response = await fetch(`http://localhost:5000/api/Wishlist/${id}`, { method: 'DELETE' });
            if (response.ok) setWishlist(prev => prev.filter(m => (m.Id || m.id) !== id));
        } catch (err) { console.error("Delete failed", err); }
    };

    // Universal Poster Helper
    const getPoster = (item) => {
        const path = item.PosterUrl || item.posterUrl;
        if (!path) return 'https://via.placeholder.com/500x750?text=No+Poster';
        return path.startsWith('http') ? path : `https://image.tmdb.org/t/p/w500${path}`;
    };

    return (
        <div style={styles.container}>
            <nav style={styles.nav}>
                <button onClick={() => setView('search')} style={{...styles.navTab, color: view === 'search' ? '#e5a00d' : '#666'}}>SEARCH</button>
                <button onClick={loadWishlist} style={{...styles.navTab, color: view === 'wishlist' ? '#e5a00d' : '#666'}}>MY WISHLIST</button>
            </nav>

            <h1 style={styles.logo}>PlexBuddy</h1>

            {view === 'search' ? (
                <>
                    <form onSubmit={handleSearch} style={styles.searchBar}>
                        <input value={query} onChange={(e) => setQuery(e.target.value)} placeholder="Search for movies..." style={styles.input} />
                        <button type="submit" style={styles.searchBtn}>Search</button>
                    </form>
                    <div style={styles.grid}>
                        {movies.map(m => (
                            <div key={m.Id || m.id} style={styles.card}>
                                <img src={getPoster(m)} alt="poster" style={styles.poster} />
                                <h3 style={styles.title}>{m.Title || m.title}</h3>
                                <button onClick={() => addToWishlist(m)} style={styles.addBtn}>+ Wishlist</button>
                            </div>
                        ))}
                    </div>
                </>
            ) : (
                <div style={styles.grid}>
                    {wishlist.map(item => (
                        <div key={item.Id || item.id} style={styles.card}>
                            <img src={getPoster(item)} alt="poster" style={styles.poster} />
                            <h3 style={styles.title}>{item.Title || item.title}</h3>
                            <button onClick={() => removeFromWishlist(item.Id || item.id)} style={styles.removeBtn}>REMOVE</button>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

const styles = {
    container: { backgroundColor: '#111', color: '#e5a00d', minHeight: '100vh', padding: '40px', fontFamily: 'sans-serif' },
    nav: { display: 'flex', justifyContent: 'center', gap: '40px', marginBottom: '20px' },
    navTab: { background: 'none', border: 'none', cursor: 'pointer', fontSize: '1.1rem', fontWeight: 'bold' },
    logo: { textAlign: 'center', fontSize: '3rem', marginBottom: '40px' },
    searchBar: { textAlign: 'center', marginBottom: '50px' },
    input: { padding: '12px', width: '300px', borderRadius: '4px 0 0 4px', border: 'none', outline: 'none' },
    searchBtn: { padding: '12px 24px', backgroundColor: '#e5a00d', color: '#000', border: 'none', borderRadius: '0 4px 4px 0', fontWeight: 'bold', cursor: 'pointer' },
    grid: { display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(220px, 1fr))', gap: '30px' },
    card: { background: '#1a1a1a', padding: '15px', borderRadius: '12px', textAlign: 'center', border: '1px solid #333' },
    poster: { width: '100%', borderRadius: '8px', marginBottom: '15px', boxShadow: '0 4px 10px rgba(0,0,0,0.5)' },
    title: { color: '#fff', fontSize: '1rem', margin: '10px 0', minHeight: '40px' },
    addBtn: { width: '100%', padding: '10px', backgroundColor: '#e5a00d', border: 'none', borderRadius: '6px', fontWeight: 'bold', cursor: 'pointer' },
    removeBtn: { width: '100%', padding: '10px', backgroundColor: '#2a1111', color: '#ff4444', border: '1px solid #ff4444', borderRadius: '6px', fontWeight: 'bold', cursor: 'pointer' }
};

export default App;