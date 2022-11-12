using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hitboxes")]
public class HitBoxDataBase : ScriptableObject{
    public Hitbox[] hitboxes;
}

[System.Serializable]
public class Hitbox{
    public string name;

    public Color color;

    public LayerMask layer;
}
