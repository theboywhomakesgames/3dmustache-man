using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BloodDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject _bloodPref;

    [SerializeField]
    private ParticleSystem _part;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 0)
        {
            _part.GetCollisionEvents(other, collisionEvents);

            foreach (ParticleCollisionEvent ce in collisionEvents)
            {
                if (ce.normal.magnitude > 0)
                {
                    Transform blood = Instantiate(_bloodPref).transform;
                    blood.forward = -ce.normal;
                    blood.position = ce.intersection;
                }
            }
        }
    }
}
