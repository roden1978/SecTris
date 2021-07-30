using System;
using System.Collections;
using System.Collections.Generic;
using InputSwipe;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    [SerializeField] private SwipeDetection _swipeDetection;
    [SerializeField] private Game _game;
    [SerializeField] private Pillar _pillar;
    [SerializeField] private float _speed;
    private List<GameObject> _sectors;

    private float _bucketHeight;
    private float _delta;
    
    private Quaternion _nextDegree;
     
    private const int Left = 1;
    private const int Right = -1;
    private const float RotateDegrees = 72;
    private void OnEnable()
    {
        _swipeDetection.OnSwipeRight += TurnRight;
        _swipeDetection.OnSwipeLeft += TurnLeft;
        //_game.OnChangeBucketHeight += ChangeBucketHeight;
        _pillar.OnNewAssembly += ChangeMovedList;
    }

    private void OnDisable()
    {
        _swipeDetection.OnSwipeRight -= TurnRight;
        _swipeDetection.OnSwipeLeft -= TurnLeft;
        //_game.OnChangeBucketHeight -= ChangeBucketHeight;
        _pillar.OnNewAssembly -= ChangeMovedList;
    }

    private void TurnLeft()
    {
        //var originalRot = transform.rotation;
        //_nextDegree = originalRot * Quaternion.AngleAxis(RotateDegrees * Left, Vector3.up);
        StartCoroutine(Rotation(_speed, Left));
    }

    private void TurnRight()
    {
        //var originalRot = transform.rotation;
        //_nextDegree = originalRot * Quaternion.AngleAxis(RotateDegrees * Right, Vector3.up);
        StartCoroutine(Rotation(_speed, Right));
    }
    
    private IEnumerator Rotation(float speed, int direction)
    {
        while (Math.Abs(_delta) < RotateDegrees * _sectors.Count)
        {
            foreach (var sector in _sectors)
            {
                 var tmp = sector.transform.rotation;
                 sector.transform.Rotate(Vector3.up * (Time.deltaTime * speed * direction));
                 _delta += Quaternion.Angle(tmp, sector.transform.rotation);
            }
           
            yield return null;
        }
        _delta = 0.0f;
    }

    private void ChangeMovedList(List<GameObject> sectors)
    {
        _sectors = sectors;
    }
    
    /*private void RotateSectors(float degrees, int direction)
    {
        var minPoint = _game.BucketHeight + _sectorHeight;
        if (!_rigidbody.isKinematic && 
            transform.position.y > minPoint &&
            transform.rotation != _nextDegree)
        {
            var originalRot = transform.rotation;    
            transform.rotation = Quaternion.Slerp(originalRot, 
                originalRot * Quaternion.AngleAxis(degrees * direction, Vector3.up),
                Time.deltaTime * _rotateSpeed);
        }
    }*/

    /*private void ChangeBucketHeight(float height)
    {
        _bucketHeight = height;
    }*/
}

/*class test : MonoBehaviour
{
    public float rotationSpeed = 100.0f;

    float delta = 0.0f;

    Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
        StartCoroutine(ButtonRotation());
    }

    IEnumerator<> ButtonRotation()
    {
        while (true)
        {
            Quaternion tmp = transform.rotation;
            transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * -rotationSpeed);
            delta += Quaternion.Angle(tmp, transform.rotation);

            if (delta > 360.0f)
            {
                delta = 0.0f;
                transform.rotation = startRotation;
                yield return new WaitForSeconds(1);
            }

            yield return null;
        }
    }
}*/
