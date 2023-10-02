/*
 * Jacob Zydorowicz
 * ShopNavigation.cs
 * City Sim Project
 * Activates the various shop menu tabs based on the button that is clicked
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopNavigation : MonoBehaviour
{
    string menuTag;
    string currentMenu;
    GameObject newMenuObj;
    GameObject currentMenuObj;
  

    void Start()
    {
        menuTag = "Inventory";
        currentMenu = menuTag;
    }

    /// <summary>
    /// Method <c>openNewMenu</c> activates the new menu tab based on which button is clicked
    /// <paramref name="menuName"/> Name of the new menu that will be opened. Passed in from shop tab buttons
    /// </summary>
    public void openNewMenu(string menuName)
    {
        menuTag = menuName;

        newMenuObj = GameObject.FindGameObjectWithTag(menuTag);
        currentMenuObj = GameObject.FindGameObjectWithTag(currentMenu);


        currentMenuObj.transform.GetChild(0).gameObject.SetActive(false);
        newMenuObj.transform.GetChild(0).gameObject.SetActive(true);


        currentMenu = menuTag;
    }
}
