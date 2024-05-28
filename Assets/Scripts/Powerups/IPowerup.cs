public interface IPowerup
{
    
    // Mettere bool anziché void?
    public void Activate(float wait);

    // FORSE METTERE IN ALTRA INTERFACCIA
    // POWER-UP SALUTE e altri non hanno bisogno di deattivazione
    public void Deactivate(float wait);

    // public bool IsActive();
    
}
