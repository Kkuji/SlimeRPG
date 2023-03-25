using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private SlimeShooting _slimeShooting;
    [SerializeField] private float _speed;

    private RectTransform _rectTransform;
    private Vector3 _defaultPosition;
    private bool _canMove = false;

    private void Awake()
    {
        _slimeShooting.StartMovingAction += MoveBackground;
        _slimeShooting.StopMovingAction += StopBackground;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultPosition = _rectTransform.localPosition;
    }

    private void Update()
    {
        if (_canMove)
        {
            _rectTransform.localPosition += Vector3.left * _speed * Time.deltaTime;
        }

        if (_rectTransform.localPosition.x < -400)
        {
            _rectTransform.localPosition = _defaultPosition;
        }
    }

    private void MoveBackground()
    {
        _canMove = true;
    }

    private void StopBackground()
    {
        _canMove = false;
    }

    private void OnDestroy()
    {
        _slimeShooting.StartMovingAction -= MoveBackground;
        _slimeShooting.StartMovingAction -= StopBackground;
    }
}