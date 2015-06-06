 using UnityEngine;
 
 public class Credits : MonoBehaviour {
     private float offset;
     public float speed = 15.0f;
     public GUIStyle style;
     public Rect viewArea;
 
     private void Start(){
		if (this.viewArea.width == 0.0f)             
            this.viewArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height);             

         this.offset = this.viewArea.height;
     }
 
     private void Update(){
         this.offset -= Time.deltaTime * this.speed;
     }
 
     private void OnGUI(){
		GUI.BeginGroup(this.viewArea);

		Rect position = new Rect(0, this.offset, this.viewArea.width, 5000);//this.viewArea.height);
		string text = LanguageManager.Instance.getMenuText("credits_text");

		style.fontSize = 24;

        GUI.Label(position, text, this.style);
 
        GUI.EndGroup();
     }
 }