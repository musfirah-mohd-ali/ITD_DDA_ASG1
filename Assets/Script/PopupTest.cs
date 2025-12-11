using UnityEngine;
using System.Collections;

public class PopupTest : MonoBehaviour
{
    public Animator animator;
    private Coroutine popupCoroutine;


    public void PopupTrigger()
    {
        Debug.Log("Popup Triggered");
        animator.SetTrigger("ErrorActive");
        
        // Stop any existing coroutine to avoid overlap
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }

        popupCoroutine = StartCoroutine(PopupDuration(5f));
    }

    public void HideTrigger()
    {
        animator.SetTrigger("ErrorActive");

        // Stop any existing coroutine to avoid overlap
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }
    }

    private IEnumerator PopupDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Hide the popup after the duration
        animator.SetTrigger("ErrorActive");
        popupCoroutine = null;
    }
}
