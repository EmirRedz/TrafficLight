using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text btnText;
    [SerializeField] private Image btnImage;

    public Button GetButton()
    {
        return btn;
    }

    public TMP_Text GetText()
    {
        return btnText;
    }
    
    public Image GetImage()
    {
        return btnImage;
    }
}
