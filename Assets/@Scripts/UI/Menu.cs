using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // private
    Button _playButton;
    Button _homeButton;
    Button _quitButton;

    void Start()
    {
        _playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        _homeButton = GameObject.Find("HomeButton").GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

        _playButton.onClick.AddListener(PlayButtonClick);
        _homeButton.onClick.AddListener(HomeButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    void PlayButtonClick()
    {
        GameManager.Instance.IsPause = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void HomeButtonClick()
    {
        SceneManager.LoadScene(Define.Main);
        Time.timeScale = 1f;
    }

    void QuitButtonClick()
    {
        // 유니티 에디터에서 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // 게임 종료
#else
            Application.Quit();
#endif
    }
}
