using UnityEngine;

public class ToysMerge : MonoBehaviour
{
    public int toyLevel;
    public GameObject nextLevelPrefab;

    private bool isMerged = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMerged) return;

        ToysMerge otherToy = collision.gameObject.GetComponent<ToysMerge>();

        if (otherToy != null && otherToy.toyLevel == this.toyLevel && !otherToy.isMerged)
        {
            if (this.nextLevelPrefab == null) return;

            if (this.gameObject.GetInstanceID() > otherToy.gameObject.GetInstanceID())
            {
                this.isMerged = true;
                otherToy.isMerged = true;

                Vector3 midPoint = (this.transform.position + otherToy.transform.position) / 2f;
                Instantiate(nextLevelPrefab, midPoint, Quaternion.identity);

                Destroy(this.gameObject);
                Destroy(otherToy.gameObject);
            }
        }
    }
}