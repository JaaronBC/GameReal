using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class EnemyState : MonoBehaviour
{
    public TextMeshProUGUI healthText;
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
    //ice status effect variables
    public int freezeCounter = 0; 
    //Dark status effect variables
    int darkMultiplier = 1;

    [SerializeField] GameObject lightningPrefab;
    [SerializeField] GameObject darkVFXPrefab;

    public void Damaged(float damage, string element = "none")
    {
        if (statusEffects.Contains("light"))
        {
            damage *= 1.2f; // Increase damage by 20% if enemy is illuminated
            Debug.Log("Light makes the enemy more vulnerable! " + damage);
        }
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
            if (statusEffects.Contains("frostbite"))
            {
                damage *= 1.5f; // Increase damage by 50% if enemy is frostbitten and hit with water
                Debug.Log("Water intensifies frostbite! " + damage);
            }
            break;

        case "fire":
            if (statusEffects.Contains("ice") || statusEffects.Contains("frostbite"))
            {
                effectsToRemove.Add("ice");
                effectsToRemove.Add("frostbite");
                Debug.Log("Fire melted ice/frostbite!");
            } 
            if (statusEffects.Contains("air"))
            {
                damage *= 2f; // Increase damage by 100% if enemy is hit with fire while airborne
                Debug.Log("The Flames intensify! " + damage);
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
                    freezeCounter = 0; 
                    if (!statusEffects.Contains("frostbite"))
                    {
                        statusEffects.Add("frostbite");
                        currentHP -= 8f;
                    }
                    effectsToRemove.Add("ice");
                    Debug.Log("Enemy is frostbitten!");
                    freezeCounter = 0; // Reset counter after applying frostbite
                }
            break;

        case "earth":
            if (statusEffects.Contains("frostbite"))
            {
                damage *= 2f; // Increase damage by 100% if enemy is frostbitten and hit with earth
                Debug.Log("The Earth Breaks the Ice: " + damage);
                effectsToRemove.Add("frostbite"); // Remove frostbite after applying the combo
            }

            break;

        case "air":

            break;
        
        case "light":
                darkMultiplier = 1;
                Debug.Log("Light reinvigorates the dark: " + darkMultiplier);
            break;    

        case "dark":
            //Deals more damage the more effects are in StatusEffects
            damage *= darkMultiplier;
            damage *= (1 + 0.5f * statusEffects.Count);
            Debug.Log("Dark hit! Base damage increased by " + (0.5f * statusEffects.Count * 100) + "% due to " + statusEffects.Count + " existing status effects.");
            if (darkMultiplier == 1)
            {
                DarkEffect();
            }
            if (statusEffects.Contains("dark"))
            {
                darkMultiplier = 0; // Reduce damage by 100% if enemy has already been hit by dark
            }
            //Remove all statusEffects
            statusEffects.Clear();
            Debug.Log("Darkness consumes all other effects!");
            break;
        default:

            break;
        }
        if (statusEffects.Contains("frostbite"))
        {
            damage *= 1.2f; // Increase damage by 20% if enemy is frostbitten
             Debug.Log("Frostbite increases damage taken: " + damage);
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
            // Remove this enemy from the activeEnemies list in BattleScript
            battleScript.activeEnemies.Remove(gameObject);
            battleScript.CheckForBattleEnd();
        }
    PlayerCasting playerCasting = FindObjectOfType<PlayerCasting>();
    Destroy(gameObject);
    playerCasting.CycleTarget();
    playerCasting.VeryifyTarget();
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
        healthText.text = currentHP.ToString("0") + " / " + maxHP.ToString("0");
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
        //Debug.Log("Enemy is drenched!");
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
        //Debug.Log("Enemy is grounded!");
    }
    //Status effect for Air
    void Air()
    {
        //Debug.Log("The air around the enemy is unstable!");
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

                SpawnLightningVFX(transform.position, enemy.transform.position, lightningPrefab);

                enemy.Damaged(chainDamage, "shock"); // Mark as shock damage for potential further chaining
                shockedEnemies.Add(enemy);
            }
        }

    effectsToRemove.Add("shock");
    }
    //Status effect for Ice
    void Ice()
    {
        //Debug.Log("Enemy is getting colder!");
    }
    //Status effect for Light
    void Light()
    {
        //Debug.Log("Enemy is illuminated!");
    }
    //Status effect for Dark
    void Dark()
    {
        //Debug.Log("Enemy is shadowed!");   
        //DarkEffect();
    }
    void SpawnLightningVFX(Vector3 start, Vector3 end, GameObject prefab)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        Vector3 midPoint = (start + end) / 2f;

        GameObject vfx = Instantiate(prefab, midPoint, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        vfx.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        Vector3 scale = vfx.transform.localScale;
        scale.x = distance;
        vfx.transform.localScale = scale;

        Destroy(vfx, 0.3f);
    }
    public void DarkEffect()
    {
        int count = 8;

        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 0.4f;

            Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            GameObject vfx = Instantiate(darkVFXPrefab, spawnPos, Quaternion.identity);

            Destroy(vfx, 0.4f);
        }
    }
}
