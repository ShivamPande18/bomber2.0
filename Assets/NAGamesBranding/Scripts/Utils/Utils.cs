using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Utils 
{
    public static IEnumerator DownLoadJson<T> ( string  url , T fromJson , Action<T, bool> callback ) {
        using (UnityWebRequest www = UnityWebRequest.Get ( url )) {

            yield return www.SendWebRequest ();

            if (www.isNetworkError || www.isHttpError) {
                Debug.LogError (www.error);
                callback( JsonUtility.FromJson<T>(string.Empty) , false );
            } else {
                if (www.isDone) {
                    Debug.Log (System.Text.Encoding.UTF8.GetString (www.downloadHandler.data));
                    string jsonData = System.Text.Encoding.UTF8.GetString (www.downloadHandler.data);
                    callback( JsonUtility.FromJson<T>(jsonData) , true );
                }
            }
        }
    }

    public static IEnumerator GetTextureRequest ( string url , int imgWidth, int imgHeight, Action<Sprite> callback )
    {
        using(var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    var rect = new Rect(0, 0, imgWidth, imgHeight);
                    var sprite = Sprite.Create(texture,rect,new Vector2(0.5f,0.5f));
                    callback ( sprite);
                }
            }
        }
    }

}
