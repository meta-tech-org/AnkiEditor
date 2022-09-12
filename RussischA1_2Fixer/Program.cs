
using CrowdAnkiSchema.Model;
using RussischA1_2Fixer;
using System.Text.RegularExpressions;

string deckPath = @"C:\Users\juliu\source\repos\Anki Exports\Russisch_-_Olga_Schöne_-_Ein_guter_Anfang\deck.json";

string wordPath = @"I:\Google Drive\Documents Unordered\openrussian-csv\words.csv";
string conjugationsPath = @"I:\Google Drive\Documents Unordered\openrussian-csv\conjugations.csv";

var conjugations = File.ReadAllLines(conjugationsPath).Skip(1).Select(l =>
{
    var parts = l.Split('\t');
    return new OpenRussianVerb
    {
        id = int.Parse(parts[1]),
        accented = "",
        sg1 = parts[2],
        sg2 = parts[3],
        sg3 = parts[4],
        pl1 = parts[5],
        pl2 = parts[6],
        pl3 = parts[7],
    };
}).ToList();

var words = File.ReadAllLines(wordPath).Skip(1).Select(l =>
{
    var parts = l.Split('\t');
    return new OpenRussianVerb
    {
        id = int.Parse(parts[0]),
        accented = parts[3],
    };
}).ToList();

foreach (var verb in conjugations)
{
    verb.accented = words.First(w => w.id == verb.id)?.accented;
}

// Import
Deck root = Deck.LoadFromFile(deckPath);

var noteModelVocab = root.GetNoteModelByTitle("Russisch::Wort");
var noteModelVerb = root.GetNoteModelByTitle("Russisch::Verb");

foreach (var subDeckALevel in root.children)
{
    Console.WriteLine("Starting " + subDeckALevel.name);
    foreach (var subDeckChapter in subDeckALevel.children)
    {
        Console.WriteLine("Starting " + subDeckChapter.name);
        var subdeckVocab = subDeckChapter.GetSubDeckByTitle("01 Vokabeln");
        var subdeckGrammar = subDeckChapter.GetSubDeckByTitle("02 Grammatik");

        foreach (var word in subdeckVocab.GetNotesByNoteModel(noteModelVocab))
        {
            foreach (var conjugation in conjugations)
            {
                if (conjugation.bare == StripHTML(word.ValueMain))
                {
                    //word is verb
                    if (!subdeckGrammar.notes.Any(n => n.ValueMain == word.ValueMain && n.note_model_uuid == noteModelVerb.crowdanki_uuid)){
                        //verb doesn't exist yet
                        var tagList = word.tags;
                        tagList.Add("Russisch::Verb");
                        subdeckGrammar.AddNote(noteModelVerb.crowdanki_uuid, new List<string>()
                        {
                            word.fields.ElementAt(0), //Russian
                            word.fields.ElementAt(2), //German
                            ToAnkiAccentation( conjugation.sg1), //Singular 1st
                            ToAnkiAccentation(conjugation.sg2),
                            ToAnkiAccentation(conjugation.sg3),
                            ToAnkiAccentation(conjugation.pl1),
                            ToAnkiAccentation(conjugation.pl2),
                            ToAnkiAccentation(conjugation.pl3),
                        }, tagList);
                    }
                }
            }
        }
    }
}



// Export
root.WriteToFile(deckPath);

static string ToOpenRussianAccentation(string input)
{
    return input.Replace("</font>", "'").Replace("<font color=\"#ff0000\">", "");
}

static string ToAnkiAccentation(string input)
{
    var startIndex = input.IndexOf("'");
    if (startIndex == -1)
    {
        return input;
    }
    try
    {

        var intermediate = input.Replace("'", "</font>");
        var result = intermediate.Insert(startIndex - 1, "<font color=\"#ff0000\">");
        return result;

    }
    catch (Exception e)
    {
        Console.WriteLine("Couldn't insert value for " + input + " " + startIndex);
        return null;
    }
}

static string StripHTML(string input)
{
    return Regex.Replace(input, "<.*?>", String.Empty);
}