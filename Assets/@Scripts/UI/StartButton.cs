using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // private
    Image _image;
    float _timeElapsed = 0f;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        // ��ư ����� ������������ ����ϱ� ����
        _timeElapsed += Time.deltaTime * 5f;

        float hue = Mathf.PingPong(_timeElapsed * 0.1f, 1f);
        _image.color = Color.HSVToRGB(hue, 1f, 1f);
    }
}
