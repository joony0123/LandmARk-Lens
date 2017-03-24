using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VR.WSA.WebCam;
using System.Linq;
using System;
using System.IO;

#if !UNITY_EDITOR
using Windows.Storage;
using Windows.System;
#endif

public class PhotoCap3 : MonoBehaviour
{
    // Use this for initialization
    public void capturePhotoInitiate()
    {
        #if !UNITY_EDITOR
        
        capturePhoto();


       
        #endif
    }
#if !UNITY_EDITOR
    PhotoCapture photoCaptureObject = null;
    bool haveFolderPath = false;
    StorageFolder picturesFolder;
    string tempFilePathAndName;
    string tempFileName;

    //debug
    Texture2D targetTexture = null;
    Resolution cameraResolution;

    void Start(){
        getFolderPath();
    }

    void capturePhoto(){
     while (!haveFolderPath)
        {
            Debug.Log("Waiting for folder path...");
        }
        Debug.Log("About to call CreateAsync");
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        Debug.Log("Called CreateAsync");

        //debug
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
    }
    

    async void getFolderPath()
    {
        StorageLibrary myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        picturesFolder = myPictures.SaveFolder;

        foreach(StorageFolder fodler in myPictures.Folders)
        {
            Debug.Log(fodler.Name);

        }

        Debug.Log("savePicturesFolder.Path is " + picturesFolder.Path);
        haveFolderPath = true;
    }

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            tempFileName = string.Format(@"CapturedImage{0}_n.jpg", Time.time);

            string filePath = System.IO.Path.Combine(Application.persistentDataPath, tempFileName);
            tempFilePathAndName = filePath;
            Debug.Log("Saving photo to " + filePath);

            try
            {
                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("System.ArgumentException:\n" + e.Message);
            }
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Saved Photo to disk!");
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            Debug.Log("moving "+tempFilePathAndName+" to " + picturesFolder.Path + "\\Camera Roll\\" + tempFileName);
            File.Move(tempFilePathAndName, picturesFolder.Path + "\\Camera Roll\\" + tempFileName);

            Texture2D tex = null;
            byte[] fileData;
            if (File.Exists(picturesFolder.Path + "\\Camera Roll\\" + tempFileName)){
                fileData = File.ReadAllBytes(picturesFolder.Path + "\\Camera Roll\\" + tempFileName);
                string base64 = Convert.ToBase64String(fileData);

                print(base64);

                //GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
                //GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
               

               
                LandmarkDetect landmarkScript = gameObject.GetComponent<LandmarkDetect>();        //PhotoCap3 and LandmarkDetect are on the same GameObject
                landmarkScript.detectImage(base64);

                //debug
                /*tex = new Texture2D(1920, 1080);
                tex.LoadImage(fileData);
                Renderer quadRenderer = gameObject.GetComponent<Renderer>();
                quadRenderer.material = new Material(Shader.Find("Standard"));     //Custom/Unlit/UnlitTexture
                quadRenderer.material.SetTexture("_MainTex", tex);*/
            }
        }
        else
        {
            Debug.Log("Failed to save Photo to disk " +result.hResult+" "+result.resultType.ToString());
        }
    }

#endif
}