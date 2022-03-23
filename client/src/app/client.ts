import { Client } from './ApiClient';

export const baseUrl = process.env.REACT_APP_API_URL;

export const apiClient = new Client(process.env.REACT_APP_API_URL);
