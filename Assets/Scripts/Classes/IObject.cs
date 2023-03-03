using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObject
{
    ObjectData objectData { get; set; }
    Vector3 point { get; set; }
    bool isAutomatic { get; set; }
}
