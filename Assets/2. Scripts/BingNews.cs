using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this needs to be on the same gameobject as LandmarkDetect, aka the camera
public class BingNews : MonoBehaviour {

    Dictionary<string, string> headers = new Dictionary<string, string>();

    public void fetchNews()
    {
        headers.Add("Ocp-Apim-Subscription-Key", "0cbbf65a0afc46ecb1daa8e8b6723e3e");

        var getLandmarkDetect = gameObject.GetComponent<LandmarkDetect>();
        string landmarkCity = getLandmarkDetect.landmarkCity;
        string landmarkCountry = getLandmarkDetect.landmarkCountry;
        string landmarkCityUrl = "https://api.cognitive.microsoft.com/bing/v5.0/news/search?q=" + landmarkCity + "&count=5";
        string landmarkCountryUrl = "https://api.cognitive.microsoft.com/bing/v5.0/news/search?q=" + landmarkCountry + "&count=5";

        Debug.Log("landmarkCityUrl: " + landmarkCityUrl);
        Debug.Log("landmarCountryUrl: " + landmarkCountryUrl);

        WWW landmarkCitywww = new WWW(landmarkCityUrl, null, headers);
        WWW landmarkCountrywww = new WWW(landmarkCountryUrl, null, headers);
        //StartCoroutine(WaitForRequest(landmarkCitywww));
        //StartCoroutine(WaitForRequest(landmarkCountrywww));
        fetchDescriptionAndImage("https://api.cognitive.microsoft.com/api/v5/entities/3bd9e326-67e3-cb9d-57a5-63f0f59fa061", "https://www.bing.com/th?id=ON.D6DB36F5EE39FAF1B3F03A8B14A44765&pid=News");
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            var json = JSON.Parse(www.text);
            var newsLists = json["value"].AsArray;

            foreach (var news in newsLists) {
                var newsArticle = JSON.Parse(news.ToString());

                string title = newsArticle["name"];
                string desc = newsArticle["description"];
                string readUrl = newsArticle["about"][0]["readLink"];
                string imageUrl = newsArticle["image"]["thumbnail"]["contentUrl"];

                Debug.Log(newsArticle);
                Debug.Log("Title: " + title + "\ndescription: " + desc + "\nRead URL: " + readUrl + "\nimageUrl: " + imageUrl);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    private void fetchDescriptionAndImage(string descUrl, string imageUrl)
    {
        Debug.Log(headers.Keys);
        Debug.Log(headers.Values);

        // Bring image file
        StartCoroutine(WaitForImage(new WWW(imageUrl)));

        // Bring full description of an article (not fetching anything so commented for now...)
        //StartCoroutine(WaitForDescRequest(new WWW(descUrl, null, headers)));

    }

    /* See above for comment
    IEnumerator WaitForDescRequest(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("Fetching description... ===>>> " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    */

    IEnumerator WaitForImage(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("Fetching image... ===>>> " + www.texture);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

}
