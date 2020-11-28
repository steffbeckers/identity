import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth/auth.service';
import { WeatherService } from '../shared/services/weather/weather.service';
import { WeatherForecast } from '../shared/services/weather/weatherforecast.model';

@Component({
  selector: 'app-secure',
  templateUrl: './secure.component.html',
  styleUrls: ['./secure.component.scss'],
})
export class SecureComponent implements OnInit {
  weatherForecast: WeatherForecast[];
  weatherForecastJson: string;
  error: string;

  constructor(
    public authService: AuthService,
    private weatherService: WeatherService
  ) {}

  ngOnInit(): void {
    this.weatherService.getWeatherForecast().subscribe(
      (weatherForecast: WeatherForecast[]) => {
        this.weatherForecast = weatherForecast;
        this.weatherForecastJson = JSON.stringify(
          this.weatherForecast,
          null,
          2
        );
      },
      (error: any) => {
        if (error.status === 403) {
          this.error = 'Not allowed to retrieve weather forecast.';
        }
      }
    );
  }

  update(): void {
    this.error = null;

    const weatherForecasts: WeatherForecast[] = JSON.parse(
      this.weatherForecastJson
    );
    this.weatherService.postWeatherForecast(weatherForecasts[0]).subscribe(
      (weatherForecast: WeatherForecast) => {
        this.weatherForecast = [weatherForecast];
      },
      (error: any) => {
        if (error.status === 403) {
          this.error = 'Not allowed to update the weather forecast.';
        }
      }
    );
  }
}
