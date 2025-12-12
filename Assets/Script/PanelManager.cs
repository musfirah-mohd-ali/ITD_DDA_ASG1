using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using TMPro;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] ActivePanels;
    public GameObject HomePanel;
    public GameObject DockPanel;
    public PopupTest popup;


    // Registration and Login Input Fields
    public TMP_InputField regisUserInput;
    public TMP_InputField regisEmailInput;
    public TMP_InputField regisPassInput;
    public TMP_InputField regisPassConfirmInput;

    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPassInput;


    public TMP_Text errorText;




    // Password validation variables




    public void SwitchPanel(GameObject panelToActivate)
    {
        Transform parent = panelToActivate.transform.parent;
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
        panelToActivate.SetActive(true);
    }

    public void Register()
    {
        if (regisEmailInput.text == "" || regisPassInput.text == "" || regisUserInput.text == "")
        {
            errorText.text = "Please fill in all fields.";
            popup.PopupTrigger();
            Debug.Log("Registration failed: Incomplete fields.");
            return;
        }


        // Username validation
        if (regisUserInput.text.Length < 8)
        {
            errorText.text = "Username must be at least 8 characters.";
            popup.PopupTrigger();
            Debug.Log("Registration failed: Username too short.");
            return;
        }
        // letters, numbers, underscore, period only
        if (!Regex.IsMatch(regisUserInput.text, @"^[a-zA-Z][a-zA-Z0-9._]{7,23}$"))
        {
            errorText.text = "Username must start with a letter and use only letters, numbers, underscores or periods.";
            return;
        }



        bool hasUpper = Regex.IsMatch(regisPassInput.text, "[A-Z]");
        bool hasLower = Regex.IsMatch(regisPassInput.text, "[a-z]");
        bool hasNumber = Regex.IsMatch(regisPassInput.text, "[0-9]");
        bool hasSymbol = Regex.IsMatch(regisPassInput.text, "[^a-zA-Z0-9]");
        bool hasNoSpaces = !regisPassInput.text.Contains(" ");

        // Password validation
        if (regisPassInput.text.Length < 8)
        {
            errorText.text = "Password must be at least 8 characters.";
            popup.PopupTrigger();
            Debug.Log("Registration failed: Password too short.");
            return;
        }
        // Check for character types
        if (!hasUpper || !hasLower || !hasNumber || !hasSymbol || !hasNoSpaces)
        {
            errorText.text = "Password must contain upper, lower, number, symbol, and no spaces.";
            popup.PopupTrigger();
            Debug.Log("Registration failed: Password complexity requirements not met.");
            return;
        }

        if (regisPassInput.text != regisPassConfirmInput.text)
        {
            errorText.text = "Passwords do not match.";
            popup.PopupTrigger();
            Debug.Log("Registration failed: Passwords do not match.");
            return;
        }




        var createTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(regisEmailInput.text, regisPassInput.text);

        createTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                
                errorText.text = "Error signing up. Please try again.";
                popup.PopupTrigger();
                Debug.LogError("Error signing user up!");
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("User signed up successfully!");
                regisUserInput.text = "";
                regisEmailInput.text = "";
                regisPassInput.text = "";
                regisPassConfirmInput.text = "";


                var db = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Database reference obtained.");

                // Create user profile
                var authResult = task.Result;
                Debug.Log("Firebase User ID: " + authResult.User.UserId);

                FirebaseUser newUser = authResult.User;

                string uid = newUser.UserId;
                Debug.Log("Firebase User ID: " + newUser.UserId);                

                UserProfile userProfile = new UserProfile(regisUserInput.text, regisEmailInput.text);


                string json = JsonUtility.ToJson(userProfile);
                Debug.Log("JSON being written: " + json);


                db.Child("users").Child(uid).SetRawJsonValueAsync(json).ContinueWithOnMainThread(dbTask =>
                {
                    if (dbTask.IsFaulted || dbTask.IsCanceled)
                    {
                        errorText.text = "Error saving user profile. Please try again.";
                        popup.PopupTrigger();
                        Debug.LogError("Error saving user profile!");
                        return;
                    }
                    if (dbTask.IsCompleted)
                    {
                        Debug.Log("User profile saved successfully!");
                        SwitchPanel(HomePanel); // Switch to Home Panel
                    }
                });



            }
        });
    }

    public void SignIn()
    {
        if (loginEmailInput.text == "" || loginPassInput.text == "")
        {
            errorText.text = "Please fill in all fields.";
            popup.PopupTrigger();
            Debug.Log("Login failed: Incomplete fields.");
            return;
        }

        // Sign in with email and password
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(loginEmailInput.text, loginPassInput.text);

        signInTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                errorText.text = "Error signing in. Incorrect email or password.";
                popup.PopupTrigger();
                Debug.LogError("Error signing user in!");
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("User signed in successfully!");

                FirebaseUser user = task.Result.User;
                Debug.Log("Logged in User ID: " + user.UserId);

                SwitchPanel(HomePanel); // Switch to Home Panel
            }
        });
    }

    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("User logged out.");


        loginEmailInput.text = "";
        loginPassInput.text = "";

        // Go back to login panel
        SwitchPanel(DockPanel);
    }

    void Start()
    {
        // Deactivate all panels first
        Transform parent = ActivePanels[0].transform.parent;
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
        // Activate specified panels
        foreach (GameObject panel in ActivePanels)
        {
            panel.SetActive(true);
        }   
    }
}
