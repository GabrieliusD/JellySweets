using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResizeGroupLayout : MonoBehaviour
{
    public GameObject container;
    RectTransform rect;
    GridLayoutGroup layout;
    // Start is called before the first frame update
    void Start()
    {
        rect = container.GetComponent<RectTransform>();
        layout = container.GetComponent<GridLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        float width = rect.rect.width;
        Vector2 newSize = new Vector2(width / 2, rect.rect.height);
        layout.cellSize = newSize;
    }
}
