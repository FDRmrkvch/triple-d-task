using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls a grid of animated items and optional rotating UI element.
/// Supports spawning grid items, applying random animations, and rotating a target.
/// </summary>
public class GridAnimationController : MonoBehaviour
{
    [Header("Grid Setup")]
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int totalItems = 196;

    [Header("Rotation Target")]
    [SerializeField] private RectTransform rotatingTarget;

    [Header("Animation Settings")]
    [SerializeField] private bool randomItemAnimations = true;

    [Header("Rotation Settings")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool clockwise = true;

    private List<GridItem> gridItems = new();

    private void Start()
    {
        SpawnGridItems();

        if (randomItemAnimations)
            StartCoroutine(RandomAnimationsLoop());
    }

    /// <summary>
    /// Clears existing grid children and spawns a new grid of item prefabs.
    /// </summary>
    private void SpawnGridItems()
    {
        for (int i = gridLayout.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gridLayout.transform.GetChild(i).gameObject);
        }

        gridItems.Clear();

        for (int i = 0; i < totalItems; i++)
        {
            GameObject obj = Instantiate(itemPrefab, gridLayout.transform);
            if (obj.TryGetComponent(out GridItem item))
            {
                gridItems.Add(item);
            }
        }
    }

    private void Update()
    {
        if (!rotate || rotatingTarget == null) return;

        float direction = clockwise ? -1f : 1f;
        rotatingTarget.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Loops through grid items and plays random animations periodically.
    /// </summary>
    private IEnumerator RandomAnimationsLoop()
    {
        while (true)
        {
            yield return null; // Frame delay to avoid tight loop

            if (gridItems.Count == 0) continue;

            int index = Random.Range(0, gridItems.Count);
            gridItems[index].PlayRandomAnimation();
            yield return new WaitForSeconds(0f); // Constant delay for randomness (fixed to 0s)
        }
    }
}
