using UnityEngine;

public class BucketHeightPoint : MonoBehaviour
{
    [SerializeField] private Bucket _bucket;
    private float _height;
    void Update()
    {
        transform.position = new Vector3(0, _height, 0);
    }

    private void OnEnable()
    {
        _bucket.OnChangeBucketHeight += ChangeHeight;
    }

    private void OnDisable()
    {
        _bucket.OnChangeBucketHeight -= ChangeHeight;
    }

    private void ChangeHeight(float height)
    {
        _height = height;
    }
}
