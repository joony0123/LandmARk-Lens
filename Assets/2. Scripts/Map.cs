using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {
    public string url;

    private string latitude;
    private string longitude;

	// Use this for initialization
	public void SetMap(string lat, string lng) {
        latitude = lat;
        longitude = lng;
        StartCoroutine(getMap());
    }

    private IEnumerator getMap()
    {
        yield return new WaitForSeconds(3f);

        //var obj = GameObject.Find("Name & Map Panel");
        //Debug.Log(obj);

        //var latitude = GameObject.Find("Name & Map Panel").GetComponent<Demo2>().landmarkLat;
        //var longitude = GameObject.Find("Name & Map Panel").GetComponent<Demo2>().landmarkLng;
        //Debug.Log("@@@@@" + GameObject.Find("Name & Map Panel").GetComponent<Demo2>().landmarkName);
        //string latitude = "37.423021";
        //string longitude = "-122.083739";

        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=14&scale=2&size=2560x2560&key=AIzaSyD21BFpl21Pow1qg6ce_kcLvaXFBGnxrUU"
            + "&markers=color:red%7C" + latitude + "," + longitude;

        WWW request = new WWW(url);
        yield return request;

        //GetComponent<Renderer>().material.mainTexture = request.texture;
        Texture2D texture = new Texture2D(request.texture.width, request.texture.height);
        request.LoadImageIntoTexture(texture);
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, request.texture.width, request.texture.height), new Vector2(0,0), 100f);
        Image image = gameObject.GetComponent<Image>();
        image.sprite = sprite;
    }
}