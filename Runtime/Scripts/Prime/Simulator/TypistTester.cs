using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBlueGames.Tools.TextTyper;

public class TypistTester : MonoBehaviour {

    public string testingText1 = "";
	public string testingText2 = "";

    public Typist typist;

    void Awake() {
        typist.onTypeComplete.AddListener(OnTypeComplete);
    }

    public void OnButton1Click() {
        typist.TypeText(testingText1);
    }

	public void OnButton2Click() {
        typist.TypeText(testingText2);
    }

    public void OnCompleteClick() {
        typist.InstantComplete();
    }

	private void OnTypeComplete() {
        
    }

	public void OnFrameDown() {
        typist.SetSpeedUp(true);
    }

	public void OnFrameUp() {
		typist.SetSpeedUp(false);
    }

}
