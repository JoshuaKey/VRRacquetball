using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.SecurityToken.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour {

    public class User {
        public string ID;
        public string UserName;
        public string PasswordHash;
        public int Score;
    }

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

    private string latestDebug;

    private void Awake() {
        if(Instance != null) { Destroy(this.gameObject); return; }
        Instance = this;
    }

    private void Start() {
        ballStartPosition = ball.transform.position;
        playerStartPosition = player.transform.position;
        latestDebug = $"Player position set to {player.transform.position}";
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
        if(latestDebug != string.Empty && latestDebug != null) text += "\nDebug: " + latestDebug;
        Text.text = text;
    }

    private void Reset() {
        if (score >= GameData.user.Score) {
            UpdateScore(score);
        }

        ball.transform.position = ballStartPosition;
        player.transform.position = playerStartPosition;
        ball.Reset();
        score = 0;
        bounces = 0;
    }

    public void UpdateScore(int score) {
        var request = new InvokeRequest() {
            FunctionName = "existing-systems-dynamodb-lambda-dev-updateScore",
            Payload = "{\"id\": \"" + GameData.user.ID + "\", \"score\": \"" + score + "\"}",
            InvocationType = InvocationType.RequestResponse
        };
        GameData.lambda.InvokeAsync(request, (result) => {
            if (result.Exception == null) {
                string json = Encoding.ASCII.GetString(result.Response.Payload.ToArray());

                Debug.Log(json);

                Game.User user = JsonUtility.FromJson<Game.User>(json);

                //callback(user);
            } else {
                Debug.LogError(result.Exception);
            }
        });
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
