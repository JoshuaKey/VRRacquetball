//using Amazon;
//using Amazon.CognitoIdentity;
//using Amazon.Lambda;
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

    private CognitoAWSCredentials credentials;
    private AmazonLambdaClient lambda;
    private User user;

    private void Awake() {
        if(Instance != null) { Destroy(this.gameObject); return; }
        Instance = this;

        UnityInitializer.AttachToGameObject(this.gameObject);
        Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;


        credentials = new CognitoAWSCredentials("us-east-2:7127d020-055c-4436-88cc-5d62a2156f81", RegionEndpoint.USEast2);
        lambda = new AmazonLambdaClient(credentials, RegionEndpoint.USEast2);
        //lambda.inv
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

    //public User Login(string username, string password) {
    //    var request = new InvokeRequest() {
    //        FunctionName = "existing-systems-dynamodb-lambda-dev-signInUser",
    //        Payload = "{\"username\": \"" + username + "\", \"password\": \"" + password + "\"}",
    //        InvocationType = InvocationType.RequestResponse
    //    };
    //    lambda.InvokeAsync(request, (result) => {
    //        if (result.Exception == null) {
    //            string json = Encoding.ASCII.GetString(result.Response.Payload.ToArray());
    //            Debug.Log(json);
    //            user = JsonUtility.FromJson<User>(json);
    //        } else {
    //            Debug.LogError(result.Exception);
    //        }
    //    });
    //}

    //public async TaskCompletionSource<User> Register(string username, string password) {
    //    var promise = new TaskCompletionSource<User>();
        

    //    var request = new InvokeRequest() {
    //        FunctionName = "existing-systems-dynamodb-lambda-dev-createUser",
    //        Payload = "{\"username\": \"" + username + "\", \"password\": \"" + password + "\"}",
    //        InvocationType = InvocationType.RequestResponse
    //    };
    //    lambda.InvokeAsync(request, (result) => {
    //        if (result.Exception == null) {
    //            string json = Encoding.ASCII.GetString(result.Response.Payload.ToArray());
    //            Debug.Log(json);
    //            user = JsonUtility.FromJson<User>(json);
    //            promise.TrySetResult(user);
    //        } else {
    //            Debug.LogError(result.Exception);
    //        }
    //    });



    //    //FuncToCall func = new FuncToCall(Console.WriteLine);
    //    //func.BeginInvoke(s, new AsyncCallback(WriteLineCallback), func);

    //    //new AsyncCallback(RegisterAsync);

    //    //Amazon.Lambda
    //}

    //delegate void FuncToCall(string s);

    //private void RegisterAsync(IAsyncResult res) {
    //    res
    //}

    public void UpdateScore(string id, int score) {

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
