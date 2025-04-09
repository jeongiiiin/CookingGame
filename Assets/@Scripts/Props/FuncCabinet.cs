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
        // 플레이어가 충돌하면 밝아지도록 함
        if (other.gameObject.CompareTag(Define.Player))
        {
            _material.SetColor("_EmissionColor", _emissionColor * _emissionIntensity);
            DynamicGI.SetEmissive(GetComponent<Renderer>(), _emissionColor * _emissionIntensity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 충돌 해제되면 원래 밝기로 돌아감
        if (other.gameObject.CompareTag(Define.Player))
        {
            _material.SetColor("_EmissionColor", _originEmissionColor);
            DynamicGI.SetEmissive(GetComponent<Renderer>(), _originEmissionColor);
        }
    }
}
