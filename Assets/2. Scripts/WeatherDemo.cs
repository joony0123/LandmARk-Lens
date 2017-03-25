using System.Collections;
using UnityEngine;
using SimpleJSON;
using System;
using System.Globalization;

public class WeatherDemo : MonoBehaviour {
    private string url;
    private string lat;
    private string lon;
	// Use this for initialization
	void Start () {
        StartCoroutine(fetchWeather());
	}
	
    private IEnumerator fetchWeather()
    {
        yield return new WaitForSeconds(3f);
        ArrayList dates = new ArrayList();

        //temperature contains max and min of particular day in order
        ArrayList temperature = new ArrayList();
        ArrayList weather = new ArrayList();

        //hardcode lat, lon
        lat = "35";
        lon = "139";

        //depends on how LandmarkDetect.cs has changed.
        // lat = gameObject.GetComponent<LandmarkDetect>().landmarkName;

        url = "http://api.openweathermap.org/data/2.5/forecast?lat=" + lat + "&lon=" + lon + "&appid=3bbed7b4815f12091eaada1427722254";

        WWW request = new WWW(url);
        yield return request;

        var parsedJson = JSON.Parse(request.text);
        var detail = parsedJson["list"];
        //Debug.Log(detail[0]["dt_txt"]);
        double max = double.MinValue;
        double min = double.MaxValue;

        for (int i = 0; i < detail.Count; i++)
        {
            if (i % 8 == 0)
            {
                if (i == 0)
                {
                    max = double.Parse(detail[0]["main"]["temp_max"]);
                    min = double.Parse(detail[0]["main"]["temp_min"]);
                }
                //convert Kelvin to Fahrenheit
                max = Math.Round(max * (9.0 / 5) - 459.67, 2);
                min = Math.Round(min * (9.0 / 5) - 459.67, 2);
                temperature.Add(max);
                temperature.Add(min);
                weather.Add(detail[i]["weather"][0]["main"]);
                max = double.MinValue;
                min = double.MaxValue;
                string date = detail[i]["dt_txt"];
                DateTime dt = Convert.ToDateTime(date);
                date = dt.ToString("MMMM dd");
                dates.Add(date);
                max = double.Parse(detail[i]["main"]["temp_max"]);
                min = double.Parse(detail[i]["main"]["temp_min"]);
            } else
            {
                double currentMax = double.Parse(detail[i]["main"]["temp_max"]);
                double currentMin = double.Parse(detail[i]["main"]["temp_min"]);
              
                if (currentMax > max)
                {
                    max = currentMax;
                }
                if (currentMin < min)
                {
                    min = currentMin;
                }
            }
        }
        for (int i = 0; i < weather.Count; i++)
        {
            Debug.Log(weather[i] + " ");
        }
        for (int i = 0; i < temperature.Count; i++)
        {
            Debug.Log(temperature[i] + " ");
        }
        for (int i = 0; i < dates.Count; i++)
        {
            Debug.Log(dates[i] + " ");
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
