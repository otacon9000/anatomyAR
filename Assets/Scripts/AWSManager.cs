using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.S3;
using Amazon.CognitoIdentity;
using Amazon.S3.Model;
using System.IO;
using UnityEngine.Networking;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("AWSManager is NULL");
            }
            return _instance;
        }
    }

    public GameObject imageTarget;

    public string S3Region = RegionEndpoint.USEast2.SystemName;
    private RegionEndpoint _S3region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if(_s3Client == null)
            {
                _s3Client = new AmazonS3Client(new CognitoAWSCredentials(
                    "eu-central-1:916036dd-6f46-4b85-9d30-ace697b09fd7", // Identity pool ID
                    RegionEndpoint.EUCentral1), _S3region); //region
                  
            }

            return _s3Client;
        }
    }



    private void Awake()
    {
        _instance = this;

        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        var request = new ListObjectsRequest()
        {
            BucketName = "assetbundle-horse-ar"

        };

        S3Client.ListObjectsAsync(request, (responseObj) =>
         {
             if (responseObj.Exception == null)
             {
                 responseObj.Response.S3Objects.ForEach((obj) =>
                 {
                     Debug.Log("OBJ: "+obj.Key);
                 });
             }
             else 
             {
                 Debug.LogWarning("Response error: " + responseObj.Exception);
             }

         });

        DownloadBundle();
    }

    public void DownloadBundle()
    {
        //string backetName = "assetbundle-horse-ar";
        //string fileName = "horse";

        //S3Client.GetObjectAsync(backetName, fileName, (responseObj) =>
        //{
        //    if(responseObj.Exception == null)
        //    {

        //        string data = null;
        //        using (StreamReader reader = new StreamReader(responseObj.Response.ResponseStream))
        //        {
        //            data = reader.ReadToEnd();
        //            Debug.Log("Data: " + data);
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning("ERROR: " + responseObj.Exception);
        //    }
        //});

        StartCoroutine(BundleRoutine());
    }

    IEnumerator BundleRoutine()
    {
        string uri = "https://assetbundle-horse-ar.s3.us-east-2.amazonaws.com/horse";
        var request = new WWW(uri);


        yield return request;

        AssetBundle bundle = request.assetBundle;

        GameObject horse = bundle.LoadAsset<GameObject>("horse");

        horse = Instantiate(horse);

        horse.transform.parent = imageTarget.transform;
    }


    //IEnumerator BundleRoutine()
    //{
    //    string uri = "https://assetbundle-horse-ar.s3.us-east-2.amazonaws.com/horse";

    //    using (UnityWebRequest web = UnityWebRequestAssetBundle.GetAssetBundle(uri))
    //    {
    //        yield return web.SendWebRequest();

    //        if (web.isNetworkError || web.isHttpError)
    //        {
    //            Debug.LogError("error: " + web.error);
    //            yield break;
    //        }

    //        AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(web);

    //        GameObject horse = Instantiate(assetBundle.LoadAsset("horse")) as GameObject;

    //    }
    //}

}
