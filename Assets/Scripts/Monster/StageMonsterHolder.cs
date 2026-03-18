using System.Collections.Generic;

public class StageMonsterHolder : Singleton<StageMonsterHolder>
{
    public List<Monster> Monsters = new List<Monster>();

    public void KilledBySupportFire()
    {
        foreach (Monster monster in Monsters)
        {
            if (!monster.InsideTheBase && !monster.isDying)
                monster.HitByPlayer(100f, ShotType.HEADSHOT);
        }
    }

    public void AddMonster(Monster monster)
    {
        Monsters.Add(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        Monsters.Remove(monster);
    }

    public List<Monster> GetInsideMonsters()
    {
        List<Monster> monsters = new List<Monster>();

        foreach (Monster monster in Monsters)
        {
            if(monster.InsideTheBase && !monster.isDying)
                monsters.Add(monster);
        }

        return monsters;
    }
}