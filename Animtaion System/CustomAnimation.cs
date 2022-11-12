using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "newAnimation")]
public class CustomAnimation : ScriptableObject
{
    public List<Frame> frames;

    public int frameRate;

    public bool doesLoop;
    public bool doesFlip;

}

[System.Serializable]
public class Frame{
    public float value;

    public Sprite frameSprite;

    public List<SavedHitBox> frameHitboxes;
}

[System.Serializable]
public class SavedHitBox{
    public Hitbox hitBoxData;

    public Vector2 position;
    public Vector2 size;
}





