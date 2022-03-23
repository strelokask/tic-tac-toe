import { FC } from "react";

export const Cross : FC = () => {
    const width = 100;
    const height = 100;

    return <svg className="cross" width={width} height={height} viewBox="0 0 100 100">
            <path d="M1 1 L99 99"/>
            <path d="M99 1 L1 99"/>
        </svg>
}