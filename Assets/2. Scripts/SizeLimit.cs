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
        rectTrans.sizeDelta = new Vector2(
            Mathf.Clamp(rectTrans.sizeDelta.x, 800, 10000),
            Mathf.Clamp(rectTrans.sizeDelta.y, 800, 10000)
            );
	}
}
