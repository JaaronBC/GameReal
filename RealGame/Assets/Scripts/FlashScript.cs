using System;
using UnityEngine;

public class FlashScript : MonoBehaviour
{
    private float flashDecaySpeed = 10.0f;

    private SpriteRenderer[] spriteRenderers;
    private MaterialPropertyBlock propertyBlock;
    private float flashFactor;

    private void Start()
    {
       spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
       propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (flashFactor <= 0f)
        {
            return;
        }
        flashFactor = Mathf.Lerp(flashFactor, 0f, Time.deltaTime * flashDecaySpeed);
        if (flashFactor <= 0.01f)
        {
            flashFactor = 0f;
        }

        ApplyFlashFactor();
    }

    public void Flash()
    {
        flashFactor = 1f;
        ApplyFlashFactor();
    }

    private void ApplyFlashFactor()
    {
        foreach (var renderer in spriteRenderers)
        {
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_FlashFactor", flashFactor);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }

}
