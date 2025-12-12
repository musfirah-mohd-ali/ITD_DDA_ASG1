using UnityEngine;

[System.Serializable]
public class UserProfile
{
    public string username;
    public string email;

    public UserProfile(string username, string email)
    {
        this.username = username;
        this.email = email;
    }
}
