using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyState : MonoBehaviour
{
    public BattleScript battleScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHP;
    public float currentHP;
    public float savedDamage; // Store the original damage value for status effects to reference
    public HashSet<string> statusEffects = new HashSet<string>();
    Dictionary<string, Action> statusEffectActions;
    //HashSet<string> triggeredEffects = new HashSet<string>();
    List<string> effectsToRemove = new List<string>();
    //Burn status effect variables
    float burnDuration = 5f;
    float burnTickTimer = 0f;
    float burnElapsed = 0f;
    //Light Status effect Variables
    float lightMultiplier = 1f;
    //ice status effect variables
    int freezeCounter = 0;

    public void Damaged(float damage, string element = "none")
    {
        switch (element)
        {
        case "shock":
            if (statusEffects.Contains("water"))
            {
                Debug.Log("Shock + Wet combo: Damage doubled");
                damage *= 2f;
                effectsToRemove.Add("water");
            }
            break;

        case "water":
            if (statusEffects.Contains("fire"))
            {
                effectsToRemove.Add("fire"); 
                Debug.Log("Water extinguished fire");
            }
            break;

        case "fire":
            if (statusEffects.Contains("ice") || statusEffects.Contains("frostbite"))
            {
                effectsToRemove.Add("ice");
                effectsToRemove.Add("frostbite");
                Debug.Log("Fire melted ice/frostbite!");
            }

            break;
        case "ice":
            freezeCounter++;
            if (statusEffects.Contains("water"))
                {
                    freezeCounter += 3;
                    damage *= 2f; // Increase damage by 50% if enemy is wet and hit with ice
                }
            if (freezeCounter >= 3)
                {
                    statusEffects.Add("frostbite");
                    effectsToRemove.Add("ice");
                    Debug.Log("Enemy is frostbitten!");
                    freezeCounter = 0; // Reset counter after applying frostbite
                }
            break;

        case "earth":
            if (statusEffects.Contains("frostbite"))
            {
                damage *= 2f; // Increase damage by 100% if enemy is frostbitten and hit with earth
                Debug.Log("Frostbite + Earth combo: Damage increased to " + damage);
                effectsToRemove.Add("frostbite"); // Remove frostbite after applying the combo
            }

            break;

        case "air":

            break;
        
        case "light":
            if (statusEffects.Contains("light"))
            {
                damage *= lightMultiplier;
                lightMultiplier += 0.2f; // Increase multiplier for each additional light hit
                Debug.Log("Light hit! Current multiplier: " + lightMultiplier);
            }
            break;    

        case "dark":

            break;
    
        default:

            break;
        }
        Debug.Log("Enemy took damage: " + damage + " with status effects: " + string.Join(", ", statusEffects));
        currentHP -= damage;
        if (currentHP < 0)
            currentHP = 0;
        //Call die function if HP is 0 or less
        if (currentHP <= 0) Die();
    }
    public void Die()
    {
        
    if (battleScript != null)
        {
            battleScript.activeEnemies.Remove(gameObject);
        }

    Destroy(gameObject);
    }

    void Start()
    {
        currentHP = maxHP;
        statusEffectActions = new Dictionary<string, Action>()
        {
            { "fire", OnFire },
            { "water", Wet },
            { "earth", Tremor },
            { "air", Air },
            { "shock", Shocked },
            { "ice", Ice },
            { "light", Light },
            { "dark", Dark }
        };
    }
    void Update()
    {
        foreach (var effect in statusEffects)
        {
            if (statusEffectActions.ContainsKey(effect))
            {
                statusEffectActions[effect].Invoke();
            }
        }
        foreach (var effect in effectsToRemove)
        {
            statusEffects.Remove(effect);
        }
        effectsToRemove.Clear();
    }
    //Status effect for Fire
    void OnFire()
    {
        burnTickTimer += Time.deltaTime;
        burnElapsed += Time.deltaTime;

        if (burnTickTimer >= 1f)
        {
            burnTickTimer = 0f;
            Damaged(2f, "fire"); // mark as status damage
            Debug.Log("Burn tick!");
        }


        if (burnElapsed >= burnDuration)
        {
            effectsToRemove.Add("fire"); 
            burnElapsed = 0f;
        }
    }
    //Status effect for Water
    void Wet()
    {
        Debug.Log("Enemy is drenched!");
        //Remove burn status effect if enemy is on fire and gets wet
        if (statusEffects.Contains("fire"))        
        {
            effectsToRemove.Add("fire");
            Debug.Log("Water extinguished fire!");  
        }
    }
    //Status effect for Earth
    void Tremor()
    {
        Debug.Log("Enemy is grounded!");
    }
    //Status effect for Air
    void Air()
    {
        Debug.Log("Enemy is buffeted!");
    }
    //Status effect for Shock
    void Shocked()
    {
        Debug.Log(name + " releases shock!");
        //If enemy has Wet  status effect, damage is doubles


        float chainRadius = 3f;
        float chainDamage = savedDamage * 0.5f; // Chain damage is 10% of original damage

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, chainRadius);
        HashSet<EnemyState> shockedEnemies = new HashSet<EnemyState>();

        foreach (var hit in hits)
        {
            EnemyState enemy = hit.GetComponent<EnemyState>();
            //create new HashMap to track which enemies have already been shocked in this chain reaction

            if (enemy == null || enemy == this || shockedEnemies.Contains(enemy))
                continue;

            if (!enemy.statusEffects.Contains("shock"))
            {
                Debug.Log("Shock chaining to: " + enemy.name);

                enemy.Damaged(chainDamage, "shock"); // Mark as shock damage for potential further chaining
                shockedEnemies.Add(enemy);
            }
        }

    effectsToRemove.Add("shock");
    }
    //Status effect for Ice
    void Ice()
    {
        Debug.Log("Enemy is getting colder!");
    }
    //Status effect for Light
    void Light()
    {
        Debug.Log("Enemy is illuminated!");
    }
    //Status effect for Dark
    void Dark()
    {
        Debug.Log("Enemy is shadowed!");   
    }
}
