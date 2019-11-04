using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMB : MonoBehaviour {

    public Material[] MaterialsSortedByWeight;

    private MeshRenderer _meshRenderer;

    public void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
        SetCellWeight(0.0f);
    }

    public float MaxHeight = 10.0f;
    public void SetCellWeight(float weight01) {

        //just shut off if not affected
        if (weight01 <= 0) {
            this.gameObject.SetActive(false);
            return;
        }

        this.gameObject.SetActive(true);

        // scale the height accordingly
        transform.localScale = new Vector3(
             1.0f,
             weight01 * MaxHeight,
             1.0f
        );

        // assign the material for the weight
        UpdateMaterial(weight01);

    }

    private int _currentMatIndex = -1;
    private void UpdateMaterial(float weight) {

        int maxMatIndex = MaterialsSortedByWeight.Length - 1;

        int matIndex = Mathf.RoundToInt(weight * maxMatIndex);

        if (matIndex == _currentMatIndex) {
            return;
        }
        else {
            _currentMatIndex = matIndex;
            _meshRenderer.materials = new Material[] { MaterialsSortedByWeight[matIndex] };
        }
    }


}
