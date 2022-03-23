import React, { createContext, FC, useContext, useEffect, useRef, useState } from "react";
import { GameModel } from "./ApiClient";
import { apiClient, baseUrl } from "./client";

export enum Players {
    Player1,
    Player2
}

export interface Game {
    player: number,
    id: number,
    playerName: string,
    opponentName: string,
    boardStatus: string,
    winner: number,
    nextPlayer: number
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
    nextPlayer: 0
}

const defaultPlayGameState : PlayGame = {
    joinLink: '',
    playing: false,
    turn: false,
}

interface GameContextProps{
    game: Game,
    play: PlayGame,
    updateGame: (state: Game) => void
    updatePlayerName: (name: string) => void
}

const GameContext = createContext<GameContextProps>({
    game: defaultGameState,
    play: defaultPlayGameState,
    updateGame: () => {},
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
    const game = useRef(defaultGameState);
    const [play, setPlay] = useState(defaultPlayGameState);

    useEffect(() => {
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);

        if(urlParams.has("join")){
            const gameId = +(urlParams.get("join") ?? 0);

            const getGame = async () => {
                const response = await apiClient.gamesGET(gameId);
                
                game.current = {
                    ...game.current,
                    player: Players.Player2, 
                    id: gameId,
                    opponentName: response.player1,
                    boardStatus: response.statusString,
                    nextPlayer: 1 + (response.nextPlayer ?? 0)
                }
            } 

            getGame();
        }
    }, [])

    const readStream = (id: number) => {
        const url = `${baseUrl}/api/Games/stream/${id}`; 

        const evtSource = new EventSource(url);

        evtSource.onmessage = handleSSE;
    }

    const handleSSE = (event : MessageEvent) => {
        const model : GameModel = JSON.parse(event.data);
        
        console.log("handle", model, game.current)

        if(game.current.player === Players.Player1 && game.current.opponentName === ''){
            game.current.opponentName = model.player2;
        }

        game.current.boardStatus = model.statusString;
        game.current.winner = model.winner ?? 0;
        
        setPlay({
            ...play, 
            playing: true,
            turn: (game.current.player === Players.Player1 && model.nextPlayer === 0)
            || (game.current.player === Players.Player2 && model.nextPlayer === 1)
        })
        
        console.log("handle", model, game.current)
    }

    const updatePlayerName = async (name: string) => {
        game.current.playerName = name;

        if (game.current.player === Players.Player1) {
            // create a new game
            const response = await apiClient.new({name})
            
            game.current = {
                ...game.current,
                id: response.id,
            }

            setPlay({
                ...play,
                joinLink: `${String(window.location).replace(/\/$/g, '')}?join=${response.id}`
            })

            readStream(response.id);
        }
        else {
            // start the game
            await apiClient.join(game.current.id, {name});

            readStream(game.current.id);
        }

        console.log("game.current", game.current);
    }
    
    const updateGame = (newState : Game) => {
        game.current = {
            ...game.current,
            ...newState
        }
    }

    const value = {
        game : game.current,
        play,
        updateGame,
        updatePlayerName
    }

    return <GameContext.Provider value={value}>
        {children}
    </GameContext.Provider>
}