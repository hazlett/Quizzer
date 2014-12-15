using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System;


[XmlRoot]
public class Game {

    [XmlIgnore]
    internal static readonly string WinState = "111111";

    [XmlAttribute]
    public string ID = "";

    [XmlAttribute]
    public string Turn = "";

    [XmlAttribute]
    public string Player1 = "";

    [XmlAttribute]
    public string Player1Correct = "0";

    [XmlAttribute]
    public string Player1Totals = "000000";

    [XmlAttribute]
    public string Player2 = "";

    [XmlAttribute]
    public string Player2Correct = "0";

    [XmlAttribute]
    public string Player2Totals = "000000";

    [XmlAttribute]
    public string Round = "";

    [XmlAttribute]
    public string Active = "";

    [XmlAttribute]
    public string DateCreated = "";

    [XmlAttribute]
    public string Classroom = "";

    public Game() { }
    public Game(string me, string them, string classroom)
    {
        Player1 = me;
        Player2 = them;
        Classroom = classroom;
        Player2Totals = (Player2Totals.ToCharArray()[2] = 'c').ToString();
        DateCreated = DateTime.Now.ToString();
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
