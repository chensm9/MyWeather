using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeather.Models;
using MyWeather.Services;
using Windows.UI.Popups;

namespace MyWeather.ViewModels {
    class WeatherViewModels {

        private WeatherHttpService service;
        public ObservableCollection<WeatherItem> AllItems { set; get; }
        public CurrentWeatherItem current_item;
        private Windows.Data.Json.JsonObject json { set; get; }

        private WeatherViewModels() {
            service = WeatherHttpService.GetInstance();
            current_item = new CurrentWeatherItem();
            AllItems = new ObservableCollection<WeatherItem>();
            GetWeather("广州");
        }

        public async void GetWeather(string text) {
            Windows.Data.Json.JsonObject total_json = await service.GetWeatherItem(text);
            if (total_json == null) {
                MessageDialog dialog = new MessageDialog("搜索不到相关城市信息\n请检查输入是否无误");
                await dialog.ShowAsync();
                return;
            }
            StringBuilder message = new StringBuilder("");
            if (total_json.GetNamedNumber("showapi_res_code") != 0) {
                message.AppendLine(total_json.GetNamedString("showapi_res_error"));
                MessageDialog dialog = new MessageDialog(message.ToString());
                await dialog.ShowAsync();
            } else {
                json = total_json.GetNamedObject("showapi_res_body");
                current_item.Area = json.GetNamedObject("cityInfo").GetNamedString("c9") +
                                    json.GetNamedObject("cityInfo").GetNamedString("c7") + "省" +
                                    json.GetNamedObject("cityInfo").GetNamedString("c3") + "市";
                current_item.Json = json.GetNamedObject("now");
                AllItems.Clear();
                for (int i = 1; i <= 7; i++) {
                    AllItems.Add(new WeatherItem(json.GetNamedObject("f"+i)));
                }
            }
        }

        private static WeatherViewModels instance;
        public static WeatherViewModels GetInstance() {
            if (instance == null)
                instance = new WeatherViewModels();
            return instance;
        }
    }
}
