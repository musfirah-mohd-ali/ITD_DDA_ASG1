using UnityEngine;
using UnityEngine.UI;
using FIrebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class PanelChanger : MonoBehaviour
{
    [SerializeField]
    public GameObject[] ActivePanels;

    public TMP_InputField regisUserInput;
    public TMP_InputField regisEmailInput;
    public TMP_InputField regisPassInput;

    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPassInput;


    public TMP_Text errorText;




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
            Debug.Log("Registration failed: Incomplete fields.");
            return;
        }
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
