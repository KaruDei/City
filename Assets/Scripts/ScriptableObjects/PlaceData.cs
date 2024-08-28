using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceData", menuName = "Data/Place", order = 0)]
public class PlaceData : ScriptableObject
{
	[Header("Place Info")]
    public string PlaceName;
    public Sprite PlaceImage;
    public string PlaceInfo;

    [Header("Place Image Sprites")]
    public Sprite PlaceImageDone;
    public Sprite PlaceImageActive;
    public Sprite PlaceImageDisabled;

    [Header("Games Data")]
	public List<PuzzleData> PuzzleDataList;
	public List<QuizData> QuizDataList;
	public List<RebusData> RebusDataList;

	[Header("Done")]
    public List<PuzzleData> PuzzleDoneList;
    public List<QuizData> QuizDoneList;
    public List<RebusData> RebusDoneList;

    [Header("Fail")]
    public List<PuzzleData> PuzzleFailList;
    public List<QuizData> QuizFailList;
    public List<RebusData> RebusFailList;

    public void ResetPlace()
    {
        PuzzleDoneList.Clear();
        QuizDoneList.Clear();
        RebusDoneList.Clear();
        PuzzleFailList.Clear();
        QuizFailList.Clear();
        RebusFailList.Clear();
    }

    public int GetGamesLevelsCount()
    {
        return PuzzleDataList.Count + QuizDataList.Count + RebusDataList.Count;
    }

    public int GetDoneGamesLevelsCount()
    {
        return PuzzleDoneList.Count + QuizDoneList.Count + RebusDoneList.Count;
    }

    public int GetFailGamesLevelsCount()
    {
        return PuzzleFailList.Count + QuizFailList.Count + RebusFailList.Count;
    }
}
