using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

public class AnimationBox : OdinEditorWindow
{
    private Vector2 ScreenDimensions;

    bool showStartMenu = true;
    bool showAnimationMenu = false;

    bool showPivot;

    int currentFrame = 0;
    [ShowIf("showStartMenu")]
    public CustomAnimation animation;
    [ShowIf("showStartMenu")]
    public HitBoxDataBase hitBox;


    private void Start()
    {
        if(animation != null){
            showStartMenu = false;
            showAnimationMenu = true;
        }
        else{
            showStartMenu = true;
            showAnimationMenu = false;
        }

    }

    private void goHome(){
        showStartMenu = true;
        showAnimationMenu = false;
    }

    [MenuItem("Tools/Animation Box")]
    public static void ShowWindow()
    {
        AnimationBox wnd = GetWindow<AnimationBox>();
        wnd.titleContent = new GUIContent("Animation Box");
    }

    Event currentEvent;
    [OnInspectorGUI]
    private void OnInspectorGUI() {

        currentEvent = Event.current;
        ScreenDimensions = new Vector2(position.width, position.height);
        if(showAnimationMenu){
            previewDrawer();
            DrawLeftMenu();
            DrawTimeline();
            DrawRigthMenu();
        


        }
        if(showStartMenu){
            if(GUILayout.Button("Start Program", GUILayout.Width(ScreenDimensions.x / 5), GUILayout.Height(20))){
                Start();
            }
        }

    }


    private void DrawTimeline(){
        VerticalDivision();
    }

    private void previewDrawer(){
        int previous = currentFrame;
        currentFrame = UpdateCurrentFrame(currentFrame);
        DrawSprite(animation.frames[currentFrame].frameSprite);
        CheckForRepaint(previous, currentFrame);
        DrawPivotRect();
    }


    void CheckForRepaint(int pr, int cu){
        if(pr != cu){

            Repaint();
        }
;
    }

    private int UpdateCurrentFrame(int nr){

        if(currentEvent.type == EventType.KeyDown){
            if(Event.current.keyCode == (KeyCode.LeftArrow))
                nr -= 1;
            else if(Event.current.keyCode == (KeyCode.RightArrow))
                nr += 1;
            if(nr == animation.frames.Count)
                nr = 0;
            else if(nr < 0)
                nr = animation.frames.Count - 1;
            return nr;
        }
        return nr;



    }

    private void DrawLeftMenu(){
        GUIStyle Fontsytle = new GUIStyle();
        Fontsytle.fontSize = 20;
        Fontsytle.fontStyle = FontStyle.Bold;
        EditorGUI.LabelField(new Rect(0,0,ScreenDimensions.x / 5, 20), "Animation Type", Fontsytle);

    }
    int currentHitbox;
    
    bool showSelection = false;
    private void DrawRigthMenu(){
        Rect ButtonSpace = new Rect(0,0,ScreenDimensions.x / 8, 30);
        ButtonSpace.xMax = ScreenDimensions.x;
        ButtonSpace.xMin = ScreenDimensions.x - ScreenDimensions.x / 8;
        if(GUI.Button(ButtonSpace, "Home")){
            goHome();
        }
        ButtonSpace.xMax = ButtonSpace.xMin;
        ButtonSpace.xMin = ScreenDimensions.x - ScreenDimensions.x / 4 + 5;
        showPivot = GUI.Toggle(ButtonSpace, showPivot, "show Pivot");


        ButtonSpace.yMin = ButtonSpace.yMax;
        ButtonSpace.yMax = ScreenDimensions.y;

        Button[] buttons = new Button[hitBox.hitboxes.Length];
        Rect singleButton = new Rect(ButtonSpace.position, new Vector2(ButtonSpace.width, ButtonSpace.height / hitBox.hitboxes.Length));





        if(showSelection == false){
            if(EditorGUI.DropdownButton(singleButton, new GUIContent(hitBox.hitboxes[currentHitbox].name), FocusType.Keyboard)){
                showSelection = true;
            }
        }


        if(showSelection){
            for(int i = 0; i < hitBox.hitboxes.Length; i++){

                singleButton.position = new Vector2(ButtonSpace.position.x, ButtonSpace.position.y + singleButton.height * i);
                //singleButton.xMax = ButtonSpace.xMax;
                if(GUI.Button(singleButton, hitBox.hitboxes[i].name)){
                    currentHitbox = i;
                    showSelection = false;
                }
            }
        }

    }

