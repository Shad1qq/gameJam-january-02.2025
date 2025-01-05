public class Pistol : Shooting
{
    internal bool shot;

    private void Awake()
    {
        coolDown = startcoolDown;
    }
    public void FixUpdate()
    {
        if (!shot || coolDown > 0)
            return;

        FireRayCasts();

        Shoot();
        coolDown = startcoolDown;
    }
}
