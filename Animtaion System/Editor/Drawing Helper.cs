using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

public class DrawingHelper : OdinEditorWindow
{


    public static void DrawSprite(Sprite aSprite, Rect rect)
    {
        Rect c = aSprite.rect;
        float spriteW = c.width;
        float spriteH = c.height;
        if (Event.current.type == EventType.Repaint)
            {
                var tex = aSprite.texture;
                c.xMin /= tex.width;
                c.xMax /= tex.width;
                c.yMin /= tex.height;
                c.yMax /= tex.height;
                GUI.DrawTextureWithTexCoords(rect, tex, c);
            }
    }
    
    public static Rect GetSpaceForCanvas(Sprite sprite, Vector2 screenDimensions){
        Rect canvas = new Rect();
        float spriteW = sprite.rect.width;
        float spriteH = sprite.rect.height;
        Rect rect = GUILayoutUtility.GetRect(spriteW, spriteH);


        if(spriteH > spriteW)
            canvas = GUILayoutUtility.GetRect(spriteW, spriteH, GUILayout.MaxHeight(screenDimensions.y / 2));
        else if(spriteH < spriteW){
            canvas = GUILayoutUtility.GetRect(spriteW, spriteH, GUILayout.MaxWidth(screenDimensions.x / 2), GUILayout.MaxHeight(screenDimensions.y / 2));
        }
        else if(spriteH == spriteW)
            canvas = GUILayoutUtility.GetRect(spriteW, spriteH, GUILayout.MaxHeight(screenDimensions.y / 2), GUILayout.MaxWidth(screenDimensions.y / 2));

        canvas.center = new Vector2(screenDimensions.x / 2, canvas.height / 2);
        return canvas;
    }
    
    public static void DrawPivotRect(Sprite sprite, Rect canvas){
        Vector2 pivotPos = ScreenSpriteConverter.SpriteToScreenPosition(sprite, sprite.pivot, canvas);
        Rect pivot = new Rect(0, 0, 5, 5);
        pivot.center = pivotPos;
        EditorGUI.DrawRect(pivot, Color.black);
    }
}
