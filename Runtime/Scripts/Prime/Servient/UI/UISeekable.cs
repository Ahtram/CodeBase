using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(UIBase))]
public class UISeekable : MonoBehaviour {

    //Seek serious APIs direction define.
    public enum SeekDirection {
        Left,
        Right,
        Down,
        Up
    };

    //Can be seeked by using the Seek serious APIs?
    //This is for implement the navigation between UIBase components.
    public bool canBeSeeked = true;

    //false: automatic calculate for seek target APIs.
    //true: return the below reference object directly.
    public bool useSpecifiedSeekTarget = false;
    //Reference seek neighbor UISeekable objects.
    public UISeekable specifiedSeekNeighborOnLeft = null;
    public UISeekable specifiedSeekNeighborOnRight = null;
    public UISeekable specifiedSeekNeighborOnDown = null;
    public UISeekable specifiedSeekNeighborOnUp = null;

	//Host.
	public UIBase HostUIBase {
		get {
            return GetComponent<UIBase>();
        }
	}

    //Get specified seek neighbor by direction.
    public UISeekable GetSpecifiedNeighbor(SeekDirection direction) {
		switch(direction) {
			case SeekDirection.Left:
                return specifiedSeekNeighborOnLeft;
            case SeekDirection.Right:
                return specifiedSeekNeighborOnRight;
            case SeekDirection.Down:
                return specifiedSeekNeighborOnDown;
            case SeekDirection.Up:
                return specifiedSeekNeighborOnUp;
			default:
                return null;
        }
	}

    //Get specified seek neighbor by direction.
    public UIBase GetSpecifiedNeighborBase(SeekDirection direction) {
        UISeekable seekable = GetSpecifiedNeighbor(direction);
        if (seekable != null) {
            return seekable.HostUIBase;
        }
        return null;
    }

    //Get calculated seek neighbor by direction.
    public UISeekable SeekNeighbor(SeekDirection direction, bool includeInactive = false) {
        //Find all seekables neighbors that is toggleOn.
        List<UISeekable> seekableList = new List<UISeekable>(transform.parent.GetComponentsInChildren<UISeekable>(includeInactive));
		//Filter out those toggled-off.
        seekableList = seekableList.Where(seekable => seekable.canBeSeeked == true).ToList();

		if (direction == SeekDirection.Left) {
            //Keep only those on the left.
            List<UISeekable> seekableListOnLeft = seekableList.Where(seekable => seekable.transform.localPosition.x < transform.localPosition.x).ToList();
			if (seekableListOnLeft.Count > 0) {
                //Sort and get the nearest.
                seekableListOnLeft = seekableListOnLeft.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyVerticalWeight((Vector2)seekable.transform.localPosition - (Vector2)transform.localPosition))).ToList();
                return seekableListOnLeft[0];
            }
			//Nothing on the left. Find the suitable guy on the right.
			List<UISeekable> seekableListOnRight = seekableList.Where(seekable => seekable.transform.localPosition.x > transform.localPosition.x).ToList();
            if (seekableListOnRight.Count > 0) {
                Vector2 measurePointAtRight = new Vector2(100000.0f, transform.localPosition.y);
				seekableListOnRight = seekableListOnRight.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyVerticalWeight((Vector2)seekable.transform.localPosition - measurePointAtRight))).ToList();
				return seekableListOnRight[0];
			}
			//Nothing found.
			return null;
		} else if (direction == SeekDirection.Right) {
            //Keep only those on the right.
            List<UISeekable> seekableListOnRight = seekableList.Where(seekable => seekable.transform.localPosition.x > transform.localPosition.x).ToList();
            if (seekableListOnRight.Count > 0) {
                //Sort and get the nearest.
                seekableListOnRight = seekableListOnRight.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyVerticalWeight((Vector2)seekable.transform.localPosition - (Vector2)transform.localPosition))).ToList();
                return seekableListOnRight[0];
            }
            //Nothing on the right. Find the suitable guy on the left.
            List<UISeekable> seekableListOnLeft = seekableList.Where(seekable => seekable.transform.localPosition.x < transform.localPosition.x).ToList();
            if (seekableListOnLeft.Count > 0) {
                Vector2 measurePointAtLeft = new Vector2(-100000.0f, transform.localPosition.y);
                seekableListOnLeft = seekableListOnLeft.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyVerticalWeight((Vector2)seekable.transform.localPosition - measurePointAtLeft))).ToList();
                return seekableListOnLeft[0];
            }
            //Nothing found.
            return null;
        } else if (direction == SeekDirection.Down) {
            //Keep only those on the down.
            List<UISeekable> seekableListOnDown = seekableList.Where(seekable => seekable.transform.localPosition.y < transform.localPosition.y).ToList();
            if (seekableListOnDown.Count > 0) {
                //Sort and get the nearest.
                seekableListOnDown = seekableListOnDown.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyHorizontalWeight((Vector2)seekable.transform.localPosition - (Vector2)transform.localPosition))).ToList();
                return seekableListOnDown[0];
            }
            //Nothing on the down. Find the suitable guy on the up.
            List<UISeekable> seekableListOnUp = seekableList.Where(seekable => seekable.transform.localPosition.y > transform.localPosition.y).ToList();
            if (seekableListOnUp.Count > 0) {
                Vector2 measurePointAtUp = new Vector2(transform.localPosition.x, 100000.0f);
                seekableListOnUp = seekableListOnUp.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyHorizontalWeight((Vector2)seekable.transform.localPosition - measurePointAtUp))).ToList();
                return seekableListOnUp[0];
            }
            //Nothing found.
            return null;
        } else {
            //Up.
            //Keep only those on the up.
            List<UISeekable> seekableListOnUp = seekableList.Where(seekable => seekable.transform.localPosition.y > transform.localPosition.y).ToList();
            if (seekableListOnUp.Count > 0) {
                //Sort and get the nearest.
                seekableListOnUp = seekableListOnUp.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyHorizontalWeight((Vector2)seekable.transform.localPosition - (Vector2)transform.localPosition))).ToList();
                return seekableListOnUp[0];
            }
            //Nothing on the up. Find the suitable guy on the down.
            List<UISeekable> seekableListOnDown = seekableList.Where(seekable => seekable.transform.localPosition.y < transform.localPosition.y).ToList();
            if (seekableListOnDown.Count > 0) {
                Vector2 measurePointAtDown = new Vector2(transform.localPosition.x, -100000.0f);
                seekableListOnDown = seekableListOnDown.OrderBy(seekable => Vector2.SqrMagnitude(AmplifyHorizontalWeight((Vector2)seekable.transform.localPosition - measurePointAtDown))).ToList();
                return seekableListOnDown[0];
            }
            //Nothing found.
            return null;
		}
    }

    private Vector2 AmplifyHorizontalWeight(Vector2 vec) {
        return new Vector2(vec.x * 2.0f, vec.y);
    }

    private Vector2 AmplifyVerticalWeight(Vector2 vec) {
        return new Vector2(vec.x, vec.y * 2.0f);
    }

	//A convenient version for seek UIBase neighbor.
    public UIBase SeekNeighborBase(SeekDirection direction, bool includeInactive = false) {
        UISeekable seekable = SeekNeighbor(direction, includeInactive);
		if (seekable != null) {
            return seekable.HostUIBase;
        }
        return null;
    }

}
