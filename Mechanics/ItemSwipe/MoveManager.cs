using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    #region singleton
    public static MoveManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    private void OnEnable()
    {
        EventHelper.ItemMovementEvent += ItemMovement;
    }

    private void OnDisable()
    {
        EventHelper.ItemMovementEvent -= ItemMovement;
    }

    public Item GetItem(int x, int y)
    {
        var items = createdItems.Where(s => s.GetXCoordinate() == x);
        var item = items.FirstOrDefault(s => s.GetYCoordinate() == y);
        return item;
    }

    public void ItemMovement(Item movedItem, string swipePosition)
    {
        switch(swipePosition)
        {
            case "Left":
                if(GetItem(movedItem.GetXCoordinate() - 1, movedItem.GetYCoordinate()) == null || GetItem(movedItem.GetXCoordinate() - 1, movedItem.GetYCoordinate()).GetItemIsMatched())
                {
                    movedItem.GetItemMovement().MakeNoMovement("Left");
                    movedItem.GetItemMovement().SetCondition(false);
                }
                else
                {
                    var swipedItem = GetItem(movedItem.GetXCoordinate() - 1, movedItem.GetYCoordinate());

                    if(!swipedItem.GetItemMovement().GetCondition())
                    {
                        Movement(movedItem, swipedItem);
                    }
                }
                break;
            case "Right":
                if (GetItem(movedItem.GetXCoordinate() + 1, movedItem.GetYCoordinate()) == null || GetItem(movedItem.GetXCoordinate() + 1, movedItem.GetYCoordinate()).GetItemIsMatched())
                {
                    movedItem.GetItemMovement().MakeNoMovement("Right");
                    movedItem.gameObject.GetComponent<ItemMovement>().SetCondition(false);
                }
                else
                {
                    var swipedItem = GetItem(movedItem.GetXCoordinate() + 1, movedItem.GetYCoordinate());

                    if (!swipedItem.GetItemMovement().GetCondition())
                    {
                        Movement(movedItem, swipedItem);
                    }
                }
                break;
            case "Up":
                if (GetItem(movedItem.GetXCoordinate(), movedItem.GetYCoordinate() + 1) == null || GetItem(movedItem.GetXCoordinate(), movedItem.GetYCoordinate() + 1).GetItemIsMatched())
                {
                    movedItem.GetItemMovement().MakeNoMovement("Up");
                    movedItem.gameObject.GetComponent<ItemMovement>().SetCondition(false);
                }
                else
                {
                    var swipedItem = GetItem(movedItem.GetXCoordinate(), movedItem.GetYCoordinate() + 1);

                    if (!swipedItem.GetItemMovement().GetCondition())
                    {
                        Movement(movedItem, swipedItem);
                    }
                }
                break;
            case "Down":
                if (GetItem(movedItem.GetXCoordinate(), movedItem.GetYCoordinate() - 1) == null || GetItem(movedItem.GetXCoordinate(), movedItem.GetYCoordinate() - 1).GetItemIsMatched())
                {
                    movedItem.GetItemMovement().MakeNoMovement("Down");
                    movedItem.gameObject.GetComponent<ItemMovement>().SetCondition(false);
                }
                else
                {
                    var swipedItem = GetItem(movedItem.GetXCoordinate(), movedItem.GetYCoordinate() - 1);

                    if (!swipedItem.GetItemMovement().GetCondition())
                    {
                        Movement(movedItem, swipedItem);
                    }
                }
                break;
        }
    }

    public void Movement(Item movedItem, Item swipedItem)
    {
        if (!swipedItem.GetItemMovement().GetCondition())
        {
            var movedItemToPosition = swipedItem.gameObject.transform.position;
            var swipedItemToPosition = movedItem.gameObject.transform.position;

            var movedItemX = movedItem.GetXCoordinate();
            var movedItemY = movedItem.GetYCoordinate();

            var swipedItemX = swipedItem.GetXCoordinate();
            var swipedItemY = swipedItem.GetYCoordinate();

            movedItem.SetCoordinates(swipedItemX, swipedItemY);
            swipedItem.SetCoordinates(movedItemX, movedItemY);

            movedItem.GetItemMovement().MakeMovement(movedItemToPosition);
            swipedItem.GetItemMovement().MakeMovement(swipedItemToPosition);
        }
    }
}
