using UnityEngine;

public class ToysMerge : MonoBehaviour
{
    public int toyLevel;
    public GameObject nextLevelPrefab;

    [HideInInspector]
    public bool isMerged = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isMerged) return;

        ToysMerge otherToy = collision.gameObject.GetComponent<ToysMerge>();

        if (otherToy != null && otherToy.toyLevel == this.toyLevel && !otherToy.isMerged)
        {
            if (this.gameObject.GetInstanceID() > otherToy.gameObject.GetInstanceID())
            {
                this.isMerged = true;
                otherToy.isMerged = true;

                if (nextLevelPrefab != null)
                {
                    Vector3 midPoint = (this.transform.position + otherToy.transform.position) / 2f;
                    Instantiate(nextLevelPrefab, midPoint, Quaternion.identity);
                }

                Destroy(this.gameObject);
                Destroy(otherToy.gameObject);
            }
        }
    }
}