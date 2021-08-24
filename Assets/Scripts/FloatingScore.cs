using System.Collections;
using UnityEngine;

public class FloatingScore : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AnimationCurve _animationCurve;
    
    private Camera _camera;
    private Rigidbody _rigidbody;

    private float _currentTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveScores());
    }

    private IEnumerator MoveScores()
    {
        var positionCameraTopEdge =
            _camera.ScreenToWorldPoint(new Vector3(0, _camera.pixelHeight, _camera.nearClipPlane));
        var delta = Random.Range(1f, 2f);
        
        while (transform.position.y < positionCameraTopEdge.y)
        {
            var position = new Vector3(_animationCurve.Evaluate(_currentTime) * delta,
                transform.position.y + positionCameraTopEdge.y * (Time.deltaTime * _speed), 
                transform.position.z);
            
            _rigidbody.MovePosition(position);
            _currentTime += Time.deltaTime;
            yield return null;
        }

        _currentTime = 0;
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
