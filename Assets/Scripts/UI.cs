using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public string Username;
    public string Password;
    public bool ToRegister;

    private void Start() {
        UnityInitializer.AttachToGameObject(this.gameObject);
        Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;

        GameData.credentials = new CognitoAWSCredentials("us-east-2:7127d020-055c-4436-88cc-5d62a2156f81", RegionEndpoint.USEast2);
        GameData.lambda = new AmazonLambdaClient(GameData.credentials, RegionEndpoint.USEast2);

        if (ToRegister) {
            Register(Username, Password, (x) => {
                GameData.user = x;
                print("Registered");
                //print(x.ID);
                //print(x.UserName);
                //print(x.PasswordHash);
                //print(x.Score);
                //SceneManager.LoadScene("GearVRControllerTest");
            });
        } else {
            Login(Username, Password, (x) => {
                GameData.user = x;
                print("Log in");
                //print(x.ID);
                //print(x.UserName);
                //print(x.PasswordHash);
                //print(x.Score);
                //SceneManager.LoadScene("GearVRControllerTest");
            });
        }
        
    }

    public void Login(string username, string password, Action<Game.User> callback) {
        var request = new InvokeRequest() {
            FunctionName = "existing-systems-dynamodb-lambda-dev-signInUser",
            Payload = "{\"username\": \"" + username + "\", \"password\": \"" + password + "\"}",
            InvocationType = InvocationType.RequestResponse
        };
        GameData.lambda.InvokeAsync(request, (result) => {
            if (result.Exception == null) {
                string json = Encoding.ASCII.GetString(result.Response.Payload.ToArray());

                Debug.Log(json);

                Game.User user = JsonUtility.FromJson<Game.User>(json);

                callback(user);
            } else {
                Debug.LogError(result.Exception);
            }
        });
    }

    public void Register(string username, string password, Action<Game.User> callback) {
        var request = new InvokeRequest() {
            FunctionName = "existing-systems-dynamodb-lambda-dev-createUser",
            Payload = "{\"username\": \"" + username + "\", \"password\": \"" + password + "\"}",
            InvocationType = InvocationType.RequestResponse
        };
        GameData.lambda.InvokeAsync(request, (result) => {
            if (result.Exception == null) {
                string json = Encoding.ASCII.GetString(result.Response.Payload.ToArray());

                Debug.Log(json);

                Game.User user = JsonUtility.FromJson<Game.User>(json);

                callback(user);
            } else {
                Debug.LogError(result.Exception);
            }
        });
    }
}

public static class GameData {
    public static CognitoAWSCredentials credentials;
    public static AmazonLambdaClient lambda;
    public static Game.User user;
}
