using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationPlayer : MonoBehaviour
{

    public static IEnumerator PlayAnimation(CustomAnimation animation, Executer subject, SpriteRenderer renderer){

        float SPF = (float) 1 / animation.frameRate;

        List<Hitbox> hitboxTypes = new List<Hitbox>();
        List<string> nameTags = new List<string>();

        //checking how many different Hitbox types there are in the animation
        for(int i = 0; i < animation.frames.Count; i++){

            for(int c = 0; c < animation.frames[i].frameHitboxes.Count; c++){

                hitboxTypes.Add(animation.frames[i].frameHitboxes[c].hitBoxData);

            }
        }

        List<GameObject> objects = new List<GameObject>();

        for(int i = 0; i < hitboxTypes.Count; i ++){
            if(!nameTags.Contains(hitboxTypes[i].name)){
                nameTags.Add(hitboxTypes[i].name);

                GameObject gobj = new GameObject();
                Destroy(gobj);
                gobj = Instantiate(gobj, subject.transform, false);
                gobj.name = hitboxTypes[i].name;
                gobj.tag = "Hitbox";
                gobj.layer = Helpers.LayerMaskIndex(hitboxTypes[i].layer);
                objects.Add(gobj);
            }
        }
        int currentFrame = 0;
        List<BoxCollider2D> currentColls = new List<BoxCollider2D>();
        while(true){
            if(animation.frames[currentFrame].frameHitboxes is not null){
                
                for(int i = 0; i < animation.frames[currentFrame].frameHitboxes.Count; i++){
                    Vector2 size = animation.frames[currentFrame].frameHitboxes[i].size;
                    Vector2 pos = animation.frames[currentFrame].frameHitboxes[i].position;
                    for(int c = 0; c < objects.Count; c++){
                        if(objects[c].name == animation.frames[currentFrame].frameHitboxes[i].hitBoxData.name){
                            BoxCollider2D coll = objects[c].AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;

                            coll = CollCreator.RectToColl(new Rect(pos, size), animation.frames[currentFrame].frameSprite, coll);
                            currentColls.Add(coll);
                        }

                    }

                }

            }
            

            renderer.sprite = animation.frames[currentFrame].frameSprite;

            yield return new WaitForSeconds(SPF);
            if(animation.frames[currentFrame].frameHitboxes.Count > 0){
                for(int i = 0; i < currentColls.Count; i++){
                    Destroy(currentColls[i]);
                }
            }
            currentFrame ++;
            if(currentFrame >= animation.frames.Count){
                currentFrame = 0;
                if(!animation.doesLoop){
                    for(int i = 0; i < objects.Count; i++){
                        Destroy(objects[i]);
                    }
                    subject.InternEndState();
                    break;
                }
            }
        }

        


    }
}


