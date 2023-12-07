
import { makeAutoObservable } from "mobx";
import { authService } from '../services/authService';
import { NotificationService } from "../services/notificationService";
import { AuthTokens } from "../services/authTokens";
import { AuthResponse } from "../interfaces/AuthResponse";
import { RefreshTokenResponse } from "../interfaces/RefreshTokenResponse";
import { UserData } from "../interfaces/UserData";

class AuthStore {
  private userData: UserData | null;

  constructor() {
    makeAutoObservable(this);
    this.userData = this.getUserDataFromPersist();
  }

  private getUserDataFromPersist(): UserData | null {
    const accessToken = AuthTokens.getAccessToken();
    const refreshToken = AuthTokens.getRefreshToken();
    const email = AuthTokens.getEmail();
    return accessToken && refreshToken && email ? { tokens: { accessToken, refreshToken}, email } : null;
  }

  private setUserData(userData: UserData | null): void {
    this.userData = userData;
    if (userData) {
      AuthTokens.setTokens(userData.tokens, userData.email);
    } else {
      AuthTokens.clearTokens();
    }
  }

  get isLoggedIn(): boolean {
    return !!this.userData?.tokens;
  }

  get currentUser() : UserData | null {
    return this.userData;
  }

  async register(email: string, password: string): Promise<void> {
    try {
      const data: AuthResponse = await authService.register(email, password);
      this.setUserData({ tokens: {accessToken: data.apiToken, refreshToken: data.refreshToken}, email });
      NotificationService.success('Registration successful.');
    } catch (error) {
      console.error('Error during registration', error);
      NotificationService.error('Registration failed.');
    }
  }

  async login(email: string, password: string): Promise<void> {
    try {
      const data: AuthResponse = await authService.login(email, password);
      this.setUserData({ tokens: {accessToken: data.apiToken, refreshToken: data.refreshToken}, email });
      NotificationService.success('Login successful.');
    } catch (error) {
      console.error('Error during login', error);
      NotificationService.error('Login failed.');
    }
  }

  async refreshToken(): Promise<void> {
    try {
      if (!this.userData) throw new Error("No user data available");
      const data: RefreshTokenResponse = await authService.refreshToken(this.userData.tokens);
      this.setUserData({ ...this.userData, tokens: {accessToken: data.apiToken, refreshToken: data.refreshToken }});
      NotificationService.success('Token refreshed successfully.');
    } catch (error) {
      console.error('Error during token refresh', error);
      NotificationService.error('Token refresh failed.');
      await this.logout();
    }
  }

  async logout(): Promise<void> {
    try {
      this.setUserData(null);
      NotificationService.success('Logout successful.');
    } catch (error) {
      console.error('Error during logout', error);
      NotificationService.error('Logout failed.');
    }
  }
}

export const authStore = new AuthStore();