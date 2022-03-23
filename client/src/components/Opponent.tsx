import { Card } from "@mui/material"
import { FC } from "react";
import { Players, useGame } from "../app/GameContext";

export const Opponent: FC = () => {
    const {game, play} = useGame();
    
    const {playerName, opponentName, player} = game;
    const {turn, joinLink} = play;
    
    const isPlayerConfirmed = (playerNumber : number) => {
        if (playerNumber === player)
        return playerName !== ''
        return opponentName !== ''
    }

    console.log(game, isPlayerConfirmed(Players.Player1), isPlayerConfirmed(Players.Player2), play);

    if(player === Players.Player1 && isPlayerConfirmed(Players.Player1) && !isPlayerConfirmed(Players.Player2))
        return <Card>
            The second player hasn't join the game yet.<br />
            Ask them to join via the following link:
            <a href={joinLink}>{joinLink}</a>.
        </Card>

    if(player === Players.Player2 && !isPlayerConfirmed(Players.Player2))
        return <Card>
            <strong>{opponentName}</strong> invited you to join this Tic-Tac-Toe game.
        </Card>
    
    if(isPlayerConfirmed(Players.Player1) && isPlayerConfirmed(Players.Player2))
        return <Card>
            You are playing against <strong>{opponentName} </strong>.
            <p>It's {turn ? 'your' : 'other'} turn.</p>
        </Card>
        
    return <></>
}