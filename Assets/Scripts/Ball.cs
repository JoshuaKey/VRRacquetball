using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float ServeForce = 3.0f;

    [Header("Components")]
    public new Rigidbody rigidbody;
    public new Collider collider;

    private Transform parent;
    private bool hasServed = false;

    private void Start() {
        parent = this.transform.parent;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Reset() {
        this.transform.parent = parent;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        hasServed = false;
    }

    public void Serve() {
        if (!hasServed) {
            hasServed = true;
            rigidbody.constraints = RigidbodyConstraints.None;
            this.transform.parent = null;

            rigidbody.AddForce(ServeForce * Vector3.up, ForceMode.Impulse);
        }
    }

}
