
namespace Core.Data
{
    public enum GameCondition
    {
        None,
        IsClockQuestComplete,
        IsPhotoQuestComplete,
        ClockDoorOpen,
        PhotoDoorOpen,
        PolaroidTaken,
        LetterClue6,
        PhotosClue1,
        RockClue4,
        CameraGirlClue2,
        TeleportAvailable,
        PillsClue5,
        IsFirstLoopsCompleted,
        IsFirstTimeInClockScene,
        #region Music Quest
        MusicSafeDoorOpen,
        AllMusicNotesCollected,
        MusicNote1,
        MusicNote2,
        MusicNote3,
        MusicNote4,
        IsMusicQuestComplete,
        #endregion
        #region Final Quest
        WordGroup1,
        WordGroup2,
        WordGroup3,
        FinalQuestCompleted,
        #endregion
        FirstTimeInMindPlace,
        LOOP4,
        FirstTimeLoop4,
        #region UI_CHAPTER_TITLE
        Chapter0,
        Chapter1,
        Chapter2,
        Chapter3,
        Chapter4,
        #endregion
        AllNpcsLockedSpoken,
        PrologueDoorsLocked,
        WalkManTaken,
        StayMonologueQueued,
    }
}