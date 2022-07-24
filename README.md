# Edit Anki Decks Programatically

## Requirements
Download https://github.com/Stvad/CrowdAnki or https://ankiweb.net/shared/info/1788670778 and export the deck you want to edit to a CrowdAnki json file

Either write your hacky scripts into AnkiEditor/Program.cs like a maniac or add your own project and reference the ```CrowdAnkiSchema``` project.

## Usage

Load a deck like this:

```
Deck root = Deck.LoadFromFile("path_to_deck.json");
```

(undocumented) usage examples can be found in AnkiEditor/Program.cs.

How to get an existing deck config (how many new/review cards you see etc.):

```
var chapterConfig = root.GetDeckConfigurationByTitle("Japanisch::Kapitel");
```

How to iterate over all subdecks:

```
foreach (var subDeck in root.children)
{
  ...
}
```

How to add new subdecks:

```
root.children.Add(Deck.CreateEmptyDeck("My new subdeck", "268c3ebf-2d30-11ec-b5f4-0c7a15ee466f"));
```

Note: the uuid is a deck config ID. Either find the deck config as shown above and use the `crowdanki_uuid` property

How to add a note:
```
var noteModel = root.GetNoteModelByTitle("My note model, stuff like "Basic"");
root.AddNote(noteModel.crowdanki_uuid, stringListOfAllFieldValues, tagList);
```

How to export the deck to disk:
```
root.WriteToFile("target.json");
```
