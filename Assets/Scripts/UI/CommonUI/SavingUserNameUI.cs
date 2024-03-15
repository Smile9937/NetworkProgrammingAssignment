using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingUserNameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameField;

    void Awake()
    {
        bool isDedicated = SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null;

        if(isDedicated)
        {
            GoToScene();
        }
        else
        {
            string userName = PlayerPrefs.GetString("userName");
            if(userName.Trim().Equals(string.Empty)) return;
            GoToScene();
        }
    }

    public void SaveButtonClicked()
    {
            string userName = userNameField.text;
            if(userName.Length >= 3 && userName.Length <= 20)
            {
                PlayerPrefs.SetString("userName", userName);
                GoToScene();
            }
    }

    private void GoToScene()
    {
        SceneManager.LoadScene("FirstScene");
    }
}
