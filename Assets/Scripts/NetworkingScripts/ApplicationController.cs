using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        bool isGraphicCardDoesntExist = SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null;
        StartInMode(isGraphicCardDoesntExist);
    }

    private async void StartInMode(bool isGraphicsDoesntExist)
    {
        if(isGraphicsDoesntExist)
        {
            await ServerSingleton.GetInstance().InitServerAsync();
            await ServerSingleton.GetInstance().StartServerAsync();
        }
        else
        {
            await HostSingleton.GetInstance().InitServerAsync();
            await ClientSingleton.GetInstance().InitClientAsync();

            if(ClientSingleton.GetInstance().isAuth)
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
}
