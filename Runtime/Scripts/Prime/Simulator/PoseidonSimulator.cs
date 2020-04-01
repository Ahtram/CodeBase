using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseidonSimulator : MonoBehaviour {

    //8 testers. (By joystick index order)
    public NereusVisualTester[] testers;

    // Use this for initialization
    void Awake() {
        //Start listen to Poseison.
        Poseidon.onControllerChanged = OnControllerChanged;
    }

    private void OnControllerChanged(int index, Triton.ControllerType type) {
        //Setup the corresbond tester.
        if (index >= 0 && index < testers.Length) {
            testers[index].Setup(Poseidon.GetNereus(index), type);
        }
    }

}
