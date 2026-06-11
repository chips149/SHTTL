using UnityEngine;

public class Follower : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 offset;
    [HideInInspector] public float smoothTime = 0.12f;
    [SerializeField] private float wobbleAmount = 0.15f;
    [SerializeField] private float wobbleSpeed = 3f;

    private Vector3 _velocity;
    private float _seed;

    public void StartFollow(Transform target, Vector3 offset, float smoothTime)
    {
        this.target = target;
        this.offset = offset;
        this.smoothTime = smoothTime;
        _velocity = Vector3.zero;
        _seed = Random.Range(0f, 100f);
    }

    public void StopFollow()
    {
        target = null;
    }

    public virtual void OnDetach() { }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 wobble = new Vector3(
            Mathf.Sin(Time.time * wobbleSpeed + _seed) * wobbleAmount,
            Mathf.Cos(Time.time * wobbleSpeed * 0.7f + _seed * 1.3f) * wobbleAmount,
            0
        );

        Vector3 targetPos = target.position + offset + wobble;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, smoothTime);
    }
}
