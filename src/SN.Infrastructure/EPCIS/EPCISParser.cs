namespace SN.Infrastructure.EPCIS;

public static class EPCISParser
{
    public static string? GetGTINfromEPCCode(string epccode)
    {
        if (!epccode.Contains("(01)")) return null;

        int index01 = epccode.IndexOf("(01)") + "(01)".Length;
        int index21 = epccode.IndexOf("(21)");
        return epccode.Substring(index01, index21 - index01);
    }

    public static string? GetSerialCodefromEPCCode(string epccode)
    {
        if (epccode.Contains("(21)"))
        {
            int index21 = epccode.IndexOf("(21)") + "(21)".Length;
            return epccode.Substring(index21);
        }
        else if (epccode.Contains("(00)"))
        {
            return epccode.Replace("(", "").Replace(")", "");
        }
        return null;
    }
}