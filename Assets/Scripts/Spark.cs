using System.Collections;
using UnityEngine;

public class Spark : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Scores _target;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _target = FindObjectOfType<Scores>();
        //Debug.Log(_target.transform.position);
    }

    private void OnEnable()
    {
        StartCoroutine(MoveSpark());
    }

    private IEnumerator MoveSpark()
    {
        while (transform.position.y < _target.transform.position.y)
        {
            //Debug.Log(Vector3.Distance(transform.position, _target.transform.position));
            _rigidbody.MovePosition(transform.position + _target.transform.position * (Time.deltaTime * _speed));
            yield return null;
        }
        
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
