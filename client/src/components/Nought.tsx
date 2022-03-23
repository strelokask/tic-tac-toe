import { FC } from "react";

export const Nought : FC = () => {
    const width = 100;
    const height = 100;

    return <svg className="nought" width={width} height={height} viewBox="0 0 114 114">
            <circle cx="57" cy="57" r="53.5"/>
        </svg>
}