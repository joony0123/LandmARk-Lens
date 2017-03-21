using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.VR.WSA;


//this script is on the whole name & map panel
public class LandmarkDetect : MonoBehaviour
{
    public string landmarkName;
    public Text locationDescription;
    
    void Start()
    {
        //locationDescription = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    //called by PhotoCap2 when it takes a photo
    public void detectImage(string base64Image)
    {
        print("detectImage");

        gameObject.AddComponent<WorldAnchor>();

        string url = "https://vision.googleapis.com/v1/images:annotate?key=AIzaSyCz4W-NjIwxZp4N3lhfXnZ09V-JoACy5bE";
        string json = "{\"requests\": [ { \"image\" : {\"content\":\"" + base64Image + "\"},\"features\":[{\"type\":\"LANDMARK_DETECTION\",\"maxResults\":20}]}]}";
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
            var parsedJson = JSON.Parse(www.text);
            landmarkName = parsedJson["responses"][0]["landmarkAnnotations"][0]["description"];
            var landmarkLat = parsedJson["responses"][0]["landmarkAnnotations"][0]["locations"][0]["latLng"]["latitude"];
            var landmarkLng = parsedJson["responses"][0]["landmarkAnnotations"][0]["locations"][0]["latLng"]["longitude"];

            locationDescription.text = landmarkName + "\n" + "<Geolocation>\n" + "Latitude : " + landmarkLat + "\nLongitude : " + landmarkLng;
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
