import React, { useEffect, useState } from 'react';
import logo from './../logo.svg';
import { GameModel } from './ApiClient';
import './App.css';
import { apiClient } from './client';

const App : React.FC = () => {
  const [games, setGames] = useState<GameModel[]>([]);
  console.table(games);
  useEffect(() => {
    async function fetchMyAPI() {
      const response = await apiClient.gamesAll();
      setGames(response)
    }

    fetchMyAPI()
  }, [])
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
