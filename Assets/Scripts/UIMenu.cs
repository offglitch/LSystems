using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{

    /*
     Getters and setters for the UI text boxes
     */

    public Text rule1UI, rule2UI, iterationsUI, axiomUI, lsystemUI;
    public Text angleUI, lengthUI;

    public static int lsystemNumText;
    public static string axiom;
    public static string iterations;
    public static string rule1Text;
    public static string rule2Text;
    public static float angleText;
    public static float lengthText;

    void Update()
    {
        lsystemUI.text = lsystemNumText.ToString();
        axiomUI.text = axiom;
        iterationsUI.text = iterations;
        rule1UI.text = rule1Text;
        rule2UI.text = rule2Text;
        angleUI.text = angleText.ToString();
        lengthUI.text = lengthText.ToString();
    }
}
