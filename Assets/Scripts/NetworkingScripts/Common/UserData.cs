using Unity.Services.Authentication;
using UnityEngine;

public class UserData
{
    public string userName;
    public string userAuthId;
    public ulong networkID;

    public GameData userGamePreferences;
}

public static class UserDataWrapper
{
    private static UserData userData;

    public static UserData GetUserData()
    {
        return userData;
    }
    public static byte[] PayLoadInBytes()
    {
        userData = new UserData()
        {
            userName = PlayerPrefs.GetString("userName", "Unknown Name"),
            userAuthId = AuthenticationService.Instance.PlayerId,
            userGamePreferences = new GameData()
        };

        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

        return payloadBytes;
    }
}