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

        // ��ư Ŭ�� ó��
        _startButton.onClick.AddListener(StartButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    void StartButtonClick()
    {
        SceneManager.LoadScene(Define.Stage);
    }

    void QuitButtonClick()
    {
        // ����Ƽ �����Ϳ��� ���� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // ���� ����
#else
            Application.Quit();
#endif
    }
}
