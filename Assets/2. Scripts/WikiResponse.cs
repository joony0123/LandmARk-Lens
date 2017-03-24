using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.VR.WSA;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Persistence;


public class WikiResponse : MonoBehaviour {

    private  string url;
    public Text wikiText;
    private string landmarkName;
    private string[] landmarkNameArr;
    private WorldAnchorManager anchorMgr;
    private WorldAnchor anchor;

    // Use this for initialization
    public void WikiStuff () {

        // World Anchoring Not Working
        /*anchor = this.gameObject.AddComponent<WorldAnchor>();
        anchorMgr = new WorldAnchorManager();
        anchorMgr.AttachAnchor(this.gameObject, "This Anchor");*/
        




        StartCoroutine(fetch());
        
    }

  

    private IEnumerator fetch()
    {
        yield return new WaitForSeconds(1f);

        //landmarkName = GameObject.Find("Name & Map Panel").GetComponent<LandmarkDetect>().landmarkName;
        landmarkName = gameObject.GetComponent<LandmarkDetect>().landmarkName;
        url = "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro=&explaintext=&titles=";
        landmarkNameArr = landmarkName.Split(' ');
        for(int i = 0; i < landmarkNameArr.Length; i++)
        {
            if (i != landmarkNameArr.Length - 1)
            {
                url += landmarkNameArr[i] + "%20";
            }
            else
            {
                url += landmarkNameArr[i];
            }
        }
            
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(www.text);

        var parsedJson = JSON.Parse(www.text);
        var info = parsedJson["query"]["pages"][0][3];

        wikiText.text = info;
        Debug.Log(info);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
