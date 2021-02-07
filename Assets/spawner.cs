using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;
using UnityEngine.Networking;

public class spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject imageObject;
    private Post[] posts;
    private List<Post> postList;
    private string jsonResponse;
    public static string username;
    public static string password;
    private string str;
    void Start()
    {
        Debug.Log("anddeb - start");
        str = String.Format("https://social-simplicity-21.herokuapp.com/get-posts-unity?username={0}&password={0}", username, password);
        Debug.Log("anddeb - "+ str);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(str);
        Debug.Log("anddeb - req");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Debug.Log("anddeb - res");
        StreamReader reader = new StreamReader(response.GetResponseStream());
        jsonResponse = reader.ReadToEnd();
        Debug.Log("anddeb - read");
        //JsonUtility.FromJson(jsonResponse);
        //posts = JsonHelper.FromJson<Post>(fixJson(jsonResponse));
        
        //Debug.Log("anddeb - JsonHelper");
        //Debug.Log("anddeb - " + posts.ToString());
        //Debug.Log("anddeb - ?");
        //postList = posts.OfType<Post>().ToList();
        //Debug.Log("anddeb - List");
        //Debug.Log("anddeb - " + postList.ToString());
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount <= 0)
            return;
        Debug.Log("anddeb - Touch");
        Touch touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Began)
        {
            var planeManager = GetComponent<ARPlaneManager>();
            float maxSize = 0;
            Vector3 finalVec = new Vector3(0,0,0);
            foreach (ARPlane plane in planeManager.trackables)
            {
                if(plane.alignment == PlaneAlignment.HorizontalUp)
                {
                    if (plane.extents.x * plane.extents.y > maxSize)
                    {
                        maxSize = plane.extents.x * plane.extents.y;
                        Debug.Log("floor");
                        Vector3 vec = plane.center;
                        Vector3 xDist = new Vector3(plane.extents.x, 0, 0);
                        Vector3 zDist = new Vector3(0, 0, plane.extents.y);
                        float deltaX = Random.Range(-1 * Mathf.Abs(plane.extents.x), Mathf.Abs(plane.extents.x));
                        float deltaZ = Random.Range(-1 * Mathf.Abs(plane.extents.y), Mathf.Abs(plane.extents.y));

                        finalVec = vec + new Vector3(deltaX, 0, deltaZ);
                    }
                }
            }

            GameObject gameObject = Instantiate<GameObject>(ARObjectManager.getObject("node_MeshObject1969818624-PolyPaper20"), finalVec, Quaternion.identity);
            gameObject.transform.localScale = new Vector3(.2f, .2f, .2f);

            //postList.RemoveAt(0);

            Material quadMaterial = (Material)Resources.Load("red");
            imageObject.GetComponent<Renderer>().material = quadMaterial;

            //imageInstance.transform.localPosition += new Vector3(0, 1, 0);

            //imageInstance
            //Debug.Log("anddeb - p -" + N[0]["displayUrl"].Value);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(str);
            Debug.Log("anddeb - req");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Debug.Log("anddeb - res");
            StreamReader reader = new StreamReader(response.GetResponseStream());
            jsonResponse = reader.ReadToEnd();
            Debug.Log("url+");
            Debug.Log(jsonResponse);
            Debug.Log("url-");
            StartCoroutine(DownloadImage(jsonResponse, imageObject, gameObject));

            //Debug.Log("anddeb - p -" + N[0]["displayUrl"].Value);
            //post.displayUrl;
            //gameObject.chil=
        }
    }

    IEnumerator DownloadImage(string MediaUrl, GameObject imgObj, GameObject gameObject)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            // ImageComponent.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;

            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            imgObj.GetComponent<Renderer>().material.mainTexture = tex;
            GameObject imageInstance = Instantiate<GameObject>(imgObj, gameObject.transform);
        }
    }
}


[System.Serializable]
public class Post
{
    public string displayUrl;
    public string caption;
    public string timestamp;
    public string video;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}