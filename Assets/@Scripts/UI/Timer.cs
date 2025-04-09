using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    // private
    Slider _timer;
    TMP_Text _timerText;
    float _time = 60f; // ���� �÷��� 60�� ���� ����
    int _minute;
    int _second;

    // public
    public Canvas GameOver;

    void Start()
    {

        _timer = GameObject.Find("TimerSlider").GetComponent<Slider>();
        _timerText = GameObject.Find("TimerText").GetComponent<TMP_Text>();

        StartCoroutine(CoTimer());
        StartCoroutine(CoTimerText());
    }

    IEnumerator CoTimer()
    {
        float timer = _time;
        _timer.value = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;
            _timer.value = 1 - (elapsedTime / timer);
            yield return null;
        }
    }

    IEnumerator CoTimerText()
    {
        float timer = _time;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            _minute = (int)timer / 60;
            _second = (int)timer % 60;
            // * 00:00�� ���� �������� ����ϱ� ����
            _timerText.text = _minute.ToString("00") + ":" + _second.ToString("00");
            yield return null;

            if (timer <= 0)
            {
                GameManager.Instance.PlayEndGameSound();
                GameManager.Instance.EndBGM();

                // �Ͻ�����
                Time.timeScale = 0;
                GameOver.gameObject.SetActive(true);

                // * WaitForSecondsRealtime
                // : timescale 0�� ���� �۵��ϵ��� ��
                yield return new WaitForSecondsRealtime(3f);

                SceneManager.LoadScene(Define.End);
                timer = 0;
                yield break;
            }
        }
    }
}
