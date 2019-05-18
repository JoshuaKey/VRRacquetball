using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [Header("Components")]
    public new Rigidbody rigidbody;
    public new Collider collider;

    private void OnCollisionEnter() {
        rigidbody.constraints = RigidbodyConstraints.None;
    }

}
