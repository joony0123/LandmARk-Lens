using System.Collections;
using UnityEngine;
using SimpleJSON;
using System;
using System.Globalization;
using UnityEngine.UI;

public class WeatherDemo : MonoBehaviour
{
    private string url;

    GameObject canvas;
    GameObject weatherPanel;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        weatherPanel = canvas.transform.GetChild(2).gameObject;     //make sure weather panel is in the right spot!
    }

    public void startWeather(string lat, string lon)
    {
        StartCoroutine(fetchWeather(lat, lon));
        //StartCoroutine(fetchWeather("60", "60"));       //debugging
    }

    private IEnumerator fetchWeather(string lat, string lon)
    {
        yield return new WaitForSeconds(.1f);
        ArrayList dates = new ArrayList();

        //temperature contains max and min of particular day in order
        ArrayList temperature = new ArrayList();
        ArrayList weather = new ArrayList();

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
            }
            else
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
        GameObject icons = weatherPanel.transform.GetChild(2).gameObject;
        GameObject temp = weatherPanel.transform.GetChild(1).gameObject;
        int tempCounter = 0;
        GameObject weatherDates = weatherPanel.transform.GetChild(0).gameObject;
        GameObject dayWeek = weatherPanel.transform.GetChild(3).gameObject;
        for (int i = 0; i < weather.Count; i++)
        {
            //Debug.Log(weather[i] + " ");
            Image image = icons.transform.GetChild(i).gameObject.GetComponent<Image>();
            if ((weather[i] + "").Contains("Clouds"))
            {
                image.sprite = Resources.Load<Sprite>("Cloudy") as Sprite;
            }
            else if ((weather[i] + "").Contains("Snow"))
            {
                image.sprite = Resources.Load<Sprite>("Snow") as Sprite;
            }
            else if ((weather[i] + "").Contains("Rain"))
            {
                image.sprite = Resources.Load<Sprite>("Rain") as Sprite;
            }
            else
            {
                image.sprite = Resources.Load<Sprite>("Sunny") as Sprite;
            }
        }
        for (int i = 0; i < temperature.Count; i += 2)
        {
            //i is max and i + 1 is min
            Text text = temp.transform.GetChild(tempCounter).gameObject.GetComponent<Text>();
            text.text = "High: " + temperature[i] + "\n" + "Low: " + temperature[i + 1];
            tempCounter++;
        }
        for (int i = 0; i < dates.Count; i++)
        {
            //Debug.Log(dates[i] + " ");
            Text text = weatherDates.transform.GetChild(i).gameObject.GetComponent<Text>();
            text.text = dates[i] + "";

            Text text2 = dayWeek.transform.GetChild(i).gameObject.GetComponent<Text>();
            string dayOfWeek = GetDay(dates[i] + "");
            text2.text = dayOfWeek;
        }
    }

    string GetDay(string input)
    {
        string[] splitStr = input.Split(' ');
        string month = splitStr[0].Trim();
        string day = splitStr[1].Trim();

        int monthInt = 1;
        switch (month)
        {
            case "January":
                monthInt = 1;
                break;
            case "February":
                monthInt = 2;
                break;
            case "March":
                monthInt = 3;
                break;
            case "April":
                monthInt = 4;
                break;
            case "May":
                monthInt = 5;
                break;
            case "June":
                monthInt = 6;
                break;
            case "July":
                monthInt = 7;
                break;
            case "August":
                monthInt = 8;
                break;
            case "September":
                monthInt = 9;
                break;
            case "October":
                monthInt = 10;
                break;
            case "November":
                monthInt = 11;
                break;
            case "December":
                monthInt = 12;
                break;
        }
        int dayInt = Convert.ToInt32(day);

        DateTime dateTime = new DateTime(2017, monthInt, dayInt);
        string dayOfWeek = dateTime.DayOfWeek.ToString();

        //Debug.Log(dayOfWeek);
        //Debug.Log("Month: " + month);
        //Debug.Log("Day: " + day);

        return dayOfWeek;
    }
}
