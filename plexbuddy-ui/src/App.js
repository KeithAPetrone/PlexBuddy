import React, { useState } from 'react';

function App() {
  const [search, setSearch] = useState('');
  
  return (
    <div style={{ padding: '40px', fontFamily: 'sans-serif', backgroundColor: '#1a1d21', color: 'white', minHeight: '100vh' }}>
      <header>
        <h1 style={{ color: '#e5a00d' }}>PlexBuddy</h1>
        <p>Your personal media assistant.</p>
      </header>
      <div style={{ marginTop: '20px' }}>
        <input 
          type="text" 
          placeholder="Search for a movie..." 
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          style={{ padding: '10px', width: '300px', borderRadius: '4px', border: 'none' }}
        />
        <button style={{ padding: '10px 20px', marginLeft: '10px', backgroundColor: '#e5a00d', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
          Search
        </button>
      </div>
    </div>
  );
}
export default App;