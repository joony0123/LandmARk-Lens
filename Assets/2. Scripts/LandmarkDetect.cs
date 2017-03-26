using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.VR.WSA;

public class LandmarkDetect : MonoBehaviour
{
    public GameObject nameMapPanel;
    public GameObject wikiPanel;
    public GameObject weatherPanel;
    public GameObject newsPanel;
    
    public GameObject mapObject;
    public string landmarkName;
    public string landmarkCity;
    public string landmarkCountry;
    public Text locationDescription;
    public Text latlngText;

    GameObject canvas;
    Text userFeedbackText;
    CanvasGroup cgNameMap;
    CanvasGroup cgWiki;
    CanvasGroup cgNews;
    CanvasGroup cgWeather;

    string Image64;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        userFeedbackText = canvas.transform.GetChild(2).gameObject.GetComponent<Text>();
        //Image64 = "";
       // detectImage(Image64);

      


    }

    //called by PhotoCap2 when it takes a photo
    public void detectImage(string imageBase64)
    {
        gameObject.AddComponent<WorldAnchor>();

        string url = "https://vision.googleapis.com/v1/images:annotate?key=AIzaSyCz4W-NjIwxZp4N3lhfXnZ09V-JoACy5bE";
        string json = "{\"requests\": [ { \"image\" : {\"content\":\"" + imageBase64 + "\"},\"features\":[{\"type\":\"LANDMARK_DETECTION\",\"maxResults\":20}]}]}";
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        WWW www = new WWW(url, Encoding.ASCII.GetBytes(json.ToCharArray()), headers);
        StartCoroutine(WaitForRequest(www));
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            var json = JSON.Parse(www.text);
            landmarkName = json["responses"][0]["landmarkAnnotations"][0]["description"];
            string landmarkLat = json["responses"][0]["landmarkAnnotations"][0]["locations"][0]["latLng"]["latitude"];
            string landmarkLng = json["responses"][0]["landmarkAnnotations"][0]["locations"][0]["latLng"]["longitude"];

            Debug.Log("Landmark name: " + landmarkName);
            Debug.Log("Landmark Latitude: " + landmarkLat);
            Debug.Log("Landmark Longitude: " + landmarkLng);

            //Only deal with panel stuff if a landmark has actually been detected
            if (landmarkName != null)
            {
                InstantiatePanels(landmarkName, landmarkLat, landmarkLng);
                userFeedbackText.text = "Landmark Detected!";
            }
            else
            {
                userFeedbackText.text = "No Landmark Detected";
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
        yield return www;
    }

    private IEnumerator WaitForGeolocationRequest(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            var parsedJson = JSON.Parse(www.text);
            landmarkCity = parsedJson["results"][0]["address_components"][4]["long_name"];
            landmarkCountry = parsedJson["results"][0]["address_components"][5]["long_name"];

            Debug.Log(landmarkCountry);
            Debug.Log(landmarkCity);

            gameObject.GetComponent<BingNews>().fetchNews();

        } else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    //misleading name, sorry
    private void InstantiatePanels(string name, string lat, string lng)
    {
        lat = lat.Substring(0, 5);
        lng = lng.Substring(0, 5);

        //Activate panel and fade it in
        nameMapPanel = canvas.transform.GetChild(0).gameObject;
        nameMapPanel.SetActive(true);
        cgNameMap = nameMapPanel.GetComponent<CanvasGroup>();

        wikiPanel = canvas.transform.GetChild(1).gameObject;
        wikiPanel.SetActive(true);
        cgWiki = wikiPanel.GetComponent<CanvasGroup>();

        weatherPanel = canvas.transform.GetChild(2).gameObject;
        weatherPanel.SetActive(true);
        cgWeather = weatherPanel.GetComponent<CanvasGroup>();

        newsPanel = canvas.transform.GetChild(3).gameObject;
        newsPanel.SetActive(true);
        cgNews = newsPanel.GetComponent<CanvasGroup>();



        //call coroutine to start the fade ins
        StartCoroutine("Fade");

        locationDescription.text = name;
        latlngText.text = "Latitude : " + lat + "   Longitude : " + lng;

        //Map Stuff
        Map mapScript = mapObject.GetComponent<Map>();
        mapScript.SetMap(lat, lng);

        //Wiki stuff
        WikiResponse wikiScript = gameObject.GetComponent<WikiResponse>();
        wikiScript.WikiStuff();

        //News stuff
        BingNews newsScript = gameObject.GetComponent<BingNews>();
        newsScript.fetchNews();

        //debug cube
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = gameObject.transform.position + Vector3.forward * 5;
    }

    IEnumerator Fade()
    {
        for (float f = 0f; f <= 1f; f += 0.1f)
        {
            if(f >= 0.9f)
            {
                f = 1f;
            }
            cgNameMap.alpha = f;
            cgWiki.alpha = f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
