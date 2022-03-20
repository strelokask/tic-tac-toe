import { Client } from './ApiClient';

export const apiClient = new Client(
    process.env.REACT_APP_API_URL
)
