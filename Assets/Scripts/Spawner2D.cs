using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spawner2D : MonoBehaviour
{
    public static Spawner2D instance;

    [Header("Ayarlar")]
    public GameObject[] objePrefablar;
    public Transform spawnPoint;
    public float hareketHizi = 7f;

    [Header("UI ve İlerleme Sistemi")]
    public Image enYuksekRutbeUI;
    public Sprite[] rutbePortreleri;
    public int ulasilanEnUstSeviye = 1; // Oyun artık net olarak 1. seviyeden başlıyor

    [Header("Dinamik Spawner Limiti")]
    public int spawnerDusebilecekMaxSeviye = 4; // Oyun dengesi için yukarıdan en fazla 4. seviye asker düşebilir (İstersen artır)
    public int maxSpawnIndex = 1; // Spawner'ın o an atabileceği max seviye (Oyun başı 1)

    [Header("Sinirlar")]
    public float solSinir = -3.5f;
    public float sagSinir = 3.5f;

    [Header("Zamanlama")]
    public float beklemeSuresi = 1f;
    private bool atisYapabilir = true;

    private GameObject suAnkiObje;
    private Rigidbody2D suAnkiRb2D;
    private Collider2D suAnkiCollider;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        // Oyun başlarken direkt 1. Seviye portreyi açıyoruz
        ulasilanEnUstSeviye = 1;
        maxSpawnIndex = 1;

        if (enYuksekRutbeUI != null)
        {
            enYuksekRutbeUI.gameObject.SetActive(true);
            if (rutbePortreleri.Length > 0)
            {
                enYuksekRutbeUI.sprite = rutbePortreleri[0];
            }
        }

        YeniObjeHazirla();
    }

    void Update()
    {
        float yatayHareket = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(yatayHareket) > 0.1f)
        {
            Vector3 yeniPozisyon = transform.position + new Vector3(yatayHareket * hareketHizi * Time.deltaTime, 0, 0);
            yeniPozisyon.x = Mathf.Clamp(yeniPozisyon.x, solSinir, sagSinir);
            transform.position = yeniPozisyon;

            if (AudioManager.instance != null) AudioManager.instance.HareketSesiCal();
        }
        else
        {
            if (AudioManager.instance != null) AudioManager.instance.HareketSesiDurdur();
        }

        if (suAnkiObje == null && atisYapabilir) YeniObjeHazirla();

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
        // maxSpawnIndex'e göre rastgele bir asker seç (Örn: maxSpawnIndex 3 ise, 0, 1, veya 2. index gelir)
        int guvenliIndex = Mathf.Min(maxSpawnIndex, objePrefablar.Length);
        int rastgeleIndex = Random.Range(0, guvenliIndex);

        GameObject secilenPrefab = objePrefablar[rastgeleIndex];
        suAnkiObje = Instantiate(secilenPrefab, spawnPoint.position, Quaternion.identity);

        suAnkiRb2D = suAnkiObje.GetComponent<Rigidbody2D>();
        suAnkiCollider = suAnkiObje.GetComponent<Collider2D>();

        if (suAnkiRb2D != null) suAnkiRb2D.isKinematic = true;
        if (suAnkiCollider != null) suAnkiCollider.enabled = false;
    }

    void ObjeBirak()
    {
        atisYapabilir = false;

        if (AudioManager.instance != null) AudioManager.instance.FirlatmaSesiCal();

        if (suAnkiRb2D != null) suAnkiRb2D.isKinematic = false;
        if (suAnkiCollider != null) suAnkiCollider.enabled = true;

        suAnkiObje = null;

        StartCoroutine(YeniObjeBekle());
    }

    IEnumerator YeniObjeBekle()
    {
        yield return new WaitForSeconds(beklemeSuresi);
        YeniObjeHazirla();
        atisYapabilir = true;
    }

    // --- SENİN İSTEDİĞİN DİNAMİK HAVUZ SİSTEMİ BURADA ---
    public void EnYuksekSeviyeyiGuncelle(int yeniSeviye)
    {
        // Eğer yeni birleşen asker rekorumuzdan büyükse:
        if (yeniSeviye > ulasilanEnUstSeviye)
        {
            ulasilanEnUstSeviye = yeniSeviye; // Rekoru güncelle

            // Spawner'dan düşebilecek asker havuzunu genişlet (Sınırı aşmamak şartıyla)
            if (maxSpawnIndex < spawnerDusebilecekMaxSeviye && yeniSeviye <= spawnerDusebilecekMaxSeviye)
            {
                maxSpawnIndex = yeniSeviye;
            }

            // Sağdaki UI Tablosunu güncelle
            if (enYuksekRutbeUI != null)
            {
                int index = yeniSeviye - 1;
                if (index >= 0 && index < rutbePortreleri.Length)
                {
                    enYuksekRutbeUI.sprite = rutbePortreleri[index];
                }
            }
        }
    }
}