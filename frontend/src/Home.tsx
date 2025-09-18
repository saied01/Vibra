import React, { useState } from "react";
import { Card, Button, Row, Col } from "react-bootstrap";

interface Artist {
  id: string;
  name: string;
  genres?: string[];
  images?: { url: string; height: number; width: number }[];
  popularity?: number;
  external_urls?: { spotify: string };
  followers?: { total: number; href: string | null };
}

const Home: React.FC = () => {
  const [query, setQuery] = useState("");
  const [artists, setArtists] = useState<Artist[]>([]);

  const handleSearch: () => Promise<void> = async () => {
    if (!query) return;

    try {
      const res = await fetch(`http://localhost:5000/artists/search?name=${encodeURIComponent(query)}`);
      const data = await res.json();

      setArtists(data.artists.items);
    } catch {
      console.error();
    }
  };


  return (
    <div className="container py-4">
      <h1 className="mb-4">Search for artists/bands</h1>

      <div className="input-group mb-4">
        <input
          type="search"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Enter artist name"
          className="form-control"
        />
        <button className="btn btn-primary" onClick={handleSearch}>
          Search
        </button>
      </div>

      <Row xs={1} md={2} lg={3} className="g-4">
        {artists.map((artist) => (
          <Col key={artist.id}>
            <Card className="h-100">
              {artist.images?.[0]?.url && (
                <Card.Img variant="top" src={artist.images[0].url} alt={artist.name} aria-placeholder="artist-img" />
              )}
              <Card.Body>
                <h5 className="card-title">{artist.name}</h5>
                {artist.genres && <p className="card-text mb-1"><strong>Genres:</strong> {artist.genres.join(", ")}</p>}
                {artist.popularity !== undefined && <p className="card-text mb-1"><strong>Popularity:</strong> {artist.popularity}</p>}
                {artist.followers?.total !== undefined && <p className="card-text mb-2"><strong>Followers:</strong> {artist.followers.total.toLocaleString()}</p>}
                {artist.external_urls?.spotify && (
                  <a href={artist.external_urls.spotify} target="_blank" className="btn btn-success">
                    Open in Spotify
                  </a>
                )}
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );

  return (
    <div>
      <h1>Search for artists/bands</h1>
      <input
        type="search"
        value={query}
        onChange={(e) => setQuery(e.target.value)} />
      <Button variant="primary" size="lg" onClick={handleSearch}>Search</Button>
      <ul>
        {artists.map((artist: Artist) => (
          <li>
            <strong>{artist.name}</strong> — {artist.genres?.join(", ")}
          </li>
        ))}
      </ul>
    </div>
  );
};


export default Home
