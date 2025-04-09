using UnityEngine;

public class FuncCabinet : MonoBehaviour
{
    // private
    Material _material;
    Color _originEmissionColor;
    Color _emissionColor = Color.gray;
    float _emissionIntensity = 1f;

    void Start()
    {
        _material = GetComponent<Renderer>().material;

        _originEmissionColor = _material.GetColor("_EmissionColor");
    }

    private void OnTriggerStay(Collider other)
    {
        // �÷��̾ �浹�ϸ� ��������� ��
        if (other.gameObject.CompareTag(Define.Player))
        {
            _material.SetColor("_EmissionColor", _emissionColor * _emissionIntensity);
            DynamicGI.SetEmissive(GetComponent<Renderer>(), _emissionColor * _emissionIntensity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ �浹 �����Ǹ� ���� ���� ���ư�
        if (other.gameObject.CompareTag(Define.Player))
        {
            _material.SetColor("_EmissionColor", _originEmissionColor);
            DynamicGI.SetEmissive(GetComponent<Renderer>(), _originEmissionColor);
        }
    }
}
