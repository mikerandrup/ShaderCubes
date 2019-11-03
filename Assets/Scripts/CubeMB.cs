using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMB : MonoBehaviour {

    public Material[] MaterialsSortedByWeight;

    private MeshRenderer _meshRenderer;

    public void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }


    public float MaxHeight = 10.0f;
    public void SetCellWeight(float weight01) {

        //just shut off if not affected
        if (weight01 <= 0) {
            this.gameObject.SetActive(false);
            return;
        }


        // scale the height accordingly
        transform.localScale = new Vector3(
             1.0f,
             weight01 * MaxHeight,
             1.0f
        );

        // assign the material for the weight
        _meshRenderer.materials = new Material[] { GetMaterialForWeight(weight01) };

    }

    private Material GetMaterialForWeight(float weight) {

        int maxMatIndex = MaterialsSortedByWeight.Length - 1;

        int matIndex = Mathf.RoundToInt(weight * maxMatIndex);

        Debug.Log("Mat index " + matIndex + " for weight " + weight + " is called " + MaterialsSortedByWeight[matIndex].name);

        return MaterialsSortedByWeight[
            matIndex
        ];

    }


}
