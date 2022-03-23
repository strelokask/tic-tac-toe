import { FC, useCallback } from "react";
import { Cross } from "./Cross";
import { Nought } from "./Nought";

interface BoardCellProps {
    status: string;
    playerTurn: boolean;
    handleClick: (i: number, j:number) => void;
    i: number;
    j: number;
}
export const BoardCell : FC<BoardCellProps> = ({status, playerTurn, handleClick, i, j}) => {
    const cellClick = useCallback(() => handleClick(i, j), [handleClick, i, j]);

    if(status === 'X')
        return <Cross />
    
    if(status === 'O')
        return <Nought />
    
    return <svg 
        onClick={cellClick}
        className={"board-cell empty " + (playerTurn ? "active" : "")}
    />
}