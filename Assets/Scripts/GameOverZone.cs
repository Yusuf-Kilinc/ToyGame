using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverZone : MonoBehaviour
{
    [Header("Game Over Ayarları")]
    public float beklemeSuresi = 3f;

    private float sayac = 0f;
    private int icerdekiAskerSayisi = 0;
    private bool oyunBitti = false;

    void Update()
    {
        if (oyunBitti) return;

        if (icerdekiAskerSayisi > 0)
        {
            sayac += Time.deltaTime;
            if (sayac >= beklemeSuresi)
            {
                OyunuBitir();
            }
        }
        else
        {
            sayac = 0f;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ToysMerge>() != null)
        {
            icerdekiAskerSayisi++;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<ToysMerge>() != null)
        {
            icerdekiAskerSayisi--;

            if (icerdekiAskerSayisi < 0)
            {
                icerdekiAskerSayisi = 0;
            }
        }
    }
    void OyunuBitir()
    {
        oyunBitti = true;
        Debug.Log("GAME OVER BROM! Askerler kutudan taştı!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}