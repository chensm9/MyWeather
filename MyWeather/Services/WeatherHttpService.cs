using MyWeather.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Popups;

namespace MyWeather.Services {
    class WeatherHttpService {

        private static String host = "https://saweather.market.alicloudapi.com";
        private static String path = "/area-to-weather";
        private static String method = "GET";
        private static String appcode = "939c5fddc1f84c689daacc92568a2034";

        private static WeatherHttpService instance;
        public static WeatherHttpService GetInstance() {
            if(instance == null)
                instance = new WeatherHttpService();
            return instance;
        }

        private WeatherHttpService() { }

        public async Task<JsonObject> GetWeatherItem(string query_text) {
            //String querys = "area=%E4%B8%BD%E6%B1%9F&areaid=101291401&need3HourForcast=0&needAlarm=0&needHourData=0&needIndex=0&needMoreDay=0";
            String querys = "area=" + query_text + "&need3HourForcast=0&needAlarm=0&needHourData=0&needIndex=0&needMoreDay=0";
            String url = host + path + "?" + querys;

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = method;
            httpRequest.Headers["Authorization"] = "APPCODE " + appcode;

            HttpWebResponse httpResponse = null;
            try {
                httpResponse = (HttpWebResponse)(await httpRequest.GetResponseAsync());
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                return (JsonObject.Parse(reader.ReadToEnd()));
            } catch (WebException ex) {
                httpResponse = (HttpWebResponse)ex.Response;
                return (JsonObject.Parse(httpResponse.ToString()));
            }

        }
    }
}
