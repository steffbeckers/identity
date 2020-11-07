import { Injectable } from '@angular/core';
import { OAuthService, UserInfo } from 'angular-oauth2-oidc';
import { filter } from 'rxjs/operators';
import { authCodeFlowConfig } from './auth.config';
import { User } from './auth.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private oauthService: OAuthService) {
    this.oauthService.configure(authCodeFlowConfig);
    this.oauthService.loadDiscoveryDocumentAndLogin();
    //this.oauthService.setupAutomaticSilentRefresh();

    // Automatically load user profile
    this.oauthService.events
      .pipe(filter((e) => e.type === 'token_received'))
      .subscribe((_) => this.oauthService.loadUserProfile());
  }

  isAuthenticated(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  get user(): User {
    const claims = this.oauthService.getIdentityClaims();
    if (!claims) return null;

    return {
      id: claims['sub'],
      username: claims['preferred_username'],
      email: claims['email'],
      claims,
    };
  }

  refresh(): void {
    this.oauthService.refreshToken();
  }
}
