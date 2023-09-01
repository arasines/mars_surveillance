namespace obs_test.Domain.Enums;

[Serializable]
public enum RobotCommand
{
    F, // Move Forward
    B, // Move Backward
    L, // Turn Left
    R, // Turn Right
    S, // Take Sample
    E // Extend Solar Panels
}