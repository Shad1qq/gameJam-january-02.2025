public class Pistol : Shooting
{
    internal bool shot;

    private void Awake()
    {
        coolDown = startcoolDown;
    }
    public void FixUpdate()
    {
        FireRayCasts();

        if (!shot || coolDown > 0)
            return;

        Shoot();
        coolDown = startcoolDown;
    }
}
