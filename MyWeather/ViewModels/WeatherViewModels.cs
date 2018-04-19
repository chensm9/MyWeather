using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeather.Models;
using MyWeather.Services;
using Windows.UI.Popups;

namespace MyWeather.ViewModels {
    class WeatherViewModels {

        private WeatherHttpService service;

        private WeatherViewModels() {
            service = WeatherHttpService.GetInstance();
        }

        public async void GetWeather(string text) {
            Windows.Data.Json.JsonObject jo = await service.GetWeatherItem(text);
            StringBuilder message = new StringBuilder("");
            message.AppendLine(jo.ToString());
            if (message.Equals("")) {
                message.AppendLine("搜索不到相关条目");
            }
            MessageDialog dialog = new MessageDialog(message.ToString());
            await dialog.ShowAsync();
        }

        private static WeatherViewModels instance;
        public static WeatherViewModels GetInstance() {
            if (instance == null)
                instance = new WeatherViewModels();
            return instance;
        }
    }
}
