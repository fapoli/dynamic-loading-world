using System.Collections.Generic;
using UnityEngine;

public class DynamicLoadingWorld : MonoBehaviour
{
    public List<WorldLocation> locations;
    public List<WeightedItem<GameObject>> genericPrefabs;

    [Header("Generator Settings")]
    public bool fillEmptyLocations;
    public bool rotateFilledLocations;
    public int seed;

    [Header("Grid Settings")]
    public int tileCountX = 10;
    public int tileCountY = 10;
    public int cellSize = 100;
    public bool isInfinite;

    private WorldLocation[,] matrix;
    private System.Random random;
    private List<WorldLocation> loadedLocations;
    private Transform player;
    private RectInt bounds;
    private WeightedList<GameObject> genericWeightedPrefabs;

    private void Start()
    {
        genericWeightedPrefabs = new WeightedList<GameObject>(seed, genericPrefabs);
        loadedLocations = new List<WorldLocation>();
        random = new System.Random(seed);
        bounds = new RectInt(0, 0, tileCountX, tileCountY);
        matrix = new WorldLocation[bounds.width + 1, bounds.height + 1];
        player = GameObject.FindWithTag("Player")
                           .GetComponent<Transform>();

        if (fillEmptyLocations)
        {
            for (var i = 0; i < bounds.width * bounds.height; i++)
            {
                var x = i % bounds.width;
                var y = i / bounds.width;

                matrix[i % bounds.width, i / bounds.width] = new WorldLocation(
                    genericWeightedPrefabs.GetRandom(),
                     new Vector2Int(i % bounds.width, i / bounds.width),
                     rotateFilledLocations ? random.Next(4) * 90 : 0);
            }
        }

        foreach (var location in locations)
            matrix[location.position.x, location.position.y] = location;
    }

    private void Update()
    {
        var playerPos = new Vector2Int(
            Mathf.RoundToInt(player.position.x / cellSize),
            Mathf.RoundToInt(player.position.z / cellSize));

        for (var y = playerPos.y - 1; y <= playerPos.y + 1; y++)
        {
            if (!isInfinite && (y < bounds.y || y > bounds.height))
                continue;

            for (var x = playerPos.x - 1; x <= playerPos.x + 1; x++)
            {
                if (!isInfinite && (x < bounds.x || x > bounds.width))
                    continue;

                var realX = x % bounds.width;
                var realY = y % bounds.height;

                if (!ContainsPosition(loadedLocations, x, y))
                {
                    var position = new Vector3(x * cellSize, transform.position.y, y * cellSize);
                    var prefab = matrix[realX, realY].prefab;
                    var map = Instantiate(prefab, position, prefab.transform.rotation, transform);
                    map.transform.Rotate(new Vector3(0, matrix[realX, realY].RotationY, 0));
                    loadedLocations.Add(new WorldLocation(map, new Vector2Int(x, y)));
                }
            }

        }

        for (var i = loadedLocations.Count - 1; i >= 0; i--)
        {
            var loaded = loadedLocations[i];
            if (Mathf.Abs(loaded.position.x - Mathf.Round(playerPos.x)) > 1 || Mathf.Abs(loaded.position.y - Mathf.Round(playerPos.y)) > 1)
            {
                loadedLocations.Remove(loaded);
                Destroy(loaded.prefab);
            }
        }
    }

    private bool ContainsPosition(List<WorldLocation> list, int x, int y)
    {
        foreach (var it in list)
            if (it.position.x == x && it.position.y == y)
                return true;

        return false;
    }
}

[System.Serializable]
public class WorldLocation
{
    public GameObject prefab;
    public Vector2Int position;
    private int rotationY;

    public int RotationY { get { return rotationY; } }

    public WorldLocation(GameObject prefab, Vector2Int position, int rotationY = 0)
    {
        this.prefab = prefab;
        this.position = position;
        this.rotationY = rotationY;
    }
}

