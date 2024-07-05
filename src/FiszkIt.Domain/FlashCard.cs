using FiszkIt.Core;

namespace FiszkIt.Domain;

public class FlashCard : ValueObject
{
    public string Question { get; private set; }
    public string Answer { get; private set; }

    public FlashCard(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }

    public void Update(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Question;
        yield return Answer;
    }
}