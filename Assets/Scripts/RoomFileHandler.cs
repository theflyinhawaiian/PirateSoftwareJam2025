using System.IO;
using System.Linq;
using Assets.Model;

public class RoomFileHandler
{
    private readonly string RoomDir = $"Resources\\Rooms";
    private void EnsureRoomFolderExists() => FileHandler.CreateDir(RoomDir);

    public RoomFileHandler()
    {
        EnsureRoomFolderExists();
    }

    public void SaveRoom(Room room, string roomName)
    {
        var fileName = roomName;
        fileName += ".json";
        var relPath = Path.Combine(RoomDir, fileName);
        FileHandler.SaveToJSON(room, relPath);
    }

    public Room LoadRoom(string roomName)
    {
        var path = roomName;
        if (!Path.HasExtension(path))
            path += ".json";

        return FileHandler.ReadFromJSON<Room>(Path.Combine(RoomDir, path));
    }

    public string[] FetchRooms() => FileHandler.GetFiles(RoomDir).Select(x => x.Split("\\").Last().Split(".").First()).ToArray();
}
