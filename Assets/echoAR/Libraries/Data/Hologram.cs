[System.Serializable]
public class Hologram
{

    public enum hologramType { VIDEO_HOLOGRAM, ECHO_HOLOGRAM, MODEL_HOLOGRAM };

    private string id;
    private hologramType type;
    private Target target; // Will not be serialized to avoid cycles
    private string targetID;

    public Hologram() { }

    public string getId()
    {
        return id;
    }

    public void setId(string id)
    {
        this.id = id;
    }

    public hologramType getType()
    {
        return type;
    }

    public void setType(hologramType type)
    {
        this.type = type;
    }

    public Target getTarget()
    {
        return target;
    }

    public void setTarget(Target target)
    {
        this.target = target;
        if (target != null)
        {
            setTargetID(target.getId());
            target.addHologram(this);
        };
    }

    public string getTargetID()
    {
        return targetID;
    }

    public void setTargetID(string targetID)
    {
        this.targetID = targetID;
    }

    public string toString()
    {
        return "Hologram{" +
                "id='" + id + '\'' +
                ", type=" + type +
                '}';
    }
}