    Rect canvas;
    Vector2 pivotPos;
    void DrawSprite(Sprite aSprite)
    {
        


        canvas = DrawingHelper.GetSpaceForCanvas(aSprite, ScreenDimensions);

        EditorGUI.DrawRect(canvas, Color.grey);

        DrawingHelper.DrawSprite(aSprite, canvas);


        if(showPivot){
            Vector2 pivotPos = ScreenSpriteConverter.SpriteToScreenPosition(aSprite, aSprite.pivot, canvas);
            Rect pivot = new Rect(0, 0, 5, 5);
            pivot.center = pivotPos;
            EditorGUI.DrawRect(pivot, Color.black);
        }


        for(int i = 0; i < animation.frames[currentFrame].frameHitboxes.Count; i ++){
            SavedHitBox box = animation.frames[currentFrame].frameHitboxes[i];
            Rect boxToDraw = new Rect(ScreenSpriteConverter.SpriteToScreenPosition(aSprite, box.position, canvas),ScreenSpriteConverter.SpriteToScreenSize(aSprite, box.size, canvas.size));
            DrawHitBox(boxToDraw, box.hitBoxData.color);
        }
    }
    
    bool MouseIsPressed;
    Vector2 startPosition;
    Rect mousePosition;
    private void DrawPivotRect(){

        if(currentEvent.type == EventType.MouseDown && canvas.Contains(currentEvent.mousePosition)){
            MouseIsPressed = true;
            startPosition = currentEvent.mousePosition;
        }
        if(currentEvent.type == EventType.MouseUp){

            if(Mathf.Abs(mousePosition.size.x) >= 20 && Mathf.Abs(mousePosition.size.y) > 20 && MouseIsPressed){
                SavedHitBox HitboxToSave = CreateSavedHitbox(hitBox.hitboxes[currentHitbox], mousePosition);
                animation.frames[currentFrame].frameHitboxes.Add(HitboxToSave);
                EditorUtility.SetDirty(animation);
            }
            MouseIsPressed = false;

        }
        if(MouseIsPressed){
            mousePosition = new Rect(startPosition, new Vector2(currentEvent.mousePosition.x - startPosition.x, currentEvent.mousePosition.y - startPosition.y));
            DrawHitBox(mousePosition, hitBox.hitboxes[currentHitbox].color);


        }

        

    }
    private SavedHitBox CreateSavedHitbox(Hitbox data, Rect HitBoxToSave){
        SavedHitBox CreatedhitBox = new SavedHitBox();
        CreatedhitBox.hitBoxData = data;
 
        CreatedhitBox.position = ScreenSpriteConverter.RectToSpritePosition(animation.frames[currentFrame].frameSprite, startPosition, canvas);
        CreatedhitBox.size = ScreenSpriteConverter.ScreenToSpriteSize(animation.frames[currentFrame].frameSprite, HitBoxToSave.size, canvas.size);


        return CreatedhitBox;
        
    }

    private List<Rect> VerticalDivision(){

        int nrOfFrames = animation.frames.Count;
        float TimelineWidth = ScreenDimensions.x - ScreenDimensions.x / 6;
        float InfoBoxWidth = ScreenDimensions.x / 6;

        float sectionWidth = TimelineWidth / nrOfFrames;
        List<Rect> sections = new List<Rect>();

        for(int i = 0; i < nrOfFrames; i++){
            Rect section = new Rect((InfoBoxWidth * 0.6f) + sectionWidth * i, ScreenDimensions.y / 2, sectionWidth, ScreenDimensions.y / 2);
            sections.Add(section);
        }


        Color color = new Color(0.5f,0.5f,0.5f, 0.3f);
        EditorGUI.DrawRect(new Rect(sections[0].position, new Vector2(TimelineWidth, ScreenDimensions.y / 2)), color);
        for(float c = 0; c < nrOfFrames; c ++){

            float dividor = 1f / nrOfFrames * c;

            Rect cR = sections[(int)c];
            Drawing.DrawLine(new Vector2(cR.position.x, cR.yMin), new Vector2(cR.xMin , cR.yMax), Color.black, 5, false);
        }
        Rect draw = sections[sections.Count - 1];
        Drawing.DrawLine(new Vector2(draw.xMax, draw.yMin), new Vector2(draw.xMax, draw.yMax), Color.black, 5, false);

        return sections;
    }

    private void FrameMenu(){

    }

    private void DrawHitBox(Rect BoxToDraw, Color color){

            Color transparent = new Color(color.r, color.g, color.b, 0.5f);
            EditorGUI.DrawRect(BoxToDraw, transparent);

            //Drawing.DrawLine(BoxToDraw.position, new Vector2(BoxToDraw.position.x, BoxToDraw.yMax), color, 5, false);
            //Drawing.DrawLine(new Vector2(BoxToDraw.xMax, BoxToDraw.yMax), new Vector2(BoxToDraw.xMax, BoxToDraw.yMin), color, 5, false);
            //Drawing.DrawLine(new Vector2(BoxToDraw.xMin - BoxToDraw.width / 2, BoxToDraw.yMin), new Vector2(BoxToDraw.xMax - BoxToDraw.width / 2, BoxToDraw.yMin) , color, 5, false);
            //Drawing.DrawLine(new Vector2(BoxToDraw.xMin - BoxToDraw.width / 2, BoxToDraw.yMax), new Vector2(BoxToDraw.xMax - BoxToDraw.width / 2, BoxToDraw.yMax), color, 5, false);
    }

}