using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private float _radius = 10f;
    private float _force = 14f;
    private bool _explosion;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            if (_explosion)
                return;
            
            _explosion = true;
            var explotionPos = transform.position;
            var colliders = Physics.OverlapSphere(explotionPos, _radius);
            var allCollider = Physics.OverlapSphere(explotionPos, _radius*5);
            var smokeEffect = Instantiate(EffectManager.Instance.Smoke, explotionPos,Quaternion.identity);
            Destroy(smokeEffect,1.5f);
            foreach (var collider in colliders)
            {
                if (collider.transform.gameObject.CompareTag("Brick"))
                {
                    var rb = collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(_force,explotionPos,_radius,.05f,ForceMode.Impulse);
                    }
                }
            }
            SetKinematicBrick(allCollider);
        }
        if (collision.gameObject.CompareTag("Dynamite"))
        {
            if (_explosion)
                return;
            
            _explosion = true;
            var explotionPos = transform.position;
            var allCollider = Physics.OverlapSphere(explotionPos, _radius*5);
            var colliders = Physics.OverlapSphere(explotionPos, _radius*2);
            var smokeEffect = Instantiate(EffectManager.Instance.SmokeDynamite, explotionPos,Quaternion.identity);
            Destroy(smokeEffect,1.5f);
            foreach (var collider in colliders)
            {
                if (collider.transform.gameObject.CompareTag("Dynamite"))
                {
                    var rb = collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(_force*5,explotionPos,_radius*5,3f,ForceMode.Impulse);
                    }
                    
                    StartCoroutine(ExplosionNextTnt());
                    
                    IEnumerator ExplosionNextTnt()
                    {
                        yield return new WaitForSeconds(.3f);
                        if (collider != null)
                        {
                            var explosionSmokeEffect = Instantiate(EffectManager.Instance.SmokeDynamite, collider.transform.position,Quaternion.identity);
                            Destroy(explosionSmokeEffect,1.5f);
                            Destroy(collider.gameObject);
                        }
                    }
                }
                if (collider.transform.gameObject.CompareTag("Brick"))
                {
                    var rb = collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(_force*5,explotionPos,_radius*5,3f,ForceMode.Impulse);
                    }
                }
            }
            SetKinematicBrick(allCollider);
            Destroy(collision.gameObject);
        }
    }

    private void SetKinematicBrick(Collider[] allCollider)
    {
        foreach (var collider in allCollider)
        {
            if (collider.transform.gameObject.CompareTag("Brick"))
            {
                var rb = collider.GetComponent<Rigidbody>();
                rb.isKinematic = false; 
            }
        }
    }
}
