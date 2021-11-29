using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMetadata : ScriptableObject
{
    public string Version;
    public string BuildNumber;
    public string BuildTime;
    public string Branch;
    public string CommitHash;
}
