import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { WeatherForecast } from './weatherforecast.model';

@Injectable({
  providedIn: 'root',
})
export class WeatherService {
  constructor(private http: HttpClient) {}

  getWeatherForecast(): Observable<WeatherForecast[]> {
    return this.http.get<WeatherForecast[]>(
      `${environment.apis.test}/weatherforecast`
    );
  }

  postWeatherForecast(
    weatherForecast: WeatherForecast
  ): Observable<WeatherForecast> {
    return this.http.post<WeatherForecast>(
      `${environment.apis.test}/weatherforecast`,
      weatherForecast
    );
  }
}
