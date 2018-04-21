using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyWeather.Models {
    public class WeatherItem : INotifyPropertyChanged {
        static string[] week = { "一", "二", "三", "四", "五", "六", "日" };

        private string day { get; set; }
        private int weekday { get; set; }
        private string day_air_temperature { get; set; }
        private string night_air_temperature { get; set; }
        private string weather { get; set; }
        private string weather_pic { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WeatherItem(Windows.Data.Json.JsonObject json) {
            Day = json.GetNamedString("day");
            weekday = (int)json.GetNamedNumber("weekday");
            day_air_temperature = json.GetNamedString("day_air_temperature");
            night_air_temperature = json.GetNamedString("night_air_temperature");
            Weather = json.GetNamedString("day_weather");
            Weather_pic = json.GetNamedString("day_weather_pic");
        }

        public string Day {
            set {
                day = value;
                NotifyPropertyChanged("Day");
            }
            get {
                return day.Substring(0, 4) + "年" + 
                       day.Substring(4, 2) + "月" +  
                       day.Substring(6, 2) + "日\n" 
                       + "星期" + week[weekday-1];
            }
        }


        public string Air_temperature {
            get {
                return night_air_temperature + "°C ～ " + day_air_temperature + "°C";
            }
        }

        public string Weather {
            set {
                weather = value;
                NotifyPropertyChanged("Weather");
            }
            get {
                return weather;
            }
        }

        public string Weather_pic {
            set {
                weather_pic = value;
                NotifyPropertyChanged("Weather_pic");
            }
            get {
                return weather_pic;
            }
        }

    }

    public class CurrentWeatherItem : INotifyPropertyChanged {
        private Windows.Data.Json.JsonObject json;
        public Windows.Data.Json.JsonObject Json {
            set {
                json = value;
                NotifyPropertyChanged("Area");
                NotifyPropertyChanged("Weather");
                NotifyPropertyChanged("Weather_pic");
                NotifyPropertyChanged("Sd");
                NotifyPropertyChanged("Wind");
                NotifyPropertyChanged("Temperature");
                NotifyPropertyChanged("Temperature_time");
            }
            get { return json; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CurrentWeatherItem(Windows.Data.Json.JsonObject jo = null) {
            json = jo;
        }

        public string Weather {
            get {
                if (json == null) return "";
                return "天气  " + json.GetNamedString("weather");
            }
        }

        public string Weather_pic {
            get {
                if (json == null) return "http://appimg.showapi.com/images/weather/icon/day/00.png";
                return json.GetNamedString("weather_pic");
            }
        }

        public string Sd {
            get {
                if (json == null) return "";
                return "湿度  "+json.GetNamedString("sd");
            }
        }

        public string Api {
            get {
                if (json == null) return "";
                return "空气质量指数  " + json.GetNamedString("api") 
                       + "      级别  " + json.GetNamedObject("api_detail").GetNamedString("quality");
            }
        }

        public string Wind {
            get {
                if (json == null) return "";
                return "风速  " + json.GetNamedString("wind_direction") + "  " + json.GetNamedString("wind_power");
            }
        }

        public string Temperature {
            get {
                if (json == null) return "";
                return json.GetNamedString("temperature") + "℃";
            }
        }

        public string Temperature_time {
            get {
                if (json == null) return "";
                return "最后更新时间 " + json.GetNamedString("temperature_time");
            }
        }

        private string area;
        public string Area {
            set { area = value; }
            get { return area; }
        }
    }
}
