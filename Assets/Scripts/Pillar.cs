using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pillar : MonoBehaviour
{
    [SerializeField] private TorsBuilder _torsBuilder;
    [SerializeField] private Transform _downPoint;

    private int _count;
    private bool _create;
    private int _activeSectors;
    private float[] _yPositions;
    
    private List<GameObject> _moved;
    private List<GameObject> _fixed;

    private const float STEP = 0.4f;
    private const float DOWN_POINT_START_POSITION_Y = 0.05f;
    private const float STOP_POINT = 5.0f;

    private void Start()
    {
        _moved = _fixed = new List<GameObject>();
        _activeSectors = 0;
        _create = true;
        StartCoroutine(SpawnSectors(.1f));
        StartCoroutine(RemoveNotActive(.5f));
        StartCoroutine(NewDownPointPosition(.5f));
    }

    private IEnumerator RemoveNotActive(float times)
    {
        while (_create)
        {
            yield return new WaitForSeconds(times);
            _fixed.RemoveAll(NotActive);
        }
    }

    private bool NotActive(GameObject obj)
    {
        return !obj.activeInHierarchy;
    }

    private IEnumerator SpawnSectors(float times)
    {
        while (_create)
        {
            yield return new WaitForSeconds(times);
            var downPointY = _downPoint.position.y;
            foreach (var item in _moved)
            {
                var positionY = item.transform.position.y;

                if (positionY > downPointY) _activeSectors++;
                if (downPointY > STOP_POINT)
                {
                    _create = false;
                    _downPoint.position = new Vector3(0, DOWN_POINT_START_POSITION_Y, 0);
                }
            }

            if (!_create) continue;
            if (_activeSectors > 0) _activeSectors = 0;
            else
            {
                foreach (var item in _moved)
                {
                    _fixed.Add(item);
                }
                _moved.Clear();
                _moved = _torsBuilder.BuildingTor();
            }
        }
        Debug.Log("Game over");
    }

    private IEnumerator NewDownPointPosition(float times)
    {
        while(_create)
        {
            yield return new WaitForSeconds(times);
            var yPosition = new MaxYPosition(_fixed).value() + STEP;
            _downPoint.position = new Vector3(0, yPosition, 0);
        }
    }
}