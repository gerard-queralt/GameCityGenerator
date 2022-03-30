using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCalculator
{
    public static Bounds ComputeBoundsOfGameObject(GameObject i_gameObject)
    {
        Bounds boundsOfElement = new Bounds(i_gameObject.transform.position, Vector3.one);
        MeshRenderer[] renderers = i_gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            Bounds rendererBounds = renderer.bounds;
            boundsOfElement.Encapsulate(rendererBounds);
        }
        return boundsOfElement;
    }

    public static float FindGroundCoordinate(Vector3 i_startPosition, float i_minY)
    {
        Vector3 direction = Vector3.down;
        float maxDistance = i_startPosition.y - i_minY;
        LayerMask mask = LayerMask.GetMask("Default");
        QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;
        RaycastHit hit;
        if(Physics.Raycast(i_startPosition, direction, out hit, maxDistance, mask, queryTrigger))
        {
            return hit.point.y;
        }
        return i_minY;
    }
}
