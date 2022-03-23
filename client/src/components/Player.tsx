import { Card, TextField } from '@mui/material'
import React, { FC, useState } from 'react'
import { useGame } from '../app/GameContext';

export const Player: FC = () => {
  const {game, updatePlayerName} = useGame();
  
  const [name, setName] = useState(game.playerName);
  
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const inputName = e.target.value;
    setName(inputName)
  }

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    updatePlayerName(name);
  }

  return <Card sx={{p: 2}}>
    {game.playerName ? <>
      Hello {game.playerName}
    </> : <>
      <form onSubmit={handleSubmit}>
        <TextField
          required
          label="Enter your name"
          value={name}
          onChange={handleChange}
        />
      </form>
    </>}
    

  </Card>
}
