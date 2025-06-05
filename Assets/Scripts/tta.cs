using UnityEngine;

public class TerrainToHeightArray : MonoBehaviour
{
    public Terrain terrain; // Gắn Terrain vào Inspector.

    void Start()
    {
        // Lấy dữ liệu từ Terrain.
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        // Độ phân giải của heightmap.
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        // Lấy dữ liệu chiều cao từ heightmap (giá trị trong khoảng 0.0 - 1.0).
        float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        // Tạo mảng lưu độ cao thực tế tại mỗi ô 1x1.
        float[,] heightArray = new float[100, 100];

        // Tính tỷ lệ giữa hệ heightmap và hệ world space (100x100).
        float scaleX = terrainSize.x / 100f;
        float scaleZ = terrainSize.z / 100f;

        for (int z = 0; z < 100; z++)
        {
            for (int x = 0; x < 100; x++)
            {
                // Chuyển tọa độ từ world space (100x100) sang heightmap.
                float normalizedX = x * scaleX / terrainSize.x;
                float normalizedZ = z * scaleZ / terrainSize.z;

                int heightX = Mathf.FloorToInt(normalizedX * (heightmapWidth - 1));
                int heightZ = Mathf.FloorToInt(normalizedZ * (heightmapHeight - 1));

                // Lấy chiều cao thực tế tại điểm (x, z).
                heightArray[z, x] = heights[heightZ, heightX] * terrainSize.y;
            }
        }

        // // Debug để kiểm tra kết quả.
        // Debug.Log($"Chiều cao tại (0, 0): {heightArray[0, 0]}");
        // Debug.Log($"Chiều cao tại (50, 50): {heightArray[50, 50]}");
        // Debug.Log($"Chiều cao tại (99, 99): {heightArray[99, 99]}");
    }
}