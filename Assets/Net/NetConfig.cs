using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =nameof(NetConfig))]
public class NetConfig : ScriptableObject
{
    public string baseUrl;
    public List<UrlConfig> urlConfigs;
}


[System.Serializable]
public class UrlConfig
{
    public PacketType packetType;
    public string url;
}

public enum PacketType
{
    None=0,
    AddDepartment,
    GetAllDepartment
}
