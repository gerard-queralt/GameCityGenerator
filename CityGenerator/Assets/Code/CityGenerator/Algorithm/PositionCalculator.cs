using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCalculator
{
    private Bounds m_area;
    private float m_maxHeight = 0;

    public PositionCalculator(Bounds i_area)
    {
        m_area = i_area;
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
        float minY = m_area.min.y;
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
        center.y += halfExtends.y + m_maxHeight;
        float heightOfElement = center.y + i_boundingBox.size.y;
        m_maxHeight = Mathf.Max(m_maxHeight, heightOfElement); //Update maximum height
        float maxDistance = Mathf.Abs(m_maxHeight - m_area.min.y);
        LayerMask mask = LayerMask.GetMask("CityGenerator_TMPObjects");
        QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;
        bool hitExistingCityObject = Physics.BoxCast(center, halfExtends, Vector3.down, i_rotation, maxDistance, mask, queryTrigger);
        return !hitExistingCityObject;
    }
}
