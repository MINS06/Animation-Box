using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpriteConverter
{
    public static Vector2 SpriteToScreenPosition(Sprite sprite, Vector2 PositionOnSprite, Rect rectangle){
        float xPos = PositionOnSprite.x / sprite.rect.width;
        float yPos = (PositionOnSprite.y / sprite.rect.height) * -1;
        Vector2 positionOnScreen = new Vector2(rectangle.position.x + rectangle.width * xPos, rectangle.yMax + rectangle.height * yPos);
        return positionOnScreen;
    }

    public static Vector2 RectToSpritePosition(Sprite sprite, Vector2 PositionOnScreen, Rect rectangle){
        Vector2 PositionInRect = PositionOnScreen - new Vector2(rectangle.xMin, rectangle.yMax);
        float relativeYpos = (PositionInRect.y / rectangle.height) * -1;
        float relativeXpos = PositionInRect.x / rectangle.width;

        Vector2 PixelPositionInSprite = new Vector2(relativeXpos, relativeYpos) * sprite.rect.size;
        return PixelPositionInSprite;
    }

    public static Vector2 ScreenToSpriteSize(Sprite sprite, Vector2 sizeOnScreen, Vector2 screenSize){

        Vector2 sizeRelativeToScreen = sizeOnScreen / screenSize ;

        Debug.Log(sizeOnScreen);

        Vector2 sizeOnSprite = sizeRelativeToScreen * sprite.rect.size;

        return sizeOnSprite;
    }

    public static Vector2 SpriteToScreenSize(Sprite sprite, Vector2 sizeInSprite, Vector2 screenSize){
        Vector2 relativeSize = sizeInSprite / sprite.rect.size;

        Vector2 sizeOnScreen = relativeSize * screenSize;

        return sizeOnScreen;
    }

}
