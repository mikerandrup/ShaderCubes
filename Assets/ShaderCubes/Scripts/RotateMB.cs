using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMB : MonoBehaviour {
    public Vector3 RotateSpeed;

    // Update is called once per frame
    void Update() {
        this.transform.Rotate(RotateSpeed * Time.deltaTime);
    }
}
