using UnityEngine;
using System.Collections;

public class transformSerializable
{
    public float tx;
    public float ty;
    public float rx;
    public float ry;
    public float rw;
}

public interface NetworkInterface {
    void sendWishesToServer(bool state);
    void receptWishesFromServer(byte[] wishes);
}
