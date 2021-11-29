using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowBuildMetadata : MonoBehaviour
{
    [SerializeField] BuildMetadata Metadata;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = $"{Metadata.Version}-{Metadata.BuildNumber}-{Metadata.Branch} built at {Metadata.BuildTime}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
