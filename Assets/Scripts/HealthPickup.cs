using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestored = 10;

    public Vector3 spinRitationSpeed = new Vector3 (0, 180, 0);

    AudioSource pickupSource;

    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable && damageable.Health < damageable.MaxHealth)
        {
            bool wasHealed = damageable.Heal(healthRestored);

            if (wasHealed)
            {
                if (pickupSource)
                {
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                }

                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.eulerAngles += spinRitationSpeed * Time.deltaTime;
    }
}
