using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UserProfile
{
    public string username;
    public string email;
    public CollectionData collections = new CollectionData();
    // 
    public UserProfile(string username, string email)
    {
        this.username = username;
        this.email = email;

    }
}


[System.Serializable]
public class CollectionData
{
    public CollectionType basic = new CollectionType();
}

[System.Serializable]
public class CollectionType
{
    public bool hasCurry = false;
    public bool hasWing = false;
    public bool hasFBalls = false;
    public bool hasSotong = false;
}