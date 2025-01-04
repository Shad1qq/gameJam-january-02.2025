using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public ControlPlits pl;
    int[] i = new int[1];

    void Start()
    {
        pl.Init();
        i[0] = pl.plits.Length - 1;

        pl.UpdateColorPlits(i);
        Invoke(nameof(Up), 2f);
    }
    private void Up()
    {
        pl.UpdatePositionPlits(i);
    }
}
