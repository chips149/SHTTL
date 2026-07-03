using UnityEngine;

public class BattleMapBuilder : MonoBehaviour
{
    public GameObject userAreaPrefab;
    public GameObject nailPrefab;
    public int width, height;
    public float xGap = 1, yGap = 1;

    public bool autoFitWidth = true;
    public bool autoFitHeight = true;
    public int maxHeight = 12;
    public bool startWithSingle;

    private void Awake()
    {
        CalcCameraFit();
        BuildGrid();
    }

    private void Start()
    {
        if (Camera.main != null && (autoFitWidth || autoFitHeight))
        {
            CalcCameraFit();
            BuildGrid();
        }
    }

    private void CalcCameraFit()
    {
        if (Camera.main == null)
        {
            Debug.LogWarning("BattleMapBuilder: Camera.main is null");
            return;
        }

        if (autoFitWidth)
        {
            float cameraWorldWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
            width = Mathf.FloorToInt(cameraWorldWidth / xGap);
        }
        if (autoFitHeight)
        {
            float cameraWorldHeight = Camera.main.orthographicSize * 2f;
            height = Mathf.Min(Mathf.FloorToInt(cameraWorldHeight / yGap), maxHeight);
        }
    }

    public void OnValidate()
    {
        CalcCameraFit();
        BuildGrid();
    }

    private void EnsureChildCount(Transform parent, int needed, GameObject prefab)
    {
        if (prefab == null) return;
        for (int i = parent.childCount; i < needed; i++)
        {
            GameObject go = Instantiate(prefab, parent);
            go.name = $"{prefab.name}_{i}";
        }
    }

    private int CountNeededNails()
    {
        int count = 0;
        for (int y = 0; y < height; y++)
        {
            bool isSingle = y % 2 == 1 ^ startWithSingle;
            count += isSingle ? width - 1 : width;
        }
        return count;
    }

    private int CountNeededUserAreas()
    {
        int count = 0;
        for (int y = 0; y < height; y++)
        {
            bool isSingle = y % 2 == 0 ^ startWithSingle;
            count += isSingle ? width - 1 : width;
        }
        return count;
    }

    private void BuildGrid()
    {
        var center = new Vector3(width * xGap / 2, height * yGap / 2, 0)
                     - new Vector3(xGap * 0.5f, yGap * 0.5f, 0);

        var nailParent = transform.Find("NailParent");
        if (nailParent != null && nailPrefab != null)
        {
            EnsureChildCount(nailParent, CountNeededNails(), nailPrefab);

            var nailIndex = 0;
            for (int y = 0; y < height; y++)
            {
                var isSingle = y % 2 == 1 ^ startWithSingle;
                int cols = isSingle ? width - 1 : width;

                for (int x = 0; x < cols; x++)
                {
                    nailParent.GetChild(nailIndex).localPosition =
                        new Vector3(x * xGap, y * yGap, 0)
                        + Vector3.right * (isSingle ? xGap * 0.5f : 0)
                        - center;
                    nailIndex++;
                }
            }
        }

        var userAreaParent = transform.Find("UserAreaParent");
        if (userAreaParent != null && userAreaPrefab != null)
        {
            EnsureChildCount(userAreaParent, CountNeededUserAreas(), userAreaPrefab);

            var userAreaIndex = 0;
            for (int y = 0; y < height; y++)
            {
                var isSingle = y % 2 == 0 ^ startWithSingle;
                int cols = isSingle ? width - 1 : width;

                for (int x = 0; x < cols; x++)
                {
                    userAreaParent.GetChild(userAreaIndex).localPosition =
                        new Vector3(x * xGap, y * yGap, 0)
                        + Vector3.right * (isSingle ? xGap * 0.5f : 0)
                        - center;
                    userAreaIndex++;
                }
            }
        }
    }
}
