import { Card } from "@mui/material";
import { FC, useEffect, useState } from "react";
import { MoveDto } from "../app/ApiClient";
import { apiClient } from "../app/client";
import { Players, useGame } from "../app/GameContext";
import { replaceAt } from "../utils";
import { BoardCell } from "./BoardCell";

export const Board : FC = () => {
    const {game, play, updateBoard} = useGame();
    
    const {boardStatus, id, player, playerName} = game;
    const {playing, turn} = play;

    const [cells, setSells] = useState<string[][]>([]);

    useEffect(() => {
        let board : string [][] = [];
        const n = 3;
        for(let i=0; i < n; i++){
            let row = [];
            for(let j=0; j < n; j++){
                row.push(boardStatus[n * i + j]);
            }
            board.push(row);
        }

        setSells(board);
    }, [boardStatus])

    const handleClick = async (i: number, j: number) => {
        if(turn){
            const model : MoveDto = {
                player: playerName,
                i, j
            }
            
            await apiClient.move(id, model);

            const status = replaceAt(boardStatus, 3 * i + j, player === Players.Player1 ? 'X' : 'O');

            updateBoard(status);
        }
    }

    if(playing)
        return <Card>
            <div className="board">
                {cells.map((row, i) => row.map((cell, j) => 
                    <BoardCell 
                        status={cell} 
                        playerTurn={turn} 
                        handleClick={handleClick}
                        i={i} j ={j}
                        key={3 * i + j}
                    />)
                )}
            </div>
        </Card>

    return <></>
}