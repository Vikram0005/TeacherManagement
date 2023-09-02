using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    public NetConfig netConfig;

    private void Awake()
    {
        Instance = this;
    }

    public void CallAPI<REQ, RES>(PacketType packetType, REQ data, Action<RES> callback)
    {
        StartCoroutine(PostRequest<REQ, RES>(packetType, data, callback));
    }

    public void CallAPI<RES>(PacketType packetType, Action<RES> callback)
    {
        StartCoroutine(GetRequest<RES>(packetType,callback));
    }

    private IEnumerator PostRequest<REQ, RES>(PacketType packetType, REQ data, Action<RES> callback)
    {
        string jsonData = JsonUtility.ToJson(data);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(GetUrlFromPacketType(packetType), jsonData))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("<color=yellow>Response :" + webRequest.downloadHandler.text + "</color>");
                    RES obj = JsonUtility.FromJson<RES>(webRequest.downloadHandler.text);
                    callback?.Invoke(obj);
                    break;
            }
        }
    }



    private IEnumerator GetRequest<T>(PacketType packetType, Action<T> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(GetUrlFromPacketType(packetType)))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("<color=yellow>Response :" + webRequest.downloadHandler.text +"</color>");
                    T obj = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    //if(callback!=null)
                    callback?.Invoke(obj);
                    break;
            }
        }
    }

    private IEnumerator GetRequest<T>(PacketType packetType, string id, Action<T> callback)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(GetUrlFromPacketType(packetType) + id);
        yield return webRequest.SendWebRequest();
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                T obj = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                callback?.Invoke(obj);
                break;
        }
    }


    private IEnumerator DeleteRequest<T>(PacketType packetType, string id, Action<T> callback)
    {
        UnityWebRequest webRequest = UnityWebRequest.Delete(GetUrlFromPacketType(packetType) + id);
        yield return webRequest.SendWebRequest();
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequest.error);
                //callback?.Invoke(false);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error: " + webRequest.error);
                //callback?.Invoke(false);
                break;
            case UnityWebRequest.Result.Success:
                T obj = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                callback?.Invoke(obj);
                break;
        }
    }

    private string GetUrlFromPacketType(PacketType packetType)
    {
        UrlConfig urlConfig = netConfig.urlConfigs.Find(u => u.packetType == packetType);
        return netConfig.baseUrl + urlConfig.url;
    }
}
