using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flyingSpeed = 2f;
    private float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;
    public Collider2D deathCollider;
    
    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);
        }
    }

    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationString.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(onDeath);
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectionColliders.Count > 0;  
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (canMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        // Fly tonext waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //Check if we have reached to waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flyingSpeed;
        UpdateDirection();

        // See if we need to switch waypoints
        if(distance <= waypointReachedDistance)
        {
            // Switch to next waypoint
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                //Loop back to original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if (transform.localScale.x > 0)
        {
            // Facing the right
            if (rb.velocity.x < 0)
            {
                // Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);    
            }
        }
        else
        {
            //Facing the Left
            if (rb.velocity.x > 0)
            {
                // Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void onDeath()
    {
        // Dead flyier falls to the ground
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
