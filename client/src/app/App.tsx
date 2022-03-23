import React from 'react'
import { Player } from '../components/Player'
import './App.css';
import { Opponent } from '../components/Opponent';
import { GameProvider } from './GameContext';
import { Board } from '../components/Board';
import { createTheme, ThemeProvider } from '@mui/material';
import { GameOver } from '../components/GameOver';

const theme = createTheme({
  components:{
    MuiPaper:{
      styleOverrides:{
        root:{
          margin: 20,
          padding: 15,
          border: '1px solid whitesmoke',
        }
      }
    }
  }
});

const App : React.FC = () => {
  return (
    <div className="app">
      <header className="app-header">
      </header>
      <div className="app-content">
        <ThemeProvider theme={theme}>
          <GameProvider>
            <Player />
            <Opponent />
            <Board />
            <GameOver />
          </GameProvider>
        </ThemeProvider>
      </div>
    </div>
  )
}

export default App
