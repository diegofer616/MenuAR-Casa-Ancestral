using UnityEngine;

public class VerificarPrimeraVez : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static VerificarPrimeraVez instance;
    bool primeraVez = true;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPrimeraVez()
    {
        primeraVez = false;
    }
    public bool EsPrimeraVez()
    {
        return primeraVez;
    }
}
