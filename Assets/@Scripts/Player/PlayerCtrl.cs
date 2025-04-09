using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // private
    Animator _animator;
    float _speed;
    float _originSpeed = 7f;

    // public
    public GameObject Lettuce, Tomato, Beef, GroundBeef, Cheese, BurgerBun_T, BurgerBun_B; // 재료

    // property
    // 플레이어 애니메이션 파라미터
    public bool IsWalk
    {
        get { return _animator.GetBool(Define.IsWalk); }
        set { _animator.SetBool(Define.IsWalk, value); }
    }
    public bool IsRun
    {
        get { return _animator.GetBool(Define.IsRun); }
        set { _animator.SetBool(Define.IsRun, value); }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        // 원래 속도 저장
        _speed = _originSpeed;
    }

    void Update()
    {
        // 이동
        Move();
    }

    // 이동하는 함수
    void Move()
    {
        if (Input.GetButton(Define.Horizontal) || Input.GetButton(Define.Vertical))
        {
            float h = Input.GetAxis(Define.Horizontal);
            float v = Input.GetAxis(Define.Vertical);

            Vector3 movement = new Vector3(h, 0, v).normalized;
            transform.position += movement * Time.deltaTime * _speed;

            // 플레이어 이동 범위 제한
            float clampedX = Mathf.Clamp(transform.position.x, -10.2f, 10.2f);
            float clampedZ = Mathf.Clamp(transform.position.z, -6.2f, 5.8f);
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

            // 플레이어 앞방향 설정
            if (movement != Vector3.zero) transform.forward = movement;

            _animator.SetBool(Define.IsWalk, true);

            // 대쉬
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _speed = _originSpeed + 5;
                _animator.SetBool(Define.IsRun, true);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _speed = _originSpeed;
                _animator.SetBool(Define.IsRun, false);
            }
        }
        else
        {
            _animator.SetBool(Define.IsWalk, false);
            _animator.SetBool(Define.IsRun, false);
            _speed = _originSpeed;
        }
    }

    // 재료 꺼내는 함수
    public void OnTriggerStay(Collider other)
    {
        // 재료 들고 있으면 나가기
        if (PlayManager.Instance.IsHoldingIngredient) return;

        // 재료를 잡을 수 있고, 스페이스바를 눌렀으면
        if (PlayManager.Instance.CanPickUp && Input.GetKey(KeyCode.Space))
        {
            GameObject newIngredient = null;

            if (other.CompareTag(Define.LettuceCabinet)) newIngredient = Instantiate(Lettuce, PlayManager.Instance.Hand.position, Quaternion.identity);
            else if (other.CompareTag(Define.TomatoCabinet)) newIngredient = Instantiate(Tomato, PlayManager.Instance.Hand.position, Quaternion.identity);
            else if (other.CompareTag(Define.BeefCabinet)) newIngredient = Instantiate(Beef, PlayManager.Instance.Hand.position, Quaternion.identity);
            else if (other.CompareTag(Define.GroundBeefCabinet)) newIngredient = Instantiate(GroundBeef, PlayManager.Instance.Hand.position, Quaternion.identity);
            else if (other.CompareTag(Define.CheeseCabinet)) newIngredient = Instantiate(Cheese, PlayManager.Instance.Hand.position, Quaternion.identity);
            else if (other.CompareTag(Define.BurgerBun_TCabinet)) newIngredient = Instantiate(BurgerBun_T, PlayManager.Instance.Hand.position, Quaternion.identity);
            else if (other.CompareTag(Define.BurgerBun_BCabinet)) newIngredient = Instantiate(BurgerBun_B, PlayManager.Instance.Hand.position, Quaternion.identity);


            if (newIngredient != null)
            {
                PlayManager.Instance.HeldIngredient = newIngredient;

                PlayManager.Instance.HeldIngredient.transform.SetParent(PlayManager.Instance.Hand);
                PlayManager.Instance.HeldIngredient.transform.localPosition = Vector3.zero;
                PlayManager.Instance.HeldIngredient.transform.localRotation = Quaternion.identity;

                PlayManager.Instance.IsHoldingIngredient = true;
                PlayManager.Instance.CanPickUp = false;
            }
        }
    }
}
