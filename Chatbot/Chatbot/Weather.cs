using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chatbot
{
    //Using OpenWeatherMap API
    //Link: https://openweathermap.org/

    class Weather
    {
        //This should be hidden in an environment variable, or something similar
        //However since its a free API and limited to 60 uses per minute only, it doesn't matter here
        private const string ApiKey = "19d1d098bb881b9f111a034555625c22";

        private string baseUrl = "https://api.openweathermap.org/data/2.5/weather?";

        public async void CurrentWeather(string city)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var res = await client.GetAsync($"{baseUrl}q={city}&appid={ApiKey}"))
                    {
                        using (var content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();

                            if (data != null)
                            {

                                var dataObj = JObject.Parse(data);

                                var mainObj = dataObj["main"];
                                var temperatureObj = mainObj["temp"];

                                double temperature;

                                try
                                {
                                    temperature = double.Parse(temperatureObj.ToString());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    throw;
                                }

                                const double kelvinValue = 273.15;
                                temperature = temperature - kelvinValue;

                                //Get the weather description
                                var weatherObj = dataObj["weather"];
                                var array = weatherObj.First;
                                var descr = array["description"];


                                Console.WriteLine($"The temperature is right now: {temperature} degrees celcius\n" +
                                                  $"Description of weather: {descr}" +
                                                  $"Type 'weather' to look at a different city");
                            }
                            else
                            {
                                Console.WriteLine("No data found");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Not a valid city\n" +
                                  "Type 'weather' to try again");
            }
        }
    }
}
