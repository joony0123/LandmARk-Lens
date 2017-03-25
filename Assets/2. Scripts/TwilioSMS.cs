using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwilioSMS : MonoBehaviour {

	// Use this for initialization
	void Start () {
        sendSMS();
	}

    public void sendSMS()
    {
        string landmarkName = gameObject.GetComponent<LandmarkDetect>().landmarkName;
        landmarkName = landmarkName.Equals("") ? "Paris" : landmarkName;
        string from = "+13174276065";
        string to = "+14705296014";
        string accountSid = "AC3beec661c21e890d91c3848ec56bab3c";
        string authToken = "0d817e48b138e7e1ec76c24d05c8da86";
        string body = "I'd like to travel in " + landmarkName + "\n. Wanna travel with me?";
        WWWForm form = new WWWForm();

        form.AddField("To", to);
        form.AddField("From", from);
        form.AddField("Body", body);
        string url = "https://" + accountSid + ":" + authToken + "@api.twilio.com/2010-04-01/Accounts/" + accountSid + "/Messages.json";
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok! SMS successfully sent!" + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

}
