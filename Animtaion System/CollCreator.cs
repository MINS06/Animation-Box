using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollCreator : MonoBehaviour
{

    public static BoxCollider2D RectToColl(Rect rect, Sprite sprite, BoxCollider2D collider){

        float yPosToPivot = rect.position.y - sprite.pivot.y;
        float xPosToPivot = rect.position.x - sprite.pivot.x;

        float yCenterToPivot = yPosToPivot - rect.size.y / 2;  
        float xCenterToPivot = xPosToPivot - rect.size.x / 2;

        float yOffset = yCenterToPivot / sprite.pixelsPerUnit;
        float xOffset = xCenterToPivot / sprite.pixelsPerUnit;




        float ySize = rect.size.y / sprite.pixelsPerUnit;
        float xSize = rect.size.x / sprite.pixelsPerUnit;

        xOffset += xSize;

        collider.size = new Vector2(xSize, ySize);

        collider.offset = new Vector2(xOffset, yOffset);

        collider.isTrigger = true;

        return collider;
    }
}
