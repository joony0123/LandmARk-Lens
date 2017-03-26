using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour {

    CanvasGroup cg;

	void Start () {
        cg = gameObject.GetComponent<CanvasGroup>();
        StartCoroutine("Fade");
	}

    IEnumerator Fade()
    {
        //Appear
        for (float g = 0f; g <= 1f; g += 0.1f)
        {
            if (g >= 0.9f)
            {
                g = 1f;
            }
            cg.alpha = g;
            yield return new WaitForSeconds(.1f);
        }

        //Pause
        yield return new WaitForSeconds(3f);

        for (float g = 1f; g >= 0f; g -= 0.1f)
        {
            if (g <= 0.1f)
            {
                g = 0f;
            }
            cg.alpha = g;
            yield return new WaitForSeconds(.1f);
        }
    }
}
