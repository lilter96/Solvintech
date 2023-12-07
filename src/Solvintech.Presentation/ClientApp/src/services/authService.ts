import axios, { AxiosResponse } from 'axios';
import { AuthResponse } from '../interfaces/AuthResponse';
import { RefreshTokenResponse } from '../interfaces/RefreshTokenResponse';
import { UserTokens } from '../interfaces/UserTokens';
import { authStore } from '../stores/authStore';

const API_BASE_URL = process.env.API_BASE_URL || 'https://localhost:44397';

class AuthService {
  constructor() {
    axios.defaults.baseURL = API_BASE_URL;
    axios.interceptors.request.use((config) => {
        const token = authStore.currentUser?.tokens.accessToken;
        if (token) {
          config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
      }, (error) => {
        return Promise.reject(error);
      });
      
  }

  async register(email: string, password: string): Promise<AuthResponse> {
    const response: AxiosResponse<AuthResponse> = await axios.post('/api/user/register', { email, password });
    return response.data;
  }

  async login(email: string, password: string): Promise<AuthResponse> {
    const response: AxiosResponse<AuthResponse> = await axios.post('/api/user/login', { email, password });
    return response.data;
  }

  async refreshToken({accessToken, refreshToken} : UserTokens): Promise<RefreshTokenResponse> {
    const response: AxiosResponse<AuthResponse> = await axios.post('/api/user/refresh-token', { apiToken: accessToken, refreshToken });
    return response.data;
  }
}

export const authService = new AuthService();