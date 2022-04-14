using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCalculator
{
    public Bounds ComputeBoundsOfGameObject(GameObject i_gameObject)
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

    public float FindGroundCoordinate(Vector3 i_startPosition, float i_minY)
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

    public bool CanElementBePlaced(Bounds i_boundingBox, Vector3 i_position, Quaternion i_rotation)
    {
        Vector3 halfExtends = i_boundingBox.extents;
        Vector3 center = i_position;
        center.y += halfExtends.y + 10f;
        LayerMask mask = LayerMask.GetMask("CityGenerator_TMPObjects");
        QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;
        bool hitExistingCityObject = Physics.BoxCast(center, halfExtends, Vector3.down, i_rotation, halfExtends.y + 100f, mask, queryTrigger);
        return !hitExistingCityObject;
    }
}
