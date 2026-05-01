using UnityEngine;
using System.Collections.Generic;
using System;
public class ProjectileScript : MonoBehaviour
{
    public Transform target;
    public GameObject targetObject;
    public float damage;
    public float speed = 10f;
    public string shape;
    public string element;
    public bool piercing = false; 
    Vector3 lastPosition;
    float distanceTraveled = 0f;
    public float maxRange = 10f;
    Vector3 moveDirection;
    Dictionary<string, Action<ProjectileScript>> shapeActions; 
    HashSet<EnemyState> piercedEnemies = new HashSet<EnemyState>(); 
    void Start()
    {
        lastPosition = transform.position;
        if (target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
        }
        else
        {
            // fallback so it doesn't crash
            moveDirection = transform.up; 
        }

        shapeActions = new Dictionary<string, Action<ProjectileScript>>()
        {
            { "bolt", (proj) => proj.Bolt() },
            { "ball", (proj) => proj.Ball() },
            { "missile", (proj) => proj.Missile() },
            { "beam", (proj) => proj.Beam() },
            { "slash", (proj) => proj.Slash() },
            { "spear", (proj) => proj.Spear() },
            { "drill", (proj) => proj.Drill() },
            {"sword", (proj) => proj.Sword() },
            {"dagger", (proj) => proj.Dagger() },
            {"arrow", (proj) => proj.Arrow() },
            {"star", (proj) => proj.Star() },
            {"wave", (proj) => proj.Wave() },
            {"vortex", (proj) => proj.Vortex() },
            {"punch", (proj) => proj.Punch() }
        };
    }
    void Update()
    {
        if (target == null && !piercing)
        {
            Destroy(gameObject);
            return;
        }

        lastPosition = transform.position;

        transform.position += moveDirection * speed * Time.deltaTime;

        float frameDistance = Vector3.Distance(lastPosition, transform.position);
        distanceTraveled += frameDistance;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        if (piercing)
        {
            if (shapeActions != null && shapeActions.ContainsKey(shape))
            {
                shapeActions[shape].Invoke(this);
            }

            if (distanceTraveled >= maxRange)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                OnHit();
                Destroy(gameObject);
            }
        }
    }

   void OnHit()
    {
        Debug.Log("Projectile hit target: " + targetObject.name + " with shape: " + shape + " and damage: " + damage);
        if (shapeActions != null && shapeActions.ContainsKey(shape))
        {
            shapeActions[shape].Invoke(this);
        }
    }
    void Bolt()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Ball()
    {
        float radius = 2f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            EnemyState enemy = hit.GetComponent<EnemyState>();
            if (enemy != null)
            {
                enemy.statusEffects.Add(element); // Add element as status effect to enemy
                enemy.savedDamage = damage; // Store the original damage value for status effects to reference
                enemy.Damaged(damage, element); // Apply damage to enemy
            }
        }
    }
    void Missile()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Beam()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Slash()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Spear()
    {
        float pierceRadius = 0.5f;

        Vector3 movement = transform.position - lastPosition;
        float distance = movement.magnitude;

        if (distance <= 0f) return;

        Vector3 direction = movement.normalized;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(lastPosition, pierceRadius, direction, distance);

        foreach (var hit in hits)
        {
            EnemyState enemy = hit.collider.GetComponent<EnemyState>();
            if (enemy != null && !piercedEnemies.Contains(enemy))
            {
                piercedEnemies.Add(enemy);

                enemy.statusEffects.Add(element);
                enemy.savedDamage = damage;
                enemy.Damaged(damage, element);
            }
        }
    }
    void Drill()
    {
        float pierceRadius = 1.0f;

        Vector3 movement = transform.position - lastPosition;
        float distance = movement.magnitude;

        if (distance <= 0f) return;

        Vector3 direction = movement.normalized;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(lastPosition, pierceRadius, direction, distance);

        foreach (var hit in hits)
        {
            EnemyState enemy = hit.collider.GetComponent<EnemyState>();
            if (enemy != null && !piercedEnemies.Contains(enemy))
            {
                piercedEnemies.Add(enemy);
                for (int i = 0; i < 5; i++) // Drill hits 5 times
                {
                    enemy.statusEffects.Add(element);
                    enemy.savedDamage = damage; // Drill does 20% damage on each hit
                    enemy.Damaged(damage, element);
                }
            }
        }
    }
    void Sword()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Dagger()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Arrow()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Star()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            //star hits twice but at half damage, so we call the damage function twice with half damage
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy

            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
    void Wave()
    {
        float pierceRadius = 3f;

        Vector3 movement = transform.position - lastPosition;
        float distance = movement.magnitude;

        if (distance <= 0f) return;

        Vector3 direction = movement.normalized;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(lastPosition, pierceRadius, direction, distance);

        foreach (var hit in hits)
        {
            EnemyState enemy = hit.collider.GetComponent<EnemyState>();
            if (enemy != null && !piercedEnemies.Contains(enemy))
            {
                piercedEnemies.Add(enemy);

                enemy.statusEffects.Add(element);
                enemy.savedDamage = damage;
                enemy.Damaged(damage, element);
            }
        }
    }
    void Vortex()
    {
        float pierceRadius = 3.0f;

        Vector3 movement = transform.position - lastPosition;
        float distance = movement.magnitude;

        if (distance <= 0f) return;

        Vector3 direction = movement.normalized;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(lastPosition, pierceRadius, direction, distance);

        foreach (var hit in hits)
        {
            EnemyState enemy = hit.collider.GetComponent<EnemyState>();
            if (enemy != null && !piercedEnemies.Contains(enemy))
            {
                piercedEnemies.Add(enemy);
                for (int i = 0; i < 5; i++) // Drill hits 5 times
                {
                    enemy.statusEffects.Add(element);
                    enemy.savedDamage = damage; // Drill does 20% damage on each hit
                    enemy.Damaged(damage, element);
                }
            }
        }
    }
    void Punch()
    {
        if (targetObject == null) return;
        EnemyState enemy = targetObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.statusEffects.Add(element); // Add element as status effect to enemy
            enemy.savedDamage = damage; // Store the original damage value for status effects to reference
            enemy.Damaged(damage, element); // Apply damage to enemy
        }
    }
}