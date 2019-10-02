import axios from 'axios';

export const headerFactory = (token) => {
  return { headers: { "Authorization": `Bearer ${token}`, "Content-Type": "application/json" } }
}

const api = axios.create({
    baseURL: 'https://rmauro-marvel-api.herokuapp.com'
});

export default api;