namespace FiszkIt.Domain;

public class Flashcard
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public Flashcard(string question, string answer)
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