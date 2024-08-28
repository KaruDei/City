using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizData", menuName = "Data/Quiz", order = 0)]
public class QuizData : ScriptableObject
{
	[Header("Parameters")]
	public string Question;
	public bool FiftyFiftyFuntion = true;
	public bool CorrectOrderFuntion;

	[Header("Ansvers")]
	public List<string> AnsversList;

	[Header("CorrectOrder")]
	public List<string> CorrectAnsverList;

    private void OnValidate()
    {
		if (FiftyFiftyFuntion && CorrectOrderFuntion)
			FiftyFiftyFuntion = false;
		else if (!FiftyFiftyFuntion && !CorrectOrderFuntion)
            FiftyFiftyFuntion = true;
        else if (!CorrectOrderFuntion)
			CorrectAnsverList.Clear();
    }
}
