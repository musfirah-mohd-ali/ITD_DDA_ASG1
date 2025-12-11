using UnityEngine;

public class PanelChanger : MonoBehaviour
{
    [SerializeField]
    GameObject DockPanel;

    public void SwitchPanel(GameObject panelToActivate)
    {
        Transform parent = panelToActivate.transform.parent;
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
        panelToActivate.SetActive(true);
    }

    void Start()
    {
        Transform parent = DockPanel.transform.parent;
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
        DockPanel.SetActive(true);
    }
}
