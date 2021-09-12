using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites = new List<Sprite>();
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (sprites.Count == 0)
            throw new UnassignedReferenceException("Box Sprites have to be set");
    }

    void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
