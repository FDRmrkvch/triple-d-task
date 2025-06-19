using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GridAnimationController : MonoBehaviour
{
    [Header("Grid Setup")]
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int totalItems = 196;

    [Header("Rotation Target")]
    [SerializeField] private RectTransform rotatingTarget;

    [Header("Random Animation Settings")]
    [SerializeField] private bool randomItemAnimations = true;
    [SerializeField] private float randomAnimInterval = 1f;

    [Header("Rotation Settings")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool clockwise = true;
    [SerializeField] private float rotationPauseDuration = 0.5f;
    [SerializeField] private bool alternateDirection = false;

    private List<GridItem> gridItems = new();
    private bool isRotating = true;
    private float rotationTimer = 0f;
    private float pauseTimer = 0f;
    private int currentDirection = 1;

    private void Start()
    {
        SpawnGridItems();

        if (randomItemAnimations) StartCoroutine(RandomAnimationsLoop());
    }

    private void SpawnGridItems()
    {
        // Очистить все дочерние объекты перед спавном
        for (int i = gridLayout.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gridLayout.transform.GetChild(i).gameObject);
        }
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

        if (isRotating)
        {
            float direction = (clockwise ? -1f : 1f) * currentDirection;
            rotatingTarget.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
            rotationTimer += Time.deltaTime;

            if (rotationTimer >= 2f) // Rotate for 2 seconds
            {
                isRotating = false;
                rotationTimer = 0f;
            }
        }
        else
        {
            pauseTimer += Time.deltaTime;

            if (pauseTimer >= rotationPauseDuration)
            {
                pauseTimer = 0f;
                isRotating = true;

                if (alternateDirection)
                    currentDirection *= -1;
            }
        }
    }

    private IEnumerator RandomAnimationsLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomAnimInterval);

            if (gridItems.Count == 0) continue;

            int index = Random.Range(0, gridItems.Count);
            gridItems[index].PlayRandomAnimation();
        }
    }
}
