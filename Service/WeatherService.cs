using JobQueueTrigger.Model;
using JobQueueTrigger.Service.Interface;
using Newtonsoft.Json.Linq;

namespace JobQueueTrigger.Service
{
    public class WeatherService : IWeatherService
    {
        public async Task<StationMeasurement[]> GetWeather()
        {
            string url = "https://data.buienradar.nl/2.0/feed/json";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        return getTargetArray(jsonContent);
                    }
                    else
                    {
                        throw new Exception("Downloading weather data did not succeed!");
                    }
                }
                catch (HttpRequestException e)
                {
                    throw e;
                }
            }
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
