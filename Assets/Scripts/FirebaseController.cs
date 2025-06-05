using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Firebase.Extensions;

public class FirebaseController : MonoBehaviour
{
    [SerializeField] private GameObject login , signup , forgetpassword;
    [SerializeField] private TMP_InputField loginEmail , loginPassword , signupEmail , signupUsername, signupPassword , signupConfirmPassword , forgetpasswordEmail;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        var dependencyStatus = task.Result;

        if (dependencyStatus == Firebase.DependencyStatus.Available) {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
            // app = Firebase.FirebaseApp.DefaultInstance;
            Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
            app.Options.DatabaseUrl = new System.Uri("https://king-god-53d03-default-rtdb.firebaseio.com/");


            InitializeFirebase();


            // Set a flag here to indicate whether Firebase is ready to use by your app.
        } else {
            UnityEngine.Debug.LogError(System.String.Format(
            "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
        }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
            Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
            Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    // Handle removing subscription and reference to the Auth instance.
    // Automatically called by a Monobehaviour after Destroy is called on it.
    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void OpenLoginPanel()
    {
        login.SetActive(true);
        signup.SetActive(false);
        forgetpassword.SetActive(false);
    }
        public void OpenSignupPanel()
    {
        login.SetActive(false);
        signup.SetActive(true);
        forgetpassword.SetActive(false);
    }
        public void OpenForgetPasswordPanel()
    {
        login.SetActive(false);
        signup.SetActive(false);
        forgetpassword.SetActive(true);
    }

    public void LoginUser()
    {
        if(string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            return;
        }

        SigninUser(loginEmail.text, loginPassword.text);
        

    }
    public void SignupUser()
    {
        if(string.IsNullOrEmpty(signupUsername.text) || string.IsNullOrEmpty(signupEmail.text) ||string.IsNullOrEmpty(signupPassword.text)  || string.IsNullOrEmpty(signupConfirmPassword.text)  )
        {

        }
        if(signupPassword.text == signupConfirmPassword.text)
        {
            CreateNewUser(signupEmail.text , signupPassword.text);
        }
    }
    public void ForgetPasswordUser()
    {
        if (string.IsNullOrEmpty(forgetpasswordEmail.text))
        {

        }
    }
    public void CreateNewUser(string username, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(username, password).ContinueWithOnMainThread(task => {
        if (task.IsCanceled) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            return;
        }

        // Firebase user has been created.
        Firebase.Auth.AuthResult result = task.Result;
        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);
        });
    }

    public void SigninUser(string username, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(username, password).ContinueWithOnMainThread(task => {
        if (task.IsCanceled) {
            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            return;
        }

        Firebase.Auth.AuthResult result = task.Result;
        
        Debug.LogFormat("User signed in successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);
        });

    }
    public void UpdateUserProfile()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null) {
        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
            DisplayName = "Jane Q. User",
            PhotoUrl = new System.Uri("https://example.com/jane-q-user/profile.jpg"),
        };
        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
            Debug.LogError("UpdateUserProfileAsync was canceled.");
            return;
            }
            if (task.IsFaulted) {
            Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
            return;
            }

            Debug.Log("User profile updated successfully.");
        });
        }
    }



}
