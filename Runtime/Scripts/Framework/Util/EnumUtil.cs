using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

//Thanks for: https://stackoverflow.com/questions/26678181/enum-parse-vs-switch-performance
//This help to speed up enum to string or string to enum performance when you need to convert them many many times. 
public class EnumUtil<TTarget> {
	private readonly Dictionary<string, TTarget> StringToEnumDict;
    private readonly Dictionary<int, string> EnumToStringDict;

    public EnumUtil() {
        string[] names = Enum.GetNames(typeof(TTarget));
        StringToEnumDict = new Dictionary<string, TTarget>();
        EnumToStringDict = new Dictionary<int, string>();
        for (int i = 0; i < names.Length; i++) {
            TTarget enumValue = (TTarget)Enum.Parse(typeof(TTarget), names[i]);
            StringToEnumDict.Add(names[i], enumValue);
            int intValue = (int)Convert.ChangeType(enumValue, typeof(int));
            if (!EnumToStringDict.ContainsKey(intValue)) {
                EnumToStringDict.Add(intValue, names[i]);
			} else {
				//This warnning will pop-up for Unity Input.KeyCode because they did tricky stuff in their shit.
                // Debug.LogWarning("Oops! Key exist:(" + intValue + "): " + (TTarget)Enum.Parse(typeof(TTarget), names[i], false));
            }
        }
    }

    public TTarget ToEnum(string value) {
        return StringToEnumDict[value];
    }

    public string ToString(TTarget target) {
        int intValue = (int)Convert.ChangeType(target, typeof(int));
		if (EnumToStringDict.ContainsKey(intValue)) {
            return EnumToStringDict[intValue];
		}
        return "";
    }
}