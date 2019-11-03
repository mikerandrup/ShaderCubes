using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRootMB : MonoBehaviour {

    public GameObject CubePrefab;
    public GameObject Effector;

    [Header("Needs to Correspond with Preview Texture")]
    public Vector2 CubeOffset = new Vector2(1, 1);
    public Texture2D DensityTexture;
    public Vector2 TextureScale = new Vector2(1, 1);
    public int TextureSizeX = 64;
    public int TextureSizeZ = 64;

    private void Start() {
        LoadDensityTexture();
        _effectorMB = Effector.GetComponent<EffectorPointMB>();
        SpawnCubes();
    }

    private List<CubeMB> SpawnedCubes;
    private void SpawnCubes() {
        SpawnedCubes = new List<CubeMB>();

        // for now, spawn the full grid of them
        for (float x = CubeOffset.x / 2; x < TextureSizeX * TextureScale.x; x += CubeOffset.x) {
            for (float z = CubeOffset.y / 2; z < TextureSizeZ * TextureScale.y; z += CubeOffset.y) {
                Spawn(x, z);
            }
        }


    }

    private void Spawn(float x, float z) {
        CubeMB cubeMB = Instantiate(CubePrefab, this.transform).GetComponent<CubeMB>();

        Vector3 pos = new Vector3(x, 0, z);

        cubeMB.transform.position = pos;

        cubeMB.SetCellWeight(
            GetEffectorWeightAt(pos)
        // GetDensityAt(pos)
        );

        SpawnedCubes.Add(cubeMB);
    }

    public float GetDensityAt(Vector3 worldPosition) {

        // 2D for now
        float minX = 0;
        float maxX = TextureScale.x * TextureSizeX;
        float minZ = 0;
        float maxZ = TextureScale.y * TextureSizeZ;

        Bounds textureBounds = new Bounds(
            center: new Vector3(maxX / 2, 0, maxZ / 2),
            size: new Vector3(maxX, 0, maxZ)
        );

        if (!textureBounds.Contains(worldPosition))
            return 0.0f;

        int indexX = Mathf.RoundToInt(worldPosition.x / maxX);
        int indexZ = Mathf.RoundToInt(worldPosition.z / maxZ);

        return _densities01[indexX, indexZ];

    }

    private EffectorPointMB _effectorMB;
    public float GetEffectorWeightAt(Vector3 worldPosition) {

        // 2D? Or not 2D? For now, project onto XZ only
        Vector2 twoDpos = new Vector2(
            worldPosition.x,
            worldPosition.z
        );
        Vector2 twoDeffector = new Vector2(
            Effector.transform.position.x,
            Effector.transform.position.z
        );

        float rawDistance = Mathf.Abs((twoDpos - twoDeffector).magnitude);

        float effect01 = 1.0f - Mathf.Clamp01(rawDistance / _effectorMB.Radius);

        return effect01;

        //float curvedWeight = 1.0f - _effectorMB.Weight.Evaluate(clampedDistance);

        //return curvedWeight;
    }

    // The texture-based method is just for prototype,
    // will use fractal approach instead soon
    private float[,] _densities01;
    private void LoadDensityTexture() {

        Color[] colors = DensityTexture.GetPixels();

        _densities01 = new float[TextureSizeX, TextureSizeZ];
        for (int i = 0, x = 0, z = 0; i < colors.Length; i++) {
            x = i % TextureSizeX;
            z = i / TextureSizeZ;
            _densities01[x, z] = colors[i].grayscale;
        }

    }
}
