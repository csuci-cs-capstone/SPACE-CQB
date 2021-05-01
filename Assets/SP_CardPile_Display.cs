using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP_CardPile_Display : MonoBehaviour
{
    [SerializeField] int length;
    [SerializeField] int width;
    [SerializeField] int xoffset;
    [SerializeField] int yoffset;
    [SerializeField] int cardCount;
    [SerializeField] int offsetChange;

    public void UpdateDisplay()
    {
        /*while (!CheckBoundary()) ;
        if (cardCount > 1)
        {
            Transform firstcard = gameObject.transform.GetChild(0);
            if (cardCount % 2 == 0)
            {
                if(cardCount > 3)
                
                    firstcard.localPosition = new Vector3(-xoffset / 2 * ((cardCount / 2) + 1), -yoffset, 0);
                
                else
                    firstcard.localPosition = new Vector3(-xoffset / 2 * (cardCount / 2), -yoffset, 0);
            }
            else
            {
                firstcard.localPosition = new Vector3(-xoffset * (cardCount / 2), -yoffset, 0);
            }

            if(firstcard != null)
            {
                for (int i = 1; i < cardCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.transform.localPosition = new Vector3(firstcard.localPosition.x + (xoffset * i), firstcard.localPosition.y, firstcard.localPosition.z);
                }
            }
        }
        else if (cardCount == 1)
        {
            gameObject.transform.GetChild(0).localPosition = new Vector2(0, -yoffset);
        }*/
    }

    private void CheckBoundary()
    {
        /*if (cardCount * xoffset > width)
        {
            xoffset -= 1;
            return false;
        }
        else if (cardCount * xoffset < width - 30)
        {
            xoffset += offsetChange / 2;
        }
        return true;*/

        GameObject card = gameObject.transform.GetChild(0).gameObject;

        while (cardCount * (card.GetComponent<BoxCollider2D>().size.x + gameObject.GetComponent<GridLayoutGroup>().spacing.x) > width)
        {
            gameObject.GetComponent<GridLayoutGroup>().spacing = new Vector2(gameObject.GetComponent<GridLayoutGroup>().spacing.x - 1, 0);
        }/*
        while(cardCount * (card.GetComponent<BoxCollider2D>().size.x + gameObject.GetComponent<GridLayoutGroup>().spacing.x) < width / 2)
        {
            gameObject.GetComponent<GridLayoutGroup>().spacing = new Vector2(gameObject.GetComponent<GridLayoutGroup>().spacing.x + 1, 0);
        }*/
    }

    public void RefreshLayoutGroupsImmediateAndRecursive(GameObject root)
    {
        cardCount = gameObject.transform.childCount;
        if(cardCount > 0)
            CheckBoundary();
        LayoutRebuilder.ForceRebuildLayoutImmediate(root.GetComponent<RectTransform>());
    }
}
