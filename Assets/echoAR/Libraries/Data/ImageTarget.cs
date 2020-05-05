[System.Serializable]
public class ImageTarget : Target
{

    private string filename;
    private string storageID;
    private string encodedFile;

    public ImageTarget() : base()
    {
        setType(targetType.IMAGE_TARGET);
    }

    public string getFilename()
    {
        return filename;
    }

    public void setFilename(string filename)
    {
        this.filename = filename;
    }

    public string getStorageID()
    {
        return storageID;
    }

    public void setStorageID(string storageID)
    {
        this.storageID = storageID;
    }

    public string getEncodedFile()
    {
        return encodedFile;
    }

    public void setEncodedFile(string encodedFile)
    {
        this.encodedFile = encodedFile;
    }
}