using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UtilTool {
    public static string getOriName(string name) {
        if (name.IndexOf("Clone") == -1)
            return name;
        return name.Substring(0, name.IndexOf("Clone")-1);
    }

}