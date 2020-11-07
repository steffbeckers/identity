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

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.weatherService
      .getWeatherForecast()
      .subscribe((weatherForecast: WeatherForecast[]) => {
        this.weatherForecast = weatherForecast;
      });
  }
}
