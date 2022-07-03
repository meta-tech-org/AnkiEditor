
using CrowdAnkiSchema.Model;
using RussischA1_2Fixer;

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

foreach (var subDeckALevel in root.children)
{
    foreach(var subDeckChapter in subDeckALevel.children)
    {
        var subdeckVocab = subDeckChapter.GetSubDeckByTitle("01 Vokabeln");
        var subdeckGrammar = subDeckChapter.GetSubDeckByTitle("02 Grammatik");
        
        foreach(var verb in subdeckVocab.GetNotesByNoteModel(noteModelVocab).Where(v => conjugations.Any(c => c.bare == v.fields[0])))
        {
        }
    }
}



// Export
root.WriteToFile(deckPath);