using UnityEngine;
using System;

/// <summary>
/// This is actually a layer for processing Unity inputs.
/// </summary>
static public class Janus {

    //XBoX and PS4 behave differently to Unity. We provide a mode switch to dynamicly switch between them.
    public enum Mode {
        XBoxOne,
        PS4
    }

    static public string[] ModeName = new string[] {
        "Xbox One",
        "PS4"
    };

    static public int ModeTypeCount {
        get {
            return Enum.GetNames(typeof(Mode)).Length;
        }
    }

    public enum ControllerNum {
        One,
        Two
    }

    //Two different input mode for different controller.
    //static public Mode mode = Mode.XBoxOne;

    //------------ Buttons -------------

    static public bool AButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XAButton");
            } else {
                return Input.GetButtonDown("P1PAButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XAButton");
            } else {
                return Input.GetButtonDown("P2PAButton");
            }
        }
    }

    static public bool AButtonUp(Mode mode, ControllerNum controllerNum){
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XAButton");
            } else {
                return Input.GetButtonUp("P1PAButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XAButton");
            } else {
                return Input.GetButtonUp("P2PAButton");
            }
        }
    }

    static public bool BButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XBButton");
            } else {
                return Input.GetButtonDown("P1PBButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XBButton");
            } else {
                return Input.GetButtonDown("P2PBButton");
            }
        }
    }

    static public bool BButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XBButton");
            } else {
                return Input.GetButtonUp("P1PBButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XBButton");
            } else {
                return Input.GetButtonUp("P2PBButton");
            }
        }
    }

    static public bool XButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XXButton");
            } else {
                return Input.GetButtonDown("P1PXButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XXButton");
            } else {
                return Input.GetButtonDown("P2PXButton");
            }
        }
    }

    static public bool XButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XXButton");
            } else {
                return Input.GetButtonUp("P1PXButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XXButton");
            } else {
                return Input.GetButtonUp("P2PXButton");
            }
        }
    }

    static public bool YButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XYButton");
            } else {
                return Input.GetButtonDown("P1PYButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XYButton");
            } else {
                return Input.GetButtonDown("P2PYButton");
            }
        }
    }

    static public bool YButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XYButton");
            } else {
                return Input.GetButtonUp("P1PYButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XYButton");
            } else {
                return Input.GetButtonUp("P2PYButton");
            }
        }
    }

    static public bool SelectButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XSelectButton");
            } else {
                return Input.GetButtonDown("P1PSelectButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XSelectButton");
            } else {
                return Input.GetButtonDown("P2PSelectButton");
            }
        }
    }

    static public bool SelectButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XSelectButton");
            } else {
                return Input.GetButtonUp("P1PSelectButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XSelectButton");
            } else {
                return Input.GetButtonUp("P2PSelectButton");
            }
        }
    }

    static public bool StartButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XStartButton");
            } else {
                return Input.GetButtonDown("P1PStartButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XStartButton");
            } else {
                return Input.GetButtonDown("P2PStartButton");
            }
        }
    }

    static public bool StartButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XStartButton");
            } else {
                return Input.GetButtonUp("P1PStartButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XStartButton");
            } else {
                return Input.GetButtonUp("P2PStartButton");
            }
        }
    }

    static public bool LBButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XLBButton");
            } else {
                return Input.GetButtonDown("P1PLBButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XLBButton");
            } else {
                return Input.GetButtonDown("P2PLBButton");
            }
        }
    }

    static public bool LBButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XLBButton");
            } else {
                return Input.GetButtonUp("P1PLBButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XLBButton");
            } else {
                return Input.GetButtonUp("P2PLBButton");
            }
        }
    }

    static public bool RBButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XRBButton");
            } else {
                return Input.GetButtonDown("P1PRBButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XRBButton");
            } else {
                return Input.GetButtonDown("P2PRBButton");
            }
        }
    }

    static public bool RBButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XRBButton");
            } else {
                return Input.GetButtonUp("P1PRBButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XRBButton");
            } else {
                return Input.GetButtonUp("P2PRBButton");
            }
        }
    }

    static public bool LJButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XLJButton");
            } else {
                return Input.GetButtonDown("P1PLJButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XLJButton");
            } else {
                return Input.GetButtonDown("P2PLJButton");
            }
        }
    }

    static public bool LJButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XLJButton");
            } else {
                return Input.GetButtonUp("P1PLJButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XLJButton");
            } else {
                return Input.GetButtonUp("P2PLJButton");
            }
        }
    }

    static public bool RJButtonDown(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P1XRJButton");
            } else {
                return Input.GetButtonDown("P1PRJButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonDown("P2XRJButton");
            } else {
                return Input.GetButtonDown("P2PRJButton");
            }
        }
    }

    static public bool RJButtonUp(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P1XRJButton");
            } else {
                return Input.GetButtonUp("P1PRJButton");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetButtonUp("P2XRJButton");
            } else {
                return Input.GetButtonUp("P2PRJButton");
            }
        }
    }

    //--------------- Axis --------------

    //-1.0f ~ 1.0f
    static public float LJHorizontal(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XLJHorizontal");
            } else {
                return Input.GetAxis("P1PLJHorizontal");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XLJHorizontal");
            } else {
                return Input.GetAxis("P2PLJHorizontal");
            }
        }
    }

    static public float LJVertical(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XLJVertical");
            } else {
                return Input.GetAxis("P1PLJVertical");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XLJVertical");
            } else {
                return Input.GetAxis("P2PLJVertical");
            }
        }
    }

    static public float RJHorizontal(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XRJHorizontal");
            } else {
                return Input.GetAxis("P1PRJHorizontal");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XRJHorizontal");
            } else {
                return Input.GetAxis("P2PRJHorizontal");
            }
        }
    }

    static public float RJVertical(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XRJVertical");
            } else {
                return Input.GetAxis("P1PRJVertical");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XRJVertical");
            } else {
                return Input.GetAxis("P2PRJVertical");
            }
        }
    }

    static public float CJHorizontal(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XCHorizontal");
            } else {
                return Input.GetAxis("P1PCHorizontal");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XCHorizontal");
            } else {
                return Input.GetAxis("P2PCHorizontal");
            }
        }
    }

    static public float CJVertical(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XCVertical");
            } else {
                return Input.GetAxis("P1PCVertical");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XCVertical");
            } else {
                return Input.GetAxis("P2PCVertical");
            }
        }
    }

    //[Preserve for reference]
    ////The will detect LJHorizontal and CJHorizontal.
    //static public float Horizontal(Mode mode, ControllerNum controllerNum) {
    //    float axisLJHorizontal = LJHorizontal(mode, controllerNum);
    //    if(!Mathf.Approximately(0.0f, axisLJHorizontal)) {
    //        return axisLJHorizontal;
    //    } else {
    //        return CJHorizontal(mode, controllerNum);
    //    }
    //}

    ////The will detect LJVertical and CJVertical.
    //static public float Vertical(Mode mode, ControllerNum controllerNum) {
    //    float axisLJVertical = LJVertical(mode, controllerNum);
    //    if (!Mathf.Approximately(0.0f, axisLJVertical)) {
    //        return axisLJVertical;
    //    } else {
    //        return CJVertical(mode, controllerNum);
    //    }
    //}

    static public float LT(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XLT");
            } else {
                return Input.GetAxis("P1PLT");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XLT");
            } else {
                return Input.GetAxis("P2PLT");
            }
        }
    }

    static public float RT(Mode mode, ControllerNum controllerNum) {
        if (controllerNum == ControllerNum.One) {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P1XRT");
            } else {
                return Input.GetAxis("P1PRT");
            }
        } else {
            if (mode == Mode.XBoxOne) {
                return Input.GetAxis("P2XRT");
            } else {
                return Input.GetAxis("P2PRT");
            }
        }
    }
}
