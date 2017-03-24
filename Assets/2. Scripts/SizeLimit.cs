using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeLimit : MonoBehaviour {

    RectTransform rectTrans;
    Rect rect;
    Vector2 sd;

    void Start()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        rect = rectTrans.rect;
        sd = rectTrans.sizeDelta;
    }

	void Update () {
        Debug.Log("Sizelimit");
        /*rect = new Rect(
            rect.x,
            rect.y,
            Mathf.Clamp(rect.width, 100, 1000),
            Mathf.Clamp(rect.height, 100, 1000)
            );*/
        //rectTrans.rect = rect;
        /*Vector2 vec = new Vector2(
            Mathf.Clamp(sd.x, 10, 1000),
            Mathf.Clamp(sd.y, 10, 1000)
            );
        rectTrans.sizeDelta = vec;*/
        rectTrans.sizeDelta = new Vector2(
            Mathf.Clamp(rectTrans.sizeDelta.x, 800, 10000),
            Mathf.Clamp(rectTrans.sizeDelta.y, 800, 10000)
            );
	}
}
