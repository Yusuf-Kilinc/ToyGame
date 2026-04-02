using UnityEngine;

public class ToysMerge : MonoBehaviour
{
    [Header("Asker Ayarları")]
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


                int eklenecekPuan = this.toyLevel * 10;
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.SkorEkle(eklenecekPuan);
                }
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.BirlesmeSesiCal();
                }
                int yeniAskerSeviyesi = this.toyLevel + 1;
                if (Spawner2D.instance != null)
                {
                    Spawner2D.instance.EnYuksekSeviyeyiGuncelle(yeniAskerSeviyesi);
                }
                Destroy(this.gameObject);
                Destroy(otherToy.gameObject);
            }
        }
    }
}