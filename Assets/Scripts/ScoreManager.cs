using UnityEngine;
using TMPro; // TextMeshPro'yu kullanmak için bu şart!

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Diğer kodların buraya kolayca ulaşması için

    [Header("UI (Arayüz) Elemanları")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    [Header("Kombo Ayarları")]
    public float comboSuresi = 1.5f; // Askerler 1.5 saniye içinde art arda birleşirse kombo artar!

    private int toplamSkor = 0;
    private int comboSayaci = 0;
    private float comboZamanlayici = 0f;

    void Awake()
    {
        // Singleton mantığı: Sahnede sadece bir tane ScoreManager olsun
        if (instance == null) instance = this;
    }

    void Update()
    {
        // Eğer kombo başladıysa, sayacı geriye doğru akıt
        if (comboSayaci > 0)
        {
            comboZamanlayici -= Time.deltaTime;

            // Süre bittiyse komboyu sıfırla
            if (comboZamanlayici <= 0)
            {
                KomboSifirla();
            }
        }
    }

    public void SkorEkle(int temelPuan)
    {
        comboSayaci++;
        comboZamanlayici = comboSuresi; // Kombo süresini yenile (hayatta tut)

        // Asıl büyü burada: 2 komboda x2 puan, 5 komboda x5 puan!
        int kazanilanPuan = temelPuan * comboSayaci;
        toplamSkor += kazanilanPuan;

        EkraniGuncelle();
    }

    void KomboSifirla()
    {
        comboSayaci = 0;
        if (comboText != null) comboText.text = ""; // Ekranda kombo yazısını gizle
    }

    void EkraniGuncelle()
    {
        if (scoreText != null)
            scoreText.text = "SKOR: " + toplamSkor.ToString();

        if (comboText != null)
        {
            if (comboSayaci > 1)
            {
                // Sadece art arda birleşmelerde "2x KOMBO!" yazsın
                comboText.text = comboSayaci + "x KOMBO!";
            }
        }
    }
}