using JobQueueTrigger.Service.Interface;
using Newtonsoft.Json.Linq;
using ServerSideProgramming.Model.Entity;

namespace JobQueueTrigger.Service
{
    public class WeatherService : IWeatherService
    {
        public async Task<StationMeasurement[]> GetWeather()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(getWeatherUrl());

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Downloading weather data did not succeed!");
                    }

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    return getTargetArray(jsonContent);
                }
                catch (HttpRequestException e)
                {
                    throw e;
                }
            }
        }

        private string getWeatherUrl()
        {
            return "https://data.buienradar.nl/2.0/feed/json";
        }

        private StationMeasurement[] getTargetArray(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            JArray jArr = getActualObject(jsonObject);
            return transformToObjArr(jArr);
        }

        private JArray getActualObject(JObject obj)
        {
            if (obj["actual"]["stationmeasurements"] == null)
            {
                throw new Exception("Data not found!");
            }
            return obj["actual"]["stationmeasurements"] as JArray;
        }

        private StationMeasurement[] transformToObjArr(JArray jsonArr)
        {
            StationMeasurement[]? arr = jsonArr.ToObject<StationMeasurement[]>();
            if (arr == null)
            {
                throw new Exception("Something went wrong during conversion!");
            }
            return arr;
        }
    }
}
