import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { SecureComponent } from './secure/secure.component';
import { HttpClientModule } from '@angular/common/http';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';

@NgModule({
  declarations: [AppComponent, HomeComponent, SecureComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['https://localhost:5001/api'],
        sendAccessToken: true,
      },
    }),
  ],
  providers: [
    // { provide: OAuthStorage, useValue: localStorage }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
