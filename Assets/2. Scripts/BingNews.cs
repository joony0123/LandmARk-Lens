using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this needs to be on the same gameobject as LandmarkDetect, aka the camera
public class BingNews : MonoBehaviour
{

    Dictionary<string, string> headers = new Dictionary<string, string>();
    ArrayList spriteList = new ArrayList();

    public void fetchNews()
    {
        headers.Add("Ocp-Apim-Subscription-Key", "0cbbf65a0afc46ecb1daa8e8b6723e3e");

        LandmarkDetect getLandmarkDetect = gameObject.GetComponent<LandmarkDetect>();
        string landmarkCity = getLandmarkDetect.landmarkCity;
        string landmarkCountry = getLandmarkDetect.landmarkCountry;
        string landmarkCityUrl = "https://api.cognitive.microsoft.com/bing/v5.0/news/search?q=" + landmarkCity + "&count=5";
        string landmarkCountryUrl = "https://api.cognitive.microsoft.com/bing/v5.0/news/search?q=" + landmarkCountry + "&count=5";

        Debug.Log("landmarkCityUrl: " + landmarkCityUrl);
        Debug.Log("landmarCountryUrl: " + landmarkCountryUrl);

        WWW landmarkCitywww = new WWW(landmarkCityUrl, null, headers);
        WWW landmarkCountrywww = new WWW(landmarkCountryUrl, null, headers);
        string city = "City";
        string country = "Country";
        StartCoroutine(WaitForRequest(landmarkCitywww, city));
        StartCoroutine(WaitForRequest(landmarkCountrywww, country));
        // fetchDescriptionAndImage("https://api.cognitive.microsoft.com/api/v5/entities/3bd9e326-67e3-cb9d-57a5-63f0f59fa061", "https://www.bing.com/th?id=ON.D6DB36F5EE39FAF1B3F03A8B14A44765&pid=News");
    }

    IEnumerator WaitForRequest(WWW www, string word)
    {
        int count = 0;
        yield return www;

        if (www.error == null)
        {
            var json = JSON.Parse(www.text);
            var newsLists = json["value"].AsArray;

            //Set it as private variable for other use
            //privatelist = newsLists;
            if (word.Equals("City")) {
                count = 0;
            } else
            {
                count = 5;
            }

            for (int i = count; i < count + 5; i++)
            {
                var newsArticle = JSON.Parse(newsLists[i].ToString()); 
                if (word.Equals("City")) {
                    newsArticle = JSON.Parse(newsLists[i].ToString());
                } else
                {
                    newsArticle = JSON.Parse(newsLists[i-5].ToString());
                }
                

                string title = newsArticle["name"];
                string desc = newsArticle["description"];
                //string readUrl = newsArticle["about"][0]["readLink"];
                string imageUrl = newsArticle["image"]["thumbnail"]["contentUrl"];

                Transform newsContentTr = GameObject.FindGameObjectWithTag("News Content").transform;
                newsContentTr.GetChild(i).GetChild(1).GetComponent<Text>().text = title;
                newsContentTr.GetChild(i).GetChild(2).GetComponent<Text>().text = desc;

                //fetch image for each news
                fetchImage(imageUrl, i);

                Debug.Log(newsArticle);
                //  Debug.Log("Title: " + title + "\ndescription: " + desc + "\nRead URL: " + readUrl + "\nimageUrl: " + imageUrl);
                Debug.Log("Title: " + title + "\ndescription: " + desc + "\nimageUrl: " + imageUrl);
            }
            //Set the texts
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    private void fetchImage(string imageUrl,int num)
    {
        Debug.Log(headers.Keys);
        Debug.Log(headers.Values);

        // Bring image file
        StartCoroutine(WaitForImage(new WWW(imageUrl),num));

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

    IEnumerator WaitForImage(WWW www, int num)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("Fetching image... ===>>> " + www.texture);
            // If success
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height);
            www.LoadImageIntoTexture(texture);
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, www.texture.width, www.texture.height), new Vector2(0, 0), 100f);
            

            GameObject.FindGameObjectWithTag("News Content").transform.GetChild(num).GetChild(0).GetComponent<Image>().sprite = sprite;

            spriteList.Add(sprite);
            //Image image = gameObject.GetComponent<Image>();
            //  image.sprite = sprite;

        }
        else
        {
            Debug.Log("Default Image");
            
        }
    }

}
