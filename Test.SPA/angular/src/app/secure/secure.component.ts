import { Component, OnInit } from '@angular/core';
import { WeatherService } from '../shared/services/weather/weather.service';
import { WeatherForecast } from '../shared/services/weather/weatherforecast.model';

@Component({
  selector: 'app-secure',
  templateUrl: './secure.component.html',
  styleUrls: ['./secure.component.scss'],
})
export class SecureComponent implements OnInit {
  weatherForecast: WeatherForecast[];
  error: string;

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.weatherService.getWeatherForecast().subscribe(
      (weatherForecast: WeatherForecast[]) => {
        this.weatherForecast = weatherForecast;
      },
      (error: any) => {
        if (error.status === 403) {
          this.error = 'Not allowed to retrieve weather forecast.';
        }
      }
    );
  }
}
