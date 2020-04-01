using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBlueGames.Tools.TextTyper;

public class TyperTester : MonoBehaviour {

    public string testingText1 = "";
	public string testingText2 = "";

    public Typer typer;

    void Awake() {
        typer.onTypeComplete.AddListener(OnTypeComplete);
    }

    public void OnButton1Click() {
        typer.TypeText(testingText1);
    }

	public void OnButton2Click() {
        typer.TypeText(testingText2);
    }

    public void OnCompleteClick() {
        typer.InstantComplete();
    }

	private void OnTypeComplete() {
        
    }

	public void OnFrameDown() {
        typer.SetSpeedUp(true);
    }

	public void OnFrameUp() {
		typer.SetSpeedUp(false);
    }

}
