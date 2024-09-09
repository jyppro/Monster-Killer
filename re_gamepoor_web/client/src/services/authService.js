import axios from 'axios';

const API_URL = '/api/auth/';

export const signup = async (username, email, password) => {
  try {
    const response = await axios.post(`${API_URL}signup`, {
      username,
      password
    });
    return response.data;
  } catch (error) {
    throw error.response.data;
  }
};
