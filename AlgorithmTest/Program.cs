Console.WriteLine("Please Input Warrior (Example: 10 swordsman, 6 archers, 3 mages, 5 healers)");
string input = Console.ReadLine(); //"10 swordsman, 6 archers, 3 mages, 5 healers";

string[] splitInput = input.Split(",");

List<Warrior> warriorList = new List<Warrior>();

foreach (var part in splitInput)
{
    var splitWord = part.Trim().Split(" ");
    Warrior warrior = new Warrior
    {
        Name = splitWord[1].ToLower().TrimEnd('s'),
        Total = Convert.ToInt32(splitWord[0])
    };
    warriorList.Add(warrior);
}

var result = CreateTeams(warriorList);

int dragon = 1;
foreach (var team in result)
{
    Console.WriteLine($"Dragon {dragon++}: " +
        $"{team["swordsman"]} swordsman, " +
        $"{team["archer"]} archers, " +
        $"{team["mage"]} mages, " +
        $"{team["healer"]} healers");
}

static List<Dictionary<string, int>> CreateTeams(List<Warrior> warriors)
{
    var result = new List<Dictionary<string, int>>();
    var pool = warriors.ToDictionary(w => w.Name, w => w.Total);

    while (pool.Values.Sum() > 0)
    {
        var team = new Dictionary<string, int>
        {
            { "swordsman", 0 },
            { "archer", 0 },
            { "mage", 0 },
            { "healer", 0 }
        };

        foreach (var role in team.Keys.ToList())
        {
            if (pool[role] > 0 && team.Values.Sum() < 5)
            {
                team[role]++;
                pool[role]--;
            }
        }

        var priority = new[] { "swordsman", "archer", "mage", "healer" };

        foreach (var role in priority)
        {
            while (team.Values.Sum() < 5 &&
                    pool[role] > 0 &&
                    team[role] < 2)
            {
                team[role]++;
                pool[role]--;
            }
        }

        foreach (var role in priority)
        {
            while (team.Values.Sum() < 5 && pool[role] > 0)
            {
                team[role]++;
                pool[role]--;
            }
        }

        result.Add(team);
    }

    return result;
}

public class Warrior
{
    public string Name { get; set; }
    public int Total { get; set; }
}