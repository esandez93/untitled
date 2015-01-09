using UnityEngine;
using System.Collections;

public class AnyText {
	public string textId;
	public string text;
	public string speaker;

	public AnyText() {

	}

	public AnyText (string textId, string text){
		this.textId = textId;
		this.text = text;
	}

	public AnyText (string textId, string text, string speaker){
		this.textId = textId;
		this.text = text;
		this.speaker = speaker;
	}
}

