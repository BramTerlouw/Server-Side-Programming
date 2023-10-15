using JobQueueTrigger.Model;
using JobQueueTrigger.Service.Interface;
using Newtonsoft.Json.Linq;

namespace JobQueueTrigger.Service
{
    public class WeatherService : IWeatherService
    {
        public async Task<StationMeasurement[]> GetWeather()
        {
            StationMeasurement[]? arr = null;
            string url = "https://data.buienradar.nl/2.0/feed/json";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        arr = getTargetArray(jsonContent);
                    }
                    else
                    {
                        Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HTTP request error: {e.Message}");
                }
            }
            return arr;
        }

        private StationMeasurement[] getTargetArray(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            JObject subObj = getActualObject(jsonObject);
            JArray jArr = getStationArray(subObj);
            return transformToObjArr(jArr);
        }

        private JObject getActualObject(JObject obj)
        {
            if (obj["actual"] == null)
            {
                throw new Exception("Actual information not found");
            }
            return obj["actual"] as JObject;
        }

        private JArray getStationArray(JObject obj)
        {
            if (obj["stationmeasurements"] != null)
            {
                throw new Exception("Station measurements not found");
            }
            return obj["stationmeasurements"] as JArray;
        }

        private StationMeasurement[] transformToObjArr(JArray jsonArr)
        {
            StationMeasurement[]? arr = jsonArr.ToObject<StationMeasurement[]>();
            if (arr == null)
            {
                throw new Exception();
            }
            return arr;
        }
    }
}
