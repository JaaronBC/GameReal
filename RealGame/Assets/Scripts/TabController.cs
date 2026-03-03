using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image[] tabImages;
    public GameObject[] pages;
    void Start()
    {
        ActivateTab(0);
    }


    public void ActivateTab(int tabNo)
    {
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }
        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;
    }
}
