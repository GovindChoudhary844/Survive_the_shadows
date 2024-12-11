using UnityEngine;
using UnityEngine.Events;


public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent <int, int> healthChanged;

    [SerializeField] private int _maxHealth = 100;
   
    Animator animator;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField] private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

            // If the healthe below 0, character is no more alive
            if (_health <= 0 )
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField] private bool _isAlive = true;

    [SerializeField] private bool isInvinciable;

    // The velocity should not be changed while this is true but needs to be respected bu other physics components like the player controller
    public bool LockVelocity { get
        {
            return animator.GetBool(AnimationString.lockVelocity);
        }
        private set
        { 
            animator.SetBool(AnimationString.lockVelocity, value);
        }
    }

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value);   
            Debug.Log("IsAlive set :" + value);

            if(value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvinciable)
        {
            if (timeSinceHit > invincibilityTime)
            {
                // Remove invincibility
                isInvinciable = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    // Returning damageable took damage or not
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvinciable)
        {
            Health -= damage;
            isInvinciable = true;

            //Notify other subscribed components that the damageable was hit to handle the knockback as such
            animator.SetTrigger(AnimationString.hitTrigger);
            LockVelocity = true;
            Debug.Log("Invoking damageableHit event");
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        //Unable to be hit
        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }

}
