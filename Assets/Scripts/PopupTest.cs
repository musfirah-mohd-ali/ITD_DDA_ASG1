using UnityEngine;
using System.Collections;

public class PopupTest : MonoBehaviour
{
    public Animator animator;
    [SerializeField]
    public GameObject popup;
    private Coroutine popupCoroutine;

    public AudioManager audioManager;
    public AudioClip errorSound;

    


    public void PopupTrigger()
    {
        // Set popup to active in scene if not already
        if (popup.activeSelf == false)
            popup.SetActive(true);

        animator.SetBool("ErrorActive", true);
        audioManager.PlaySound(errorSound);
        Debug.Log("Popup Triggered");

        // Stop any existing coroutine
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }

        popupCoroutine = StartCoroutine(HidePopupAfterDelay(5f));
    }

    public void HidePopup()
    {
        animator.SetBool("ErrorActive", false);
        Debug.Log("Popup Hidden");

        // Stop any existing coroutine
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hide the popup
        animator.SetBool("ErrorActive", false);
        Debug.Log("Popup Hidden after delay");
    }
}
