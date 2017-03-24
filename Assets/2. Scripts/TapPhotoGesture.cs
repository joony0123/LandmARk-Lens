using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;

public class TapPhotoGesture : MonoBehaviour
{
    GestureRecognizer recognizer;

    void Update()
    {
        //Raycast calculations;
        //var headPosition = Camera.main.transform.position;
        //var gazeDirection = Camera.main.transform.forward;
        //RaycastHit hitInfo;
    }

    public void Awake()
    {
        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();

        // Executed when a tap event is detected
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            Debug.Log("TAPPPPEED");
            // Raycast calculations
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;
            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                Debug.Log("Raycast hit something");
               if (hitInfo.collider.gameObject.tag == "Panel")
                {
                    Debug.Log("Hit Panel? Tag: " + hitInfo.collider.gameObject.tag);
                    //gameObject.GetComponent<PhotoCap3>().capturePhotoInitiate();
                }
                else
                {
                    Debug.Log("Take a photo!");
                    gameObject.GetComponent<PhotoCap3>().capturePhotoInitiate();
                }
            }
            else
            {
                // Call photo capture
                gameObject.GetComponent<PhotoCap3>().capturePhotoInitiate();
            }
        };
        recognizer.StartCapturingGestures();
    }
}