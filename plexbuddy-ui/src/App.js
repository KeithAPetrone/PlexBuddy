import React, { useState } from 'react';

function App() {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(false);

    const handleSearch = async (e) => {
        e.preventDefault();
        if (!query) return;
        setLoading(true); // Trigger loading state for button feedback
        try {
            const response = await fetch(`http://localhost:5000/api/Search/${query}`);
            const data = await response.json();
            setResults(data);
        } catch (err) {
            console.error("Search failed:", err);
        } finally {
            setLoading(false); // Stop loading regardless of success/fail
        }
    };

    return (
        <div style={styles.container}>
            <h1 style={styles.logo}>PlexBuddy</h1>

            <form onSubmit={handleSearch} style={styles.searchForm}>
                <input
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    placeholder="Search movies..."
                    style={styles.input}
                />
                <button type="submit" style={styles.btn}>
                    {loading ? '...' : 'Search'}
                </button>
            </form>

            <div style={styles.grid}>
                {results.map(item => (
                    <div key={item.id} style={styles.card}>
                        {/* The Badge is positioned 'absolute' relative to the card */}
                        <span style={styles.badge}>{item.mediaType?.toUpperCase()}</span>

                        {/* Poster wraps in a link. 'target="_blank"' opens IMDb in a new tab */}
                        <a href={item.imdbUrl} target="_blank" rel="noopener noreferrer" style={styles.posterLink}>
                            <div style={styles.posterArea}>
                                {/* Fallback text that stays hidden behind the image unless the image fails */}
                                <div style={styles.placeholder}>NO POSTER</div>
                                {item.posterUrl && (
                                    <img
                                        src={`https://image.tmdb.org/t/p/w500${item.posterUrl}`}
                                        alt="poster"
                                        style={styles.img}
                                        // If the image link is broken (404), we hide it to show the placeholder text
                                        onError={(e) => e.target.style.display = 'none'}
                                    />
                                )}
                            </div>
                        </a>

                        <div style={styles.info}>
                            <p style={styles.title}>
                                {item.title}
                                {/* Logic: Only render parentheses if item.year is truthy (not null/empty) */}
                                {item.year && <span style={styles.year}> ({item.year})</span>}
                            </p>
                            <button style={styles.addBtn}>+ Add to Wishlist</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

const styles = {
    container: { backgroundColor: '#0a0a0a', color: '#e5a00d', minHeight: '100vh', padding: '40px 20px', fontFamily: 'sans-serif' },
    logo: { textAlign: 'center', fontSize: '3.5rem', margin: '0 0 30px 0', fontWeight: '900', letterSpacing: '-2px' },
    searchForm: { display: 'flex', justifyContent: 'center', marginBottom: '50px' },
    input: { padding: '12px', width: '300px', borderRadius: '8px 0 0 8px', border: 'none', background: '#1a1a1a', color: '#fff' },
    btn: { padding: '12px 20px', background: '#e5a00d', border: 'none', borderRadius: '0 8px 8px 0', cursor: 'pointer', fontWeight: 'bold' },

    // Grid uses 'auto-fill' to automatically decide how many cards fit per row based on screen width
    grid: { display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '30px' },

    // 'height' is fixed to ensure all cards align perfectly even with different title lengths
    card: { background: '#111', borderRadius: '12px', overflow: 'hidden', border: '1px solid #222', display: 'flex', flexDirection: 'column', height: '440px', position: 'relative' },

    posterArea: { height: '300px', backgroundColor: '#161616', display: 'flex', alignItems: 'center', justifyContent: 'center', position: 'relative' },
    placeholder: { color: '#333', fontWeight: 'bold', fontSize: '12px', position: 'absolute' },
    img: { width: '100%', height: '100%', objectFit: 'cover', zIndex: 2, position: 'relative' },
    badge: { position: 'absolute', top: '10px', left: '10px', background: '#e5a00d', color: '#000', fontSize: '10px', padding: '3px 6px', borderRadius: '4px', fontWeight: 'bold', zIndex: 5 },

    // 'flexGrow: 1' makes the info section fill the remaining space in the card
    info: { padding: '15px', flexGrow: 1, display: 'flex', flexDirection: 'column', justifyContent: 'space-between' },
    title: { fontSize: '14px', color: '#fff', margin: '0', fontWeight: '600' },
    year: { color: '#666', fontWeight: '400' },
    addBtn: { width: '100%', padding: '10px', background: '#e5a00d', border: 'none', borderRadius: '6px', cursor: 'pointer', fontWeight: 'bold' }
};

export default App;