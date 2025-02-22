using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public Image typeImage;
    void Start()
    {
        UpdateCardDisplay();
    }
    public void UpdateCardDisplay()
    {

    }
}
