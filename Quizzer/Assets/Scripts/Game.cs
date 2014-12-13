using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;


[XmlRoot]
public class Game {

    [XmlAttribute]
    public string ID = "";

    [XmlAttribute]
    public string Turn = "";

    [XmlAttribute]
    public string Player1 = "";

    [XmlAttribute]
    public string Player1Correct = "";

    [XmlAttribute]
    public string Player2 = "";

    [XmlAttribute]
    public string Player2Correct = "";

    [XmlAttribute]
    public string Round = "";

    [XmlAttribute]
    public string Active = "";

    [XmlAttribute]
    public string Classroom = "";

    public Game() { }
    public Game(string me, string them)
    {
        Player1 = me;
        Player2 = them;
    }
    public string ToXml()
    {
        XmlSerializer xmls = new XmlSerializer(typeof(Game));
        StringWriter writer = new StringWriter();
        xmls.Serialize(writer, this);
        writer.Close();
        return writer.ToString(); 
    }

}
