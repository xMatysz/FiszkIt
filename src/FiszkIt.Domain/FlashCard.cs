using FiszkIt.Core;

namespace FiszkIt.Domain;

public class FlashCard : Entity
{
    public string Question { get; private set; }
    public string Answer { get; private set; }

    public FlashCard(string question, string answer)
        : base(Guid.NewGuid())
    {
        Question = question;
        Answer = answer;
    }

    public void Update(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }
}