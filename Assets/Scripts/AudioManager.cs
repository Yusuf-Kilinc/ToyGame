using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Ses Kaynakları (Audio Sources)")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource hareketSource; // A/D için ayrı bir source, daha iyi kontrol için

    [Header("Ses Dosyaları (Audio Clips)")]
    public AudioClip arkaPlanMuzigi;
    public AudioClip birlesmeSesi; // Telefon ahizesi sesi (Pitch 1.2)
    public AudioClip firlatmaSesi; // 404785__owlstorm__retro...wobble-down (Pitch 0.9-1.0)
    public AudioClip hareketSesi;   // 253173__suntemple__retro...falling-down (Pitch 1.5-2.0, çok kısa)

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (bgmSource != null && arkaPlanMuzigi != null)
        {
            bgmSource.clip = arkaPlanMuzigi;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void BirlesmeSesiCal()
    {
        if (sfxSource != null && birlesmeSesi != null)
        {
            sfxSource.PlayOneShot(birlesmeSesi, 1f);
        }
    }

    // Spawner2D'den çağrılacak
    public void FirlatmaSesiCal()
    {
        if (sfxSource != null && firlatmaSesi != null)
        {
            // Fırlatma sesi biraz daha tatlı ve tok olsun
            sfxSource.PlayOneShot(firlatmaSesi, 0.9f);
        }
    }

    // Spawner2D'den çağrılacak
    public void HareketSesiCal()
    {
        // Hareket sesini 'PlayOneShot' ile çalmıyoruz, 
        // çünkü çok hızlı tekrar edip gürültü yapabilir. 
        // Ayrı bir source üzerinde Play/Stop mantığı kullanacağız.
        if (hareketSource != null && hareketSesi != null)
        {
            if (!hareketSource.isPlaying)
            {
                hareketSource.clip = hareketSesi;
                // 'falling down' sesi hareket için çok uzun, 
                // bu yüzden Pitch'i çok arttırarak 'mekanik bir tık' haline getiriyoruz.
                hareketSource.pitch = Random.Range(1.8f, 2.2f); // Hafif varyasyon
                hareketSource.volume = 0.5f; // Çok yüksek olmasın
                hareketSource.Play();
            }
        }
    }

    public void HareketSesiDurdur()
    {
        if (hareketSource != null)
        {
            hareketSource.Stop();
        }
    }
}