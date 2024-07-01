public class PlayMode
{
    public enum PlayModeType {PlayVsPC, PlayLocal, PlayOnline}
    public static PlayModeType selectedPlayMode;

    public static bool IsOnline {
        get {
            return selectedPlayMode == PlayModeType.PlayOnline;
        }
    }
}
