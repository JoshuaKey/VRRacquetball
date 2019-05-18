using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    [Header("Values")]
    public Vector3 ballStartPosition;
    public Vector3 playerStartPosition;

    [Header("Components")]
    public Player player;
    public Ball ball;

    [SerializeField] private int score;
    [SerializeField] private int bounces;

    public static Game Instance;

    private void Awake() {
        if(Instance != null) { Destroy(this.gameObject); return; }
        Instance = this;
    }

    private void Start() {
        Reset();
    }

    private void Reset() {
        ball.transform.position = ballStartPosition;
        player.transform.position = playerStartPosition;
        score = 0;
    }

    public void WallBounce() {
        score++;
    }

    public void FloorBounce() {
        bounces++;
        if (bounces >= 2) {
            // Player Loses
            // Reset?
            Reset();
        }
    }

    public void OutOfBounds() {
        // Player Loses
        // Reset?
        Reset();
    }

    private void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 20), "Score: " + score);
        GUI.Label(new Rect(10, 10, 100, 20), "Bounce: " + bounces);
    }
}
