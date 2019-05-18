using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Ball ball;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = this.transform.position;
        pos.x = Mathf.Lerp(this.transform.position.x, ball.transform.position.x, Time.deltaTime);
        this.transform.position = pos;
    }
}
