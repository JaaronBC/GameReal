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
    Dictionary<string, Action<ProjectileScript>> shapeActions; 
    void Start()
    {
        shapeActions = new Dictionary<string, Action<ProjectileScript>>()
        {
            { "bolt", (proj) => proj.Bolt() },
            { "ball", (proj) => proj.Ball() },
            { "missile", (proj) => proj.Missile() },
            { "beam", (proj) => proj.Beam() },
            { "slash", (proj) => proj.Slash() }
        };
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle-90f);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            OnHit();
            Destroy(gameObject);
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
}