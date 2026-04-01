using UnityEngine;
using System.Collections;

public class Spawner2D : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject[] objePrefablar;
    public Transform spawnPoint;
    public float hareketHizi = 7f;

    [Header("Sinirlar")]
    public float solSinir = -3.5f;
    public float sagSinir = 3.5f;

    [Header("Zamanlama")]
    public float beklemeSuresi = 1f;
    private bool atisYapabilir = true;

    private GameObject suAnkiObje;
    private Rigidbody2D suAnkiRb2D;
    private Collider2D suAnkiCollider; // BUG ÇÖZÜMÜ: Collider'ı kontrol etmek için ekledik

    public static int maxSpawnIndex = 3;

    void Start()
    {
        maxSpawnIndex = 3;
        YeniObjeHazirla();
    }

    void Update()
    {
        float yatayHareket = Input.GetAxis("Horizontal");
        Vector3 yeniPozisyon = transform.position + new Vector3(yatayHareket * hareketHizi * Time.deltaTime, 0, 0);
        yeniPozisyon.x = Mathf.Clamp(yeniPozisyon.x, solSinir, sagSinir);
        transform.position = yeniPozisyon;

        // EĞER OBJE BİR ŞEKİLDE YOK OLDUYSA SİSTEMİ KURTAR (Ekstra Güvenlik Ağı)
        if (suAnkiObje == null && atisYapabilir)
        {
            YeniObjeHazirla();
        }

        if (suAnkiObje != null && atisYapabilir)
        {
            suAnkiObje.transform.position = spawnPoint.position;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) && atisYapabilir && suAnkiObje != null)
        {
            ObjeBirak();
        }
    }

    void YeniObjeHazirla()
    {
        int guvenliIndex = Mathf.Min(maxSpawnIndex, objePrefablar.Length);
        int rastgeleIndex = Random.Range(0, guvenliIndex);
        GameObject secilenPrefab = objePrefablar[rastgeleIndex];

        suAnkiObje = Instantiate(secilenPrefab, spawnPoint.position, Quaternion.identity);

        suAnkiRb2D = suAnkiObje.GetComponent<Rigidbody2D>();
        suAnkiCollider = suAnkiObje.GetComponent<Collider2D>(); // Collider'ı bulduk

        if (suAnkiRb2D != null)
        {
            suAnkiRb2D.isKinematic = true; // Aşağı düşmesini engelle
        }

        if (suAnkiCollider != null)
        {
            suAnkiCollider.enabled = false; // BUG ÇÖZÜMÜ: Havadayken çarpışmaları tamamen kapat!
        }
    }

    void ObjeBirak()
    {
        atisYapabilir = false;

        if (suAnkiRb2D != null)
        {
            suAnkiRb2D.isKinematic = false; // Fizikleri aç, düşsün
        }

        if (suAnkiCollider != null)
        {
            suAnkiCollider.enabled = true; // BUG ÇÖZÜMÜ: Düşerken çarpışmaları geri aç!
        }

        suAnkiObje = null;

        StartCoroutine(YeniObjeBekle());
    }

    IEnumerator YeniObjeBekle()
    {
        yield return new WaitForSeconds(beklemeSuresi);
        YeniObjeHazirla();
        atisYapabilir = true;
    }

    public static void SeviyeKontrol(int ulasilanSeviye)
    {
        if (ulasilanSeviye >= 6 && maxSpawnIndex < 4) maxSpawnIndex = 4;
        if (ulasilanSeviye >= 10 && maxSpawnIndex < 5) maxSpawnIndex = 5;
    }
}