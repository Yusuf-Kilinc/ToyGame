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
        int rastgeleIndex = Random.Range(0, objePrefablar.Length);
        GameObject secilenPrefab = objePrefablar[rastgeleIndex];

        suAnkiObje = Instantiate(secilenPrefab, spawnPoint.position, Quaternion.identity);

        suAnkiRb2D = suAnkiObje.GetComponent<Rigidbody2D>();
        if (suAnkiRb2D != null)
        {
            suAnkiRb2D.isKinematic = true;
        }
    }
    void ObjeBirak()
    {
        atisYapabilir = false;

        if (suAnkiRb2D != null)
        {
            suAnkiRb2D.isKinematic = false;
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