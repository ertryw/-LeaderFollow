using UnityEngine;

public class UnitStats
{
    public int Id { get; set; }
    public float Speed { get; set; }
    public float Stamina { get; set; }
    public float Mobility { get; set; }

    public UnitStats(GameLimits gameLimits)
    {
        Id = Random.Range(0, 100000);
        Speed = Random.Range(gameLimits.speed.min, gameLimits.speed.max);
        Stamina = Random.Range(gameLimits.stamina.min, gameLimits.stamina.max);
        Mobility = Random.Range(gameLimits.mobility.min, gameLimits.mobility.max);
    }

    public UnitStats(UnitData data)
    {
        Id = data.id;
        Speed = data.speed;
        Stamina = data.stamina;
        Mobility = data.mobility;
    }

    public UnitData Data(Transform transform, float baseStamina, bool leader) 
    {
        return new UnitData()
        { 
            id = Id,
            leader = leader,
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z,
            rX = transform.rotation.eulerAngles.x,
            rY = transform.rotation.eulerAngles.y,
            rZ = transform.rotation.eulerAngles.z,
            speed = Speed,
            stamina = Stamina,
            baseStamina = baseStamina,
            mobility = Mobility
        };
    }
}

[System.Serializable]
public class UnitData
{
    public bool leader;
    public float x,y,z;
    public float rX, rY, rZ;
    public int id;
    public float speed;
    public float stamina;
    public float baseStamina;
    public float mobility;
}
