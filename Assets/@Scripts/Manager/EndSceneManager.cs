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
        // 일시정지 해제
        Time.timeScale = 1f;

        _coin = GameObject.Find("Coin").GetComponent<TMP_Text>();
        _restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

        // 버튼 클릭 처리
        _restartButton.onClick.AddListener(RestartButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    void Update()
    {
        // 코인 출력
        string coinText = string.Format("{0:n0}", GameManager.Instance.Coin);
        _coin.text = $"{coinText}";
    }

    // 재시작 버튼 클릭 함수
    void RestartButtonClick()
    {
        // 게임씬으로 이동
        SceneManager.LoadScene(Define.Stage);
    }

    // 종료 버튼 클릭 함수
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
