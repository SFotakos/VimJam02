using System.Collections.Generic;
using UnityEngine;

public class BoxUnloadingArea : MonoBehaviour
{
    public List<BoxPosition> boxesPosition = new List<BoxPosition>();
    Vector2 unloadAreaSize, unloadAreaCenter;
    Vector2 pointSpacing = new Vector2(0.35f, 0.2f);

    private void Awake()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        unloadAreaCenter = boxCollider2D.bounds.center;
        unloadAreaSize = boxCollider2D.bounds.size;
        
        int rowsAmount = Mathf.FloorToInt(unloadAreaSize.y / pointSpacing.y);
        int columnsAmount = Mathf.FloorToInt(unloadAreaSize.x / pointSpacing.x);
        Vector2 bottomLeftPosition = (unloadAreaCenter - unloadAreaSize / 2);

        for (int i = 0; i < rowsAmount; i++)
        {
            for (int j = 0; j < columnsAmount; j++)
            {
                boxesPosition.Add(new BoxPosition(bottomLeftPosition + new Vector2(j * pointSpacing.x, i * pointSpacing.y)));
            }
        }
    }

    public void AddBox(GameObject box)
    {
        bool placedBox = false;
        foreach (BoxPosition position in boxesPosition)
        {
            if (!position.hasBox)
            {
                placedBox = true;
                box.transform.position = position.boxPosition;
                box.transform.parent = gameObject.transform;
                position.hasBox = true;
                break;
            }
        }
        box.SetActive(placedBox);
    }

    public class BoxPosition
    {
        public bool hasBox;
        public Vector2 boxPosition;

        public BoxPosition(Vector2 boxTransform, bool hasBox = false)
        {
            this.boxPosition = boxTransform;
            this.hasBox = hasBox;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(unloadAreaCenter, unloadAreaSize);
        foreach (BoxPosition position in boxesPosition)
        {
            Gizmos.DrawWireCube(position.boxPosition, pointSpacing);
        }
    }
}
