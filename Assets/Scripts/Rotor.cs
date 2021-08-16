using System;
using System.Collections;
using System.Collections.Generic;
using InputSwipe;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    [SerializeField] private SwipeDetection _swipeDetection;
    [SerializeField] private Pillar _pillar;
    [SerializeField] [Range(0,72)] private int _angel = 12;
    
    private List<GameObject> _sectors;

    private bool _canRotate = true;

    private int _delta;

    private const int Left = 1;
    private const int Right = -1;
    private const float RotateDegrees = 72;

    private void OnEnable()
    {
        _swipeDetection.OnSwipeRight += TurnRight;
        _swipeDetection.OnSwipeLeft += TurnLeft;
        _pillar.OnNewAssembly += ChangeMovedList;
    }

    private void OnDisable()
    {
        _swipeDetection.OnSwipeRight -= TurnRight;
        _swipeDetection.OnSwipeLeft -= TurnLeft;
        _pillar.OnNewAssembly -= ChangeMovedList;
    }

    private void TurnLeft()
    {
        if(_canRotate)
            StartCoroutine(Rotation(_angel, Left));
    }

    private void TurnRight()
    {
        if(_canRotate)
            StartCoroutine(Rotation(_angel, Right));
    }

    private IEnumerator Rotation(float angel, int direction)
    {
        _canRotate = false;
        while (RotateDegrees * _sectors.Count / angel - Math.Abs(_delta) > 0)
        {
            foreach (var sector in _sectors)
            {
                sector.transform.Rotate(Vector3.up * direction, angel);
                _delta++;
            }

            yield return new WaitForFixedUpdate();
        }

        _canRotate = true;
        _delta = 0;
    }

    private void ChangeMovedList(List<GameObject> sectors)
    {
        _sectors = sectors;
    }

}
