import Cookies from 'js-cookie';
import { UserTokens } from '../interfaces/UserTokens';

const ACCESS_TOKEN_KEY = 'accessToken';
const REFRESH_TOKEN_KEY = 'refreshToken';
const USER_EMAIL_KEY = 'userEmail';

export class AuthTokens {
  static setTokens(tokens: UserTokens, email: string) {
    Cookies.set(ACCESS_TOKEN_KEY, tokens.accessToken, { expires: 1/24, secure: true }); 
    Cookies.set(REFRESH_TOKEN_KEY, tokens.refreshToken, { expires: 7, secure: true });
    localStorage.setItem(USER_EMAIL_KEY, email);
  }

  static getAccessToken(): string | undefined {
    return Cookies.get(ACCESS_TOKEN_KEY);
  }

  static getRefreshToken(): string | undefined {
    return Cookies.get(REFRESH_TOKEN_KEY);
  }

  static getEmail(): string | null {
    return localStorage.getItem(USER_EMAIL_KEY);
  }

  static clearTokens() {
    Cookies.remove(ACCESS_TOKEN_KEY);
    Cookies.remove(REFRESH_TOKEN_KEY);
    localStorage.removeItem(USER_EMAIL_KEY);
  }
}
