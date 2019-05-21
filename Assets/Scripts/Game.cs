using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour {

    [Header("Values")]
    public Vector3 ballStartPosition;
    public Vector3 playerStartPosition;

    [Header("Components")]
    public Player player;
    public Ball ball;

    [Header("Debug")]
    public GameObject trackingSpace;
    public GameObject centerEye;
    public GameObject trackingAnchor;
    public Canvas debugCanvas;
    public TextMeshProUGUI Text;

    [SerializeField] private int score;
    [SerializeField] private int bounces;

    public static Game Instance;

    private void Awake() {
        if(Instance != null) { Destroy(this.gameObject); return; }
        Instance = this;
    }

    private void Start() {
        ballStartPosition = ball.transform.position;
        playerStartPosition = player.transform.position;
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.Button.One)) {
            ball.Serve();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two)) {
            debugCanvas.gameObject.SetActive(debugCanvas.gameObject.activeInHierarchy);
        }

        if (Application.isEditor) {
            if (Input.GetKeyDown(KeyCode.T)) {
                debugCanvas.gameObject.SetActive(debugCanvas.gameObject.activeInHierarchy);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                ball.Serve();
            }
        }
    }

    void LateUpdate() {
        string text = "";
        text += "Bounces: " + bounces;
        text += "\nScore: " + score;
        text += "\nPlayer: " + player.transform.position + " " + player.transform.rotation;
        text += "\nBall: " + ball.transform.position + " " + ball.transform.rotation;
        text += "\nTrack: " + trackingSpace.transform.position + " " + trackingSpace.transform.rotation;
        text += "\nEye: " + centerEye.transform.position + " " + centerEye.transform.rotation;
        text += "\nAnchor: " + trackingAnchor.transform.position + " " + trackingAnchor.transform.rotation;
        Text.text = text;
    }

    private void Reset() {
        ball.transform.position = ballStartPosition;
        player.transform.position = playerStartPosition;
        ball.Reset();
        score = 0;
        bounces = 0;
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
        GUI.Label(new Rect(10, 30, 100, 20), "Bounce: " + bounces);
    }

    public static void Build() {

    }
}
