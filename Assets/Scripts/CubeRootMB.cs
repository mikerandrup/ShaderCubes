using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeRootMB : MonoBehaviour {

    public GameObject CubePrefab;
    public GameObject Effector;

    [Header("Needs to Correspond with Preview Texture")]
    public Vector2 CubeOffset = new Vector2(1, 1);
    public Texture2D DensityTexture;
    public Vector2 TextureScale = new Vector2(1, 1);
    public int TextureSizeX = 64;
    public int TextureSizeY = 64;

    private void Start() {
        LoadDensityTexture();
    }

    public float GetDensityAtWorldPosition(Vector3 worldPosition) {

        // ignoring Y of Vector3 for now

        return 0.5f;

    }

    // The texture-based method is just for prototype,
    // will use fractal approach instead soon
    private float[,] _densities01;
    private void LoadDensityTexture() {

        Color[] colors = DensityTexture.GetPixels();

        _densities01 = new float[TextureSizeX, TextureSizeY];
        for (int i = 0, x = 0, y = 0; i < colors.Length; i++) {
            x = i % TextureSizeX;
            y = i / TextureSizeY;
            _densities01[x, y] = colors[i].grayscale;
        }

    }
}
