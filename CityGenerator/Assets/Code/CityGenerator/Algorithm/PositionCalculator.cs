using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCalculator
{
    private Bounds m_districtArea;
    private float m_maxHeight = 0;

    public PositionCalculator(Bounds i_districtArea)
    {
        m_districtArea = i_districtArea;
    }

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

    public float FindGroundCoordinate(Vector3 i_startPosition)
    {
        Vector3 direction = Vector3.down;
        float minY = m_districtArea.min.y;
        float maxDistance = i_startPosition.y - minY;
        LayerMask mask = LayerMask.GetMask("Default");
        QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;
        RaycastHit hit;
        if(Physics.Raycast(i_startPosition, direction, out hit, maxDistance, mask, queryTrigger))
        {
            return hit.point.y;
        }
        return minY;
    }

    public bool CanElementBePlaced(Bounds i_boundingBox, Vector3 i_position, Quaternion i_rotation)
    {
        Vector3 halfExtends = i_boundingBox.extents;
        Vector3 center = i_position;
        center.y += halfExtends.y;
        LayerMask mask = LayerMask.GetMask("CityGenerator_TMPObjects");
        QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;
        Collider[] overlapped = Physics.OverlapBox(center, halfExtends, i_rotation, mask, queryTrigger);
        return overlapped.Length == 0;
    }
}
