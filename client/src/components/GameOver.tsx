import { FC } from "react";
import { useGame } from "../app/GameContext";

export const GameOver : FC = () => {
    const {game} = useGame();
    
    const {gameOver, winner, player} = game;
    if (gameOver) {
        if(winner === 0) return <h1>Draw</h1>
        else if(winner === player) return <h1>You win!</h1>
        else return <h1>You lose</h1>
    }

    return <></>
}