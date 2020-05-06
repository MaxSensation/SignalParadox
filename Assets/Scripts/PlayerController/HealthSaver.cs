
public static class HealthSaver
{
    private static int _health;
    
    public static void SaveInt(int health)
    {
        _health = health;
    }

    public static int LoadInt()
    {
        return _health;
    }
}
