using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    // private
    Button _startButton;
    Button _quitButton;

    void Start()
    {
        _startButton = GameObject.Find("StartButton").GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

        // 버튼 클릭 처리
        _startButton.onClick.AddListener(StartButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    void StartButtonClick()
    {
        SceneManager.LoadScene(Define.Stage);
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
