using UnityEngine;

public class BucketHeightPoint : MonoBehaviour
{
    [SerializeField] private Game _game;
    private float _height;
    void Update()
    {
        transform.position = new Vector3(0, _height, 0);
    }

    private void OnEnable()
    {
        _game.OnChangeBucketHeight += ChangeHeight;
    }

    private void OnDisable()
    {
        _game.OnChangeBucketHeight -= ChangeHeight;
    }

    private void ChangeHeight(float height)
    {
        _height = height;
    }
}
