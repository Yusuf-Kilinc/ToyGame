using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
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

    void Start()
    {
        YeniObjeHazirla();
    }

    void Update()
    {
        float yatayHareket = Input.GetAxis("Horizontal");
        Vector3 yeniPozisyon = transform.position + new Vector3(yatayHareket * hareketHizi * Time.deltaTime, 0, 0);

        yeniPozisyon.x = Mathf.Clamp(yeniPozisyon.x, solSinir, sagSinir);
        transform.position = yeniPozisyon;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) && atisYapabilir && suAnkiObje != null)
        {
            ObjeBirak();
        }
    }

    void YeniObjeHazirla()
    {
        int rastgeleIndex = Random.Range(0, objePrefablar.Length);
        GameObject secilenPrefab = objePrefablar[rastgeleIndex];

        suAnkiObje = Instantiate(secilenPrefab, spawnPoint.position, Quaternion.identity);
        suAnkiObje.transform.localScale = secilenPrefab.transform.localScale;
        suAnkiObje.transform.SetParent(spawnPoint, true);

        Rigidbody rb = suAnkiObje.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void ObjeBirak()
    {
        atisYapabilir = false;

        suAnkiObje.transform.SetParent(null, true);

        Rigidbody rb = suAnkiObje.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
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
}