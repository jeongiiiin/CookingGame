using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class EndSceneManager : MonoBehaviour
{
    // private
    TMP_Text _coin;
    Button _restartButton;
    Button _quitButton;

    void Start()
    {
        // �Ͻ����� ����
        Time.timeScale = 1f;

        _coin = GameObject.Find("Coin").GetComponent<TMP_Text>();
        _restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

        // ��ư Ŭ�� ó��
        _restartButton.onClick.AddListener(RestartButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    void Update()
    {
        // ���� ���
        string coinText = string.Format("{0:n0}", GameManager.Instance.Coin);
        _coin.text = $"{coinText}";
    }

    // ����� ��ư Ŭ�� �Լ�
    void RestartButtonClick()
    {
        // ���Ӿ����� �̵�
        SceneManager.LoadScene(Define.Stage);
    }

    // ���� ��ư Ŭ�� �Լ�
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
