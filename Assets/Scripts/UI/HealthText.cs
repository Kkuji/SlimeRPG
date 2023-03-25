using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private float _timeToLive;
    [SerializeField] private float _speed;

    private RectTransform _rectTransform;
    private float _timeLived = 0f;
    private Color _color;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _color = _textMesh.color;
    }

    private void Update()
    {
        _timeLived += Time.deltaTime;
        _rectTransform.position += Vector3.up * _speed * Time.deltaTime;
        _textMesh.color = new Color(_color.r, _color.g, _color.b, 1 - (_timeLived / _timeToLive));

        if (_timeLived > _timeToLive)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        _textMesh.SetText(text);
    }
}