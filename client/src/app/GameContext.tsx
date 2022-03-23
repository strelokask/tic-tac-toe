import React, { createContext, FC, useCallback, useContext, useEffect, useRef, useState } from "react";
import { GameModel } from "./ApiClient";
import { apiClient, baseUrl } from "./client";

export enum Players {
    Player1 = 1,
    Player2
}

export interface Game {
    player: number,
    id: number,
    playerName: string,
    opponentName: string,
    boardStatus: string,
    nextPlayer: number,
    winner: number,
    gameOver: boolean;
}

export interface PlayGame {
    joinLink: string,
    playing: boolean,
    turn: boolean,
}

const defaultGameState : Game = {
    player: Players.Player1,
    id: 0,
    playerName: '',
    opponentName: '',
    boardStatus: '         ',
    winner: 0,
    nextPlayer: 0,
    gameOver: false
}

const defaultPlayGameState : PlayGame = {
    joinLink: '',
    playing: false,
    turn: false,
}

interface GameContextProps{
    game: Game,
    play: PlayGame,
    playerName: string,
    updateBoard: (board: string) => void
    updatePlayerName: (name: string) => void
}

const GameContext = createContext<GameContextProps>({
    game: defaultGameState,
    play: defaultPlayGameState,
    playerName: '',
    updateBoard: () => {},
    updatePlayerName: () => {}
});


export const useGame = () : GameContextProps => {
    const context = useContext(GameContext)
    if (context === undefined) {
      throw new Error('useGame must be used within a GameProvider')
    }
    return context;
}

export const GameProvider : FC = ({children}) => {
    const gameRef = useRef(defaultGameState);
    const [play, setPlay] = useState(defaultPlayGameState);
    const [playerName, setPlayerName] = useState('');

    
    const handleSSE = useCallback((event : MessageEvent) => {
        const model : GameModel = JSON.parse(event.data);

        if(gameRef.current.player === Players.Player1 && gameRef.current.opponentName === ''){
            gameRef.current.opponentName = model.player2;
        }

        gameRef.current.boardStatus = model.statusString;
        gameRef.current.winner =  model.winner ?? 0;
        gameRef.current.gameOver = model.winner !== 0 || !model.statusString.includes(' '); 
        
        setPlay({
            joinLink: '',
            playing: true,
            turn: (gameRef.current.player === Players.Player1 && model.nextPlayer === 0)
            || (gameRef.current.player === Players.Player2 && model.nextPlayer === 1)
        })
    }, [])

    const readStream = useCallback(() => {
        const url = `${baseUrl}/api/Games/stream/${gameRef.current.id}`; 

        const evtSource = new EventSource(url);

        evtSource.onmessage = handleSSE;
    }, [handleSSE])


    useEffect(() => {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);

        if(urlParams.has("join")){
            const gameId = +(urlParams.get("join") ?? 0);

            const getGame = async () => {
                const response = await apiClient.games(gameId);
                
                gameRef.current = {
                    ...gameRef.current,
                    player: Players.Player2,
                    playerName: response.player2,
                    id: gameId,
                    opponentName: response.player1,
                    boardStatus: response.statusString,
                    nextPlayer: response.nextPlayer
                }

                setPlay({...defaultPlayGameState, playing: response.nextPlayer === Players.Player2, turn: response.nextPlayer === Players.Player2})

                if(response.player2 !== ''){
                    readStream()
                }
            } 

            getGame();
        }
    }, [readStream])

    const updatePlayerName = async (name: string) => {
        gameRef.current.playerName = name;

        setPlayerName(name);

        if (gameRef.current.player === Players.Player1) {
            // create a new game
            const response = await apiClient.new({name})
            
            gameRef.current.id = response.id

            setPlay({
                ...play,
                joinLink: `${String(window.location).replace(/\/$/g, '')}?join=${response.id}`
            })

        }
        else {
            // start the game
            await apiClient.join(gameRef.current.id, {name});

            setPlay({...play, playing: true});
        }

        readStream();
    }
    
    const updateBoard = (status: string) => {
        gameRef.current.boardStatus = status;
    }

    const value = {
        game : gameRef.current,
        play,
        playerName,
        updateBoard,
        updatePlayerName
    }

    return <GameContext.Provider value={value}>
        {children}
    </GameContext.Provider>
}