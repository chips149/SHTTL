using UnityEngine;

public class Cake : MonoBehaviour
{
    [SerializeField] private Cooldown _cooldown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Marble") || !_cooldown.CanActivate()) return;

        _cooldown.Begin();

        MarbleBehavior marble = collision.GetComponent<MarbleBehavior>();
        marble.AttachCake();
    }

}
