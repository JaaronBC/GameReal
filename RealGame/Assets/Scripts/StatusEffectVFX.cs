using UnityEngine;
using System.Collections.Generic;

public class StatusEffectVFX : MonoBehaviour
{
    public EnemyState enemy;

    [System.Serializable]
    public class EffectData
    {
        public string effectName;
        public GameObject prefab;
        public float spawnInterval = 0.5f;
    }

    public List<EffectData> effects;

    Dictionary<string, float> timers = new Dictionary<string, float>();

    void Update()
    {
        foreach (var effect in effects)
        {
            if (!enemy.statusEffects.Contains(effect.effectName))
                continue;

            if (!timers.ContainsKey(effect.effectName))
                timers[effect.effectName] = 0f;

            timers[effect.effectName] += Time.deltaTime;

            if (timers[effect.effectName] >= effect.spawnInterval)
            {
                SpawnEffect(effect.prefab);
                timers[effect.effectName] = Random.Range(0f, effect.spawnInterval);
            }
        }
    }

    void SpawnEffect(GameObject prefab)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.5f, 0.5f),
            0f
        );

        GameObject vfx = Instantiate(prefab, transform.position + randomOffset, Quaternion.identity);

        Destroy(vfx, 0.7f);
    }
